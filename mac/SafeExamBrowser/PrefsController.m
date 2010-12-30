//
//  PrefsController.m
//  SEB
//
//  Created by Daniel R. Schneider on 06.06.10.
//  Copyright 2010 ETH Zurich LET. All rights reserved.
//

#import "PrefsController.h"


@implementation PrefsController


// Definitition of the dependent keys for comparing admin passwords
+ (NSSet *)keyPathsForValuesAffectingCompareAdminPasswords {
    return [NSSet setWithObjects:@"adminPassword", @"confirmAdminPassword", nil];
}


// Definitition of the dependent keys for comparing quit passwords
+ (NSSet *)keyPathsForValuesAffectingCompareQuitPasswords {
    return [NSSet setWithObjects:@"quitPassword", @"confirmQuitPassword", nil];
}


// Method called by the bindings object controller for comparing the admin passwords
- (NSString*) compareAdminPasswords {
	if ((adminPassword != nil) | (confirmAdminPassword != nil)) { 
        //if at least one of the fields is defined
        if ((confirmAdminPassword == @"𝈭𝈖𝈒𝉇𝈁𝉈") & (adminPassword != @"𝈭𝈖𝈒𝉇𝈁𝉈")) {
            //when the admin password was changed (user started to edit it), then the  
            //placeholder string in the confirm admin password field needs to be removed
            [self setValue:nil forKey:@"confirmAdminPassword"];
            if (adminPassword == nil) {
                //if admin pw was deleted completely, we have to return nil (pw's match)
                return nil;
            }
        }
       	if (![adminPassword isEqualToString:confirmAdminPassword]) {
			//if the two passwords don't match, show it in the label
            return (NSString*)([NSString stringWithString:NSLocalizedString(@"Please confirm password",nil)]);
		}
    }
    return nil;
}


// Method called by the bindings object controller for comparing the quit passwords
- (NSString*) compareQuitPasswords {
	if ((quitPassword != nil) | (confirmQuitPassword != nil)) { 
        //if at least one of the fields is defined
        if ((confirmQuitPassword == @"𝈭𝈖𝈒𝉇𝈁𝉈") & (quitPassword != @"𝈭𝈖𝈒𝉇𝈁𝉈")) {
            //when the quit password was changed (user started to edit it), then the  
            //placeholder string in the confirm quit password field needs to be removed
            [self setValue:nil forKey:@"confirmQuitPassword"];
            if (quitPassword == nil) {
                //if admin pw was deleted completely, we have to return nil (pw's match)
                return nil;
            }
        }
       	if (![quitPassword isEqualToString:confirmQuitPassword]) {
			//if the two passwords don't match, show it in the label
            return (NSString*)([NSString stringWithString:NSLocalizedString(@"Please confirm password",nil)]);
		}
    }
    return nil;
}


- (void) loadPrefs:(id)sender {
	// Loads preferences from the system's user defaults database
	NSUserDefaults *preferences = [NSUserDefaults standardUserDefaults];
	[startURL setStringValue:[preferences stringForKey:@"startURL"]];
    
    if ([[preferences objectForKey:@"hashedAdminPassword"] isEqualToData:[NSData data]]) {
        //empty passwords need to be set to NIL because of the text fields' bindings 
        //([NSData data] produces an empty NSData object)
        [self setValue:nil forKey:@"adminPassword"];
        [self setValue:nil forKey:@"confirmAdminPassword"];
    } else {
        //if there actually was a hashed password set, use a placeholder string
        [self setValue:@"𝈭𝈖𝈒𝉇𝈁𝉈" forKey:@"adminPassword"];
        [self setValue:@"𝈭𝈖𝈒𝉇𝈁𝉈" forKey:@"confirmAdminPassword"];
    }
    
    if ([[preferences objectForKey:@"hashedQuitPassword"] isEqualToData:[NSData data]]) {
        [self setValue:nil forKey:@"quitPassword"];
        [self setValue:nil forKey:@"confirmQuitPassword"];
    } else {
        [self setValue:@"𝈭𝈖𝈒𝉇𝈁𝉈" forKey:@"quitPassword"];
        [self setValue:@"𝈭𝈖𝈒𝉇𝈁𝉈" forKey:@"confirmQuitPassword"];
    }
    
	[allowQuit setState:(([[preferences stringForKey:@"allowQuit"] 
                           isEqualToString:@"YES"]) ? NSOnState : NSOffState)];
}


- (void) savePrefs:(id)sender {
	// Saves preferences to the system's user defaults database
	NSUserDefaults *preferences = [NSUserDefaults standardUserDefaults];
	[preferences setObject:[startURL stringValue] forKey:@"startURL"];
    if (adminPassword == nil) {
        //if no admin pw was entered, save a empty NSData object in preferences
        [preferences setObject:[NSData data] forKey:@"hashedAdminPassword"];
    } else if (adminPassword != @"𝈭𝈖𝈒𝉇𝈁𝉈") {
        //if password was changed, save the new hashed password in preferences
        [preferences setObject:[self generateSHAHash:adminPassword] forKey:@"hashedAdminPassword"];
    }
    if (quitPassword == nil) {
        //if no quit pw was entered, save a empty NSData object in preferences
        [preferences setObject:[NSData data] forKey:@"hashedQuitPassword"];
    } else if (quitPassword != @"𝈭𝈖𝈒𝉇𝈁𝉈") {
        //if password was changed, save the new hashed password in preferences
        [preferences setObject:[self generateSHAHash:quitPassword] forKey:@"hashedQuitPassword"];
    }
    //save the flag if quit is allowed or not in preferences
	[preferences setObject:(([allowQuit state] == NSOnState) ? @"YES" : @"NO") forKey:@"allowQuit"];
}


- (void) openPreferencesWindow:(id)sender{
    if (![prefsWindow isVisible]) {
        [self loadPrefs:self];	//load preferences only when preferences were not already open
        [prefsWindow center];
        [prefsWindow makeKeyAndOrderFront:self];
        [[NSApplication sharedApplication] runModalForWindow:prefsWindow];
    } else {
        [prefsWindow makeKeyAndOrderFront:self];
    }
}


- (void) closePreferencesWindow:(id)sender {
    [prefsWindow orderOut:self];
    [[NSApplication sharedApplication] stopModal];
}


- (BOOL) preferencesAreOpen {
    return [prefsWindow isVisible];
}


// Action for OK button in preferences
- (IBAction) prefsOK:(id)sender {
	[self savePrefs:self];	//save preferences
	[self closePreferencesWindow:self]; 
}


// Action for Cancel button in preferences
- (IBAction) prefsCancel:(id)sender {
	// Don't save changed values
	[self closePreferencesWindow:self];
}


// Action for the Restart button in preferences
- (IBAction) restartSEB:(id)sender {
	// Save preferences and restart XULRunner with the new settings
	[self savePrefs:self];	//save preferences

    // Load start URL from the system's user defaults database
    NSUserDefaults *preferences = [NSUserDefaults standardUserDefaults];
	NSString *urlText = [preferences stringForKey:@"startURL"];
	
    // Load start URL into browser window
	[[webView mainFrame] loadRequest:[NSURLRequest requestWithURL:[NSURL URLWithString:urlText]]];
	
    // Close the preferences window
	[self closePreferencesWindow:self];
}


// Action for the Quit button in preferences
- (IBAction) quitSEB:(id)sender {
	// Save preferences and quit SEB
	[self savePrefs:self];	//save preferences
	[self closePreferencesWindow:self];
	
	[sebController setQuittingMyself:TRUE]; //SEB is quitting itself
	[NSApp terminate: nil];
}


// Action for the About button in preferences
- (IBAction) aboutSEB:(id)sender {
    [aboutWindow setStyleMask:NSBorderlessWindowMask];
	[aboutWindow center];
	[aboutWindow orderFront:self];
    [[NSApplication sharedApplication] runModalForWindow:aboutWindow];
}


// Action for the Help button in preferences
- (IBAction) showHelp:(id)sender {
    //stop the preferences window to be modal, so help page can be viewed properly
    [[NSApplication sharedApplication] stopModal];
    // Load manual page URL into browser window
    NSString *urlText = @"http://www.safeexambrowser.org/macosx";
	[[webView mainFrame] loadRequest:
     [NSURLRequest requestWithURL:[NSURL URLWithString:urlText]]];
    
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

/*// Load preferences when the preferences window is displayed
- (void)windowDidBecomeKey:(NSNotification *)notification {
	[self loadPrefs:self];	//load preferences
}
*/

@end
