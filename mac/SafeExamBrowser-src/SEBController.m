//
//  SEBController.m
//  Safe Exam Browser
//
//  Created by Daniel R. Schneider on 29.04.10.
//  Copyright (c) 2010-2011 Daniel R. Schneider, ETH Zurich, 
//  Educational Development and Technology (LET), 
//  based on the original idea of Safe Exam Browser 
//  by Stefan Schneider, University of Giessen
//  Project concept: Thomas Piendl, Daniel R. Schneider, 
//  Dirk Bauer, Karsten Burger, Marco Lehre, 
//  Brigitte Schmucki, Oliver Rahs. French localization: Nicolas Dunand
//
//  ``The contents of this file are subject to the Mozilla Public License
//  Version 1.1 (the "License"); you may not use this file except in
//  compliance with the License. You may obtain a copy of the License at
//  http://www.mozilla.org/MPL/
//  
//  Software distributed under the License is distributed on an "AS IS"
//  basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//  License for the specific language governing rights and limitations
//  under the License.
//  
//  The Original Code is Safe Exam Browser for Mac OS X.
//  
//  The Initial Developer of the Original Code is Daniel R. Schneider.
//  Portions created by Daniel R. Schneider are Copyright 
//  (C) 2010-2011 Daniel R. Schneider, ETH Zurich, Educational Development
//  and Technology (LET), based on the original idea of Safe Exam Browser 
//  by Stefan Schneider, University of Giessen. All Rights Reserved.
//  
//  Contributor(s): ______________________________________.
//

#include <Carbon/Carbon.h>
#import "SEBController.h"

#import <IOKit/pwr_mgt/IOPMLib.h>

#include <ctype.h>
#include <stdlib.h>
#include <stdio.h>

#include <mach/mach_port.h>
#include <mach/mach_interface.h>
#include <mach/mach_init.h>

#include <IOKit/pwr_mgt/IOPMLib.h>
#include <IOKit/IOMessage.h>

io_connect_t  root_port; // a reference to the Root Power Domain IOService


OSStatus MyHotKeyHandler(EventHandlerCallRef nextHandler,EventRef theEvent,id sender);
void MySleepCallBack(void * refCon, io_service_t service, natural_t messageType, void * messageArgument);

@implementation SEBController

@synthesize f3Pressed;	//create getter and setter for F3 key pressed flag
@synthesize quittingMyself;	//create getter and setter for flag that SEB is quitting itself


#pragma mark Initialization

- (void)awakeFromNib {	
	// Flag initializing
	quittingMyself = FALSE; //flag to know if quit application was called externally
    
// Save the bundle ID of all currently running apps which are visible in a array 
	NSArray *runningApps = [[NSWorkspace sharedWorkspace] runningApplications];
    NSRunningApplication *iterApp;
    visibleApps = [NSMutableArray array]; //array for storing bundleIDs of visible apps
    [visibleApps retain];

    for (iterApp in runningApps) 
    {
        BOOL isHidden = [iterApp isHidden];
        NSString *appBundleID = [iterApp valueForKey:@"bundleIdentifier"];
        if ((appBundleID != nil) & !isHidden) {
            [visibleApps addObject:appBundleID]; //add ID of the visible app
        }
    }

// Setup Notifications and Kiosk Mode    
    
    // Hide all other applications
	[[NSWorkspace sharedWorkspace] performSelectorOnMainThread:@selector(hideOtherApplications) 
													withObject:NULL waitUntilDone:NO];
	
    // Add an observer for the notification that another application became active (SEB got inactive)
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(regainActiveStatus:) 
												 name:NSApplicationDidResignActiveNotification 
                                               object:NSApp];
	
    // Add an observer for the notification that another application was unhidden by the finder
	NSWorkspace *workspace = [NSWorkspace sharedWorkspace];
	[[workspace notificationCenter] addObserver:self 
                                       selector:@selector(regainActiveStatus:) 
                                           name:NSWorkspaceDidUnhideApplicationNotification 
                                         object:workspace];
	
// Switch to kiosk mode by setting the proper presentation options
	[self startKioskMode];
	
    // Add an observer for changes of the Presentation Options
	[NSApp addObserver:self
			forKeyPath:@"currentSystemPresentationOptions"
			   options:NSKeyValueObservingOptionNew
			   context:NULL];
		
// Cover all attached screens with cap windows to prevent clicks on desktop making finder active
	[self coverScreens];
    
    // Add a observer for changes of the screen configuration
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(adjustScreenLocking:) 
												 name:NSApplicationDidChangeScreenParametersNotification 
                                               object:NSApp];
	

	NSUserDefaults *preferences = [NSUserDefaults standardUserDefaults];
	if ([preferences objectForKey:@"startURL"] == nil) {
		firstStart = YES;
		// Set default preferences for the case there are no user prefs yet
		NSDictionary *appDefaults = [NSDictionary dictionaryWithObjectsAndKeys:
									 @"http://www.safeexambrowser.org/macosx", @"startURL", 
									 [NSData data], @"hashedAdminPassword",
									 [NSData data], @"hashedQuitPassword", 
									 [NSNumber numberWithBool:YES], @"allowQuit", 
                                     [NSNumber numberWithBool:NO], @"allowSwitchToThirdPartyApps",
                                     [NSNumber numberWithBool:NO], @"allowDownUploads",
                                     [NSHomeDirectory() stringByAppendingPathComponent:  @"Downloads"], @"downloadDirectory",
                                     [NSNumber numberWithBool:NO], @"openDownloads",
                                     nil];
		[preferences registerDefaults:appDefaults];
	} else {
		firstStart = NO;
	}
    
    // Add an observer for the request to conditionally exit SEB
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(exitSEB:)
                                                 name:@"requestExitNotification" object:nil];
	
    // Add an observer for the request to unconditionally quit SEB
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(requestedQuit:)
                                                 name:@"requestQuitNotification" object:nil];
	
    // Add an observer for the request to reload start URL
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(requestedRestart:)
                                                 name:@"requestRestartNotification" object:nil];
	
    // Add an observer for the request to show about panel
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(requestedShowAbout:)
                                                 name:@"requestShowAboutNotification" object:nil];
	
    // Add an observer for the request to show help
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(requestedShowHelp:)
                                                 name:@"requestShowHelpNotification" object:nil];

    // Add an observer for the request to show help
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(preferencesClosed:)
                                                 name:@"preferencesClosed" object:nil];

// Prevent display sleep
	IOReturn success = IOPMAssertionCreateWithName(
		kIOPMAssertionTypeNoDisplaySleep,										   
		kIOPMAssertionLevelOn, 
		CFSTR("Safe Exam Browser Kiosk Mode"), 
		&assertionID1); 
#ifdef DEBUG
	if (success == kIOReturnSuccess) {
		NSLog(@"Display sleep is switched off now.");
	}
#endif		
	
/*	// Prevent idle sleep
	success = IOPMAssertionCreateWithName(
		kIOPMAssertionTypeNoIdleSleep, 
		kIOPMAssertionLevelOn, 
		CFSTR("Safe Exam Browser Kiosk Mode"), 
		&assertionID2); 
#ifdef DEBUG
	if (success == kIOReturnSuccess) {
		NSLog(@"Idle sleep is switched off now.");
	}
#endif		
*/	
	// Installing I/O Kit sleep/wake notification to cancel sleep
	
	IONotificationPortRef notifyPortRef; // notification port allocated by IORegisterForSystemPower
    io_object_t notifierObject; // notifier object, used to deregister later
    void* refCon; // this parameter is passed to the callback
	
    // register to receive system sleep notifications

    root_port = IORegisterForSystemPower( refCon, &notifyPortRef, MySleepCallBack, &notifierObject );
    if ( root_port == 0 )
    {
        NSLog(@"IORegisterForSystemPower failed");
    } else {
	    // add the notification port to the application runloop
		CFRunLoopAddSource( CFRunLoopGetCurrent(),
					   IONotificationPortGetRunLoopSource(notifyPortRef), kCFRunLoopCommonModes ); 
	}
	
// Check if SEB is running inside a virtual machine
    SInt32		myAttrs;
	OSErr		myErr = noErr;
	
	// Get details for the present operating environment
	// by calling Gestalt (Userland equivalent to CPUID)
	myErr = Gestalt(gestaltX86AdditionalFeatures, &myAttrs);
	if (myErr == noErr) {
		if ((myAttrs & (1UL << 31)) | (myAttrs == 0x209)) {
			// Bit 31 is set: VMware Hypervisor running (?)
            // or gestaltX86AdditionalFeatures values of VirtualBox detected
#ifdef DEBUG
            NSLog(@"SERIOUS SECURITY ISSUE DETECTED: SEB was started up in a virtual machine! gestaltX86AdditionalFeatures = %X", myAttrs);
#endif
            NSRunAlertPanel(NSLocalizedString(@"Virtual Machine detected!", nil), 
                            NSLocalizedString(@"You are not allowed to run SEB inside a virtual machine!", nil), 
                            NSLocalizedString(@"Quit", nil), nil, nil);
            quittingMyself = TRUE; //SEB is terminating itself
            [NSApp terminate: nil]; //quit SEB
            
#ifdef DEBUG
		} else {
            NSLog(@"SEB is running on a native system (no VM) gestaltX86AdditionalFeatures = %X", myAttrs);
#endif
        }
	}
   
// Set up SEB Browser 
    
    // Maximize the browser window
    // (this is done here, after presentation options are set,
    // because otherwise menu bar and dock are deducted from screen size)
	[browserWindow
	 setFrame:[browserWindow frameRectForContentRect:[[browserWindow screen] frame]]
	 display:YES];
	[NSApp activateIgnoringOtherApps: YES];
	[browserWindow makeKeyAndOrderFront:self];
    
	// Load start URL from the system's user defaults database
	NSString *urlText = [preferences stringForKey:@"startURL"];
	
    // Add "SEB" to the browser's user agent, so the LMS SEB plugins recognize us
	NSString *customUserAgent = [webView userAgentForURL:[NSURL URLWithString:urlText]];
	[webView setCustomUserAgent:[customUserAgent stringByAppendingString:@" SEB"]];
    
	// Load start URL into browser window
	[[webView mainFrame] loadRequest:[NSURLRequest requestWithURL:[NSURL URLWithString:urlText]]];
    
    
	// Due to the infamous Flash plugin we completely disable plugins in the 32-bit build
#ifdef __i386__        // 32-bit Intel build
	[[webView preferences] setPlugInsEnabled:NO];
#endif
	
	if (firstStart) {
		NSString *titleString = NSLocalizedString(@"Important Notice for First Time Users", nil);
#ifdef __i386__        // 32-bit Intel build needs more information
		NSString *messageString = [NSString stringWithFormat:@"%@%@", NSLocalizedString(@"32bitDisclaimer", nil), NSLocalizedString(@"FirstTimeUserNotice", nil)];
#else
		NSString *messageString = NSLocalizedString(@"FirstTimeUserNotice", nil);
#endif
		NSRunAlertPanel(titleString, messageString, NSLocalizedString(@"OK", nil), nil, nil);
		
	}
    
// Handling of Hotkeys for Preferences-Window
	
	// Register Carbon event handlers for the required hotkeys
	f3Pressed = FALSE; //Initialize flag for first hotkey
	EventHotKeyRef gMyHotKeyRef;
	EventHotKeyID gMyHotKeyID;
	EventTypeSpec eventType;
	eventType.eventClass=kEventClassKeyboard;
	eventType.eventKind=kEventHotKeyPressed;
	InstallApplicationEventHandler((void*)MyHotKeyHandler, 1, &eventType, (void*)self, NULL);	
    //Pass pointer to flag for F3 key to the event handler
	// Register F3 as a hotkey
	gMyHotKeyID.signature='htk1';
	gMyHotKeyID.id=1;
	RegisterEventHotKey(99, 0, gMyHotKeyID,
						GetApplicationEventTarget(), 0, &gMyHotKeyRef);
	// Register F6 as a hotkey
	gMyHotKeyID.signature='htk2';
	gMyHotKeyID.id=2;
	RegisterEventHotKey(97, 0, gMyHotKeyID,
						GetApplicationEventTarget(), 0, &gMyHotKeyRef);
    
// Show the About SEB Window
    [aboutWindow setStyleMask:NSBorderlessWindowMask];
    [aboutWindow center];
	[aboutWindow orderFront:self];
    [aboutWindow setLevel:NSFloatingWindowLevel];
    
    // Close the About SEB Window after a delay
    [self performSelector:@selector(closeAboutWindow) withObject: nil afterDelay: 3];
          
}



#pragma mark Methods

// Method executed when hotkeys are pressed
OSStatus MyHotKeyHandler(EventHandlerCallRef nextHandler,EventRef theEvent,
						  id userData)
{
	EventHotKeyID hkCom;
	GetEventParameter(theEvent,kEventParamDirectObject,typeEventHotKeyID,NULL,
					  sizeof(hkCom),NULL,&hkCom);
	int l = hkCom.id;
	id self = userData;
	
	switch (l) {
		case 1: //F3 pressed
			[self setF3Pressed:TRUE];	//F3 was pressed
			
			break;
		case 2: //F6 pressed
			if ([self f3Pressed]) {	//if F3 got pressed before
				[self setF3Pressed:FALSE];
				[self openPreferences:self]; //show preferences window
			}
			break;
	}
	return noErr;
}


// Method called by I/O Kit power management
void MySleepCallBack( void * refCon, io_service_t service, natural_t messageType, void * messageArgument )
{
    printf( "messageType %08lx, arg %08lx\n",
		   (long unsigned int)messageType,
		   (long unsigned int)messageArgument );
	
    switch ( messageType )
    {
			
        case kIOMessageCanSystemSleep:
            /* Idle sleep is about to kick in. This message will not be sent for forced sleep.
			 Applications have a chance to prevent sleep by calling IOCancelPowerChange.
			 Most applications should not prevent idle sleep.
			 
			 Power Management waits up to 30 seconds for you to either allow or deny idle sleep.
			 If you don't acknowledge this power change by calling either IOAllowPowerChange
			 or IOCancelPowerChange, the system will wait 30 seconds then go to sleep.
			 */
			
            // cancel idle sleep
            IOCancelPowerChange( root_port, (long)messageArgument );
            // uncomment to allow idle sleep
            //IOAllowPowerChange( root_port, (long)messageArgument );
            break;
			
        case kIOMessageSystemWillSleep:
            /* The system WILL go to sleep. If you do not call IOAllowPowerChange or
			 IOCancelPowerChange to acknowledge this message, sleep will be
			 delayed by 30 seconds.
			 
			 NOTE: If you call IOCancelPowerChange to deny sleep it returns kIOReturnSuccess,
			 however the system WILL still go to sleep. 
			 */
			
			//IOCancelPowerChange( root_port, (long)messageArgument );
			//IOAllowPowerChange( root_port, (long)messageArgument );
            break;
			
        case kIOMessageSystemWillPowerOn:
            //System has started the wake up process...
            break;
			
        case kIOMessageSystemHasPoweredOn:
            //System has finished waking up...
			break;
			
        default:
            break;
			
    }
}


// Close the About Window
- (void) closeAboutWindow {
    [aboutWindow orderOut:self];
}


- (void) coverScreens {
	// Open background windows on all available screens to prevent Finder becoming active when clicking on the desktop background
	NSArray *screens = [NSScreen screens];	// get all available screens
    capWindows = [NSMutableArray array];	// array for storing our cap (covering) background windows
	[capWindows retain];	// don't autorelease the array
    NSScreen *iterScreen;
    NSUInteger screenIndex = 1;
    for (iterScreen in screens)
    {
		//NSRect frame = size of the current screen;
		NSRect frame = [iterScreen frame];
		NSUInteger styleMask = NSBorderlessWindowMask;
		NSRect rect = [NSWindow contentRectForFrameRect:frame styleMask:styleMask];
		//set origin of the window rect to left bottom corner (important for non-main screens, since they have offsets)
		rect.origin.x = 0;
		rect.origin.y = 0;
		NSWindow *window =  [[NSWindow alloc] initWithContentRect:rect styleMask:styleMask backing: NSBackingStoreBuffered defer:false screen:iterScreen];
		[window setBackgroundColor:[NSColor blackColor]];
		[window setSharingType: NSWindowSharingNone];  //don't allow other processes to read window contents
		[window orderBack:self];
		[capWindows addObject: window];
        NSView *superview = [window contentView];
        CapView *capview = [[CapView alloc] initWithFrame:rect];
        [superview addSubview:capview];
        [capview release];
		
        screenIndex++;
    }
}	


// Called when changes of the screen configuration occur 
// (new display is contected or removed or display mirroring activated)

- (void) adjustScreenLocking: (id)sender {
    // Close the covering windows
	// (which most likely are no longer there where they should be)
	int windowIndex;
	int windowCount = [capWindows count];
    for (windowIndex = 0; windowIndex < windowCount; windowIndex++ )
    {
		[[capWindows objectAtIndex:windowIndex] close];

	}
	[capWindows release];
	// Open new covering background windows on all currently available screens
	[self coverScreens];
}


- (void) regainActiveStatus: (id)sender {
	// hide all other applications if not in debug build setting
#ifndef DEBUG
    // Load preferences from the system's user defaults database
	NSUserDefaults *preferences = [NSUserDefaults standardUserDefaults];
	BOOL allowSwitchToThirdPartyApps = [preferences boolForKey:@"allowSwitchToThirdPartyApps"];
    if (!allowSwitchToThirdPartyApps) {
		// if switching to ThirdPartyApps not allowed
        [NSApp activateIgnoringOtherApps: YES];
        [[NSWorkspace sharedWorkspace] performSelectorOnMainThread:@selector(hideOtherApplications) withObject:NULL waitUntilDone:NO];
    }
#endif
}


- (void) startKioskMode {
	// Switch to kiosk mode by setting the proper presentation options
    // Load preferences from the system's user defaults database
	NSUserDefaults *preferences = [NSUserDefaults standardUserDefaults];
	BOOL allowSwitchToThirdPartyApps = [preferences boolForKey:@"allowSwitchToThirdPartyApps"];
    if (!allowSwitchToThirdPartyApps) {
		// if switching to ThirdPartyApps not allowed
	@try {
		NSApplicationPresentationOptions options = 
		NSApplicationPresentationHideDock + 
		NSApplicationPresentationHideMenuBar + 
		NSApplicationPresentationDisableProcessSwitching + 
		NSApplicationPresentationDisableForceQuit + 
		NSApplicationPresentationDisableSessionTermination;
		
		[NSApp setPresentationOptions:options];
	}
	@catch(NSException *exception) {
		NSLog(@"Error.  Make sure you have a valid combination of presentation options.");
	}
    } else {
        @try {
            NSApplicationPresentationOptions options =
            NSApplicationPresentationHideMenuBar +
            NSApplicationPresentationHideDock +
            NSApplicationPresentationDisableForceQuit + 
            NSApplicationPresentationDisableSessionTermination;
            
            [NSApp setPresentationOptions:options];
        }
        @catch(NSException *exception) {
            NSLog(@"Error.  Make sure you have a valid combination of presentation options.");
        }

    }
}	

- (NSString*) showEnterPasswordDialog: (NSWindow *)window {
// User has asked to see the dialog. Display it.
    [enterPassword setStringValue:@""]; //reset the enterPassword NSSecureTextField
    
    [NSApp beginSheet: enterPasswordDialog
       modalForWindow: window
        modalDelegate: nil
       didEndSelector: nil
          contextInfo: nil];
    [NSApp runModalForWindow: enterPasswordDialog];
    // Dialog is up here.
    [NSApp endSheet: enterPasswordDialog];
    [enterPasswordDialog orderOut: self];
    return ([enterPassword stringValue]);
}


- (IBAction) okEnterPassword: (id)sender {
    [NSApp stopModal];
}


- (IBAction) cancelEnterPassword: (id)sender {
    [NSApp stopModal];
    [enterPassword setStringValue:@""];
}


- (IBAction) exitSEB:(id)sender {
	// Load quitting preferences from the system's user defaults database
	NSUserDefaults *preferences = [NSUserDefaults standardUserDefaults];
	NSData *hashedQuitPassword = [preferences objectForKey:@"hashedQuitPassword"];
    if ([preferences boolForKey:@"allowQuit"] == YES) {
		// if quitting SEB is allowed
		
        if (![hashedQuitPassword isEqualToData:[NSData data]]) {
			// if quit password is set, then restrict quitting
            NSString *password = [self showEnterPasswordDialog:browserWindow];
			
            if ([hashedQuitPassword isEqualToData:[self generateSHAHash:password]]) {
				// if the correct quit password was entered
				quittingMyself = TRUE; //SEB is terminating itself
                [NSApp terminate: nil]; //quit SEB
            }
        } else {
        // if no quit password is required, then confirm quitting
            int answer = NSRunAlertPanel(NSLocalizedString(@"Quit",nil), NSLocalizedString(@"Are you sure you want to quit SEB?",nil),
                                         NSLocalizedString(@"Cancel",nil), NSLocalizedString(@"Quit",nil), nil);
            switch(answer)
            {
                case NSAlertDefaultReturn:
                    return; //Cancel: don't quit
                default:
					quittingMyself = TRUE; //SEB is terminating itself
                    [NSApp terminate: nil]; //quit SEB
            }
        }
    } 
}


- (void) openPreferences:(id)sender {
    if (![preferencesController preferencesAreOpen]) {
        // Load admin password from the system's user defaults database
        NSUserDefaults *preferences = [NSUserDefaults standardUserDefaults];
        NSData *hashedAdminPW = [preferences objectForKey:@"hashedAdminPassword"];
        if (![hashedAdminPW isEqualToData:[NSData data]]) {
            // If admin password is set, then restrict access to the preferences window  
            NSString *password = [self showEnterPasswordDialog:browserWindow];
            if (![hashedAdminPW isEqualToData:[self generateSHAHash:password]]) {
                //if hash of entered password is not equal to the one in preferences
                return;
            }         
        }

    }
	[preferencesController showPreferences:self];
}


- (void)requestedQuit:(NSNotification *)notification
{
    quittingMyself = TRUE; //SEB is terminating itself
    [NSApp terminate: nil]; //quit SEB
}


- (void)requestedRestart:(NSNotification *)notification
{
    // Load start URL from the system's user defaults database
    NSUserDefaults *preferences = [NSUserDefaults standardUserDefaults];
	NSString *urlText = [preferences stringForKey:@"startURL"];
	// Load start URL into browser window
	[[webView mainFrame] loadRequest:[NSURLRequest requestWithURL:[NSURL URLWithString:urlText]]];
}


- (void)requestedShowAbout:(NSNotification *)notification
{
    [aboutWindow setStyleMask:NSBorderlessWindowMask];
	[aboutWindow center];
	[aboutWindow orderFront:self];
    [[NSApplication sharedApplication] runModalForWindow:aboutWindow];
}


- (void)requestedShowHelp:(NSNotification *)notification
{
    // Load manual page URL into browser window
    NSString *urlText = @"http://www.safeexambrowser.org/macosx";
	[[webView mainFrame] loadRequest:
     [NSURLRequest requestWithURL:[NSURL URLWithString:urlText]]];

}


- (void)preferencesClosed:(NSNotification *)notification
{
    //preferences were closed and maybe the third party app setting was changed
    //so just in case we adjust the kiosk settings
    [self startKioskMode];
}


- (NSData*) generateSHAHash:(NSString*)inputString {
    unsigned char hashedChars[32];
    CC_SHA256([inputString UTF8String],
              [inputString lengthOfBytesUsingEncoding:NSUTF8StringEncoding], 
              hashedChars);
    NSData *hashedData = [NSData dataWithBytes:hashedChars length:32];
    return hashedData;
}


#pragma mark Delegates

// Called when SEB should be terminated
- (NSApplicationTerminateReply)applicationShouldTerminate:(NSApplication *)sender {
	if (quittingMyself) {
		return NSTerminateNow; //SEB wants to quit, ok, so it should happen
	} else { //SEB should be terminated externally(!)
		return NSTerminateCancel; //this we can't allow, sorry...
	}
}


// Called just before SEB will be terminated
- (void)applicationWillTerminate:(NSNotification *)aNotification {
    runningAppsWhileTerminating = [[NSWorkspace sharedWorkspace] runningApplications];
    NSRunningApplication *iterApp;
    for (iterApp in runningAppsWhileTerminating) 
    {
        NSString *appBundleID = [iterApp valueForKey:@"bundleIdentifier"];
        if ([visibleApps indexOfObject:appBundleID] != NSNotFound) {
            [iterApp unhide]; //unhide the originally visible application
        }
    }
	
	// Clear the browser cache in ~/Library/Caches/org.safeexambrowser.Safe-Exam-Browser/
	NSURLCache *cache = [NSURLCache sharedURLCache];
	[cache removeAllCachedResponses];

	// Allow display and system to sleep again
	IOReturn success = IOPMAssertionRelease(assertionID1);
	/*// Allow system to sleep again
	success = IOPMAssertionRelease(assertionID2);*/
}


/*- (void)windowDidResignKey:(NSNotification *)notification {
	[NSApp activateIgnoringOtherApps: YES];
	[browserWindow 
	 makeKeyAndOrderFront:self];
	#ifdef DEBUG
	NSLog(@"[browserWindow makeKeyAndOrderFront]");
	NSBeep();
	#endif
	
}
*/


// Called when currentPresentationOptions change
- (void)observeValueForKeyPath:(NSString *)keyPath
					  ofObject:id
                        change:(NSDictionary *)change
                       context:(void *)context
{
    if ([keyPath isEqual:@"currentSystemPresentationOptions"]) {
		//the current Presentation Options changed, so make SEB active and reset them
        // Load preferences from the system's user defaults database
        NSUserDefaults *preferences = [NSUserDefaults standardUserDefaults];
        BOOL allowSwitchToThirdPartyApps = [preferences boolForKey:@"allowSwitchToThirdPartyApps"];
        if (!allowSwitchToThirdPartyApps) {
            // if switching to ThirdPartyApps not allowed
            #ifdef DEBUG
            NSLog(@"currentSystemPresentationOptions changed!");
            #endif
            // Change the activation policy
            [NSApp activateIgnoringOtherApps: YES];
            [browserWindow makeKeyAndOrderFront:self];
            [self startKioskMode];
            //[self regainActiveStatus:nil];
        }
    }	
}
 
@end
