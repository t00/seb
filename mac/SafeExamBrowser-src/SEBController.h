//
//  SEBController.h
//  SEB
//
//  Created by Daniel R. Schneider on 29.04.10.
//  Copyright 2010 ETH Zurich LET. All rights reserved.
//

// Main Safe Exam Browser controller class

#import <Cocoa/Cocoa.h>
#import <WebKit/WebKit.h>
#import "PrefsController.h"
#import "CapView.h"
#import <IOKit/pwr_mgt/IOPMLib.h>

	
@interface SEBController : NSObject <NSWindowDelegate> {
	
    NSArray *runningAppsWhileTerminating;
    NSMutableArray *visibleApps;
	NSMutableArray *capWindows;
	//NSPoint testpoint;
	BOOL f3Pressed;
	BOOL quittingMyself;
	BOOL firstStart;
	IBOutlet NSWindow *browserWindow;
	IBOutlet WebView *webView;
	IBOutlet id prefsController;
	IBOutlet NSWindow *aboutWindow;
	IBOutlet NSView *passwordView;
	IBOutlet NSSecureTextField *enterPassword;
    IBOutlet NSWindow *enterPasswordDialog;
	
	IOPMAssertionID assertionID1;
	IOPMAssertionID assertionID2;

       
}

- (void) closeAboutWindow;
- (void) coverScreens;
- (void) adjustScreenLocking: (id)sender;
- (void) regainActiveStatus: (id)sender;
- (void) startKioskMode;

- (NSString *) showEnterPasswordDialog: (NSWindow *)window;
- (IBAction) okEnterPassword: (id)sender;
- (IBAction) cancelEnterPassword: (id)sender;

- (IBAction) exitSEB:(id)sender;

- (void) openPreferences:(id)sender;

- (NSData *) generateSHAHash:(NSString*)inputString;

@property(readwrite) BOOL f3Pressed;
@property(readwrite) BOOL quittingMyself;


@end
