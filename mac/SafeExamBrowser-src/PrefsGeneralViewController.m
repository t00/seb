//
//  SEBController.h
//  Safe Exam Browser
//
//  Created by Daniel R. Schneider on 18.04.11.
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

// Preferences General Pane
// Settings for passwords to enter preferences and quit SEB
// Buttons to quit and restart SEB, show About panel and help

#import "PrefsGeneralViewController.h"


@implementation PrefsGeneralViewController

- (void) awakeFromNib
{
    [[MBPreferencesController sharedController].window setDelegate:self];
#ifdef DEBUG
    NSLog(@"Set PrefsGeneralViewController as delegate for preferences window")
#endif

    // Setup bindings to the preferences window close button
    NSButton *closeButton = [[MBPreferencesController sharedController].window standardWindowButton:NSWindowCloseButton];
    NSDictionary *bindingOptions = [NSDictionary dictionaryWithObjectsAndKeys:
                                    @"NSIsNil",NSValueTransformerNameBindingOption,nil];
    [closeButton bind:@"enabled" 
             toObject:controller 
          withKeyPath:@"selection.compareAdminPasswords" 
              options:bindingOptions];
    [closeButton bind:@"enabled2" 
             toObject:controller 
          withKeyPath:@"selection.compareQuitPasswords" 
              options:bindingOptions];
}


- (NSString *)title
{
	return NSLocalizedString(@"General", @"Title of 'General' preference pane");
}


- (NSString *)identifier
{
	return @"GeneralPane";
}


- (NSImage *)image
{
	return [NSImage imageNamed:@"NSPreferencesGeneral"];
}


- (void)willBeDisplayed
{
    [self loadPrefs:self];
}


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
    NSString *url = [preferences stringForKey:@"startURL"];
    if (url) { //if there is no preferences file yet, startURL can be nil during first execution of this method
       	[startURL setStringValue:url];
        
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
    }
}


- (void) savePrefs:(id)sender {
	// Saves preferences to the system's user defaults database
	NSUserDefaults *preferences = [NSUserDefaults standardUserDefaults];
    // Load start URL from the system's user defaults database
    if (![[preferences stringForKey:@"startURL"] isEqualToString:startURL.stringValue]) {
        [preferences setObject:[startURL stringValue] forKey:@"startURL"];
        // Post a notification that it was requested to reload start URL
        [[NSNotificationCenter defaultCenter]
         postNotificationName:@"requestRestartNotification" object:self];
    }

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
    // Post a notification that preferences were closed
    [[NSNotificationCenter defaultCenter]
     postNotificationName:@"preferencesClosed" object:self];
}


// Action for the Restart button in preferences
// Save preferences and restart SEB with the new settings
- (IBAction) restartSEB:(id)sender {
    [self savePrefs:self];	//save preferences
    // Close preferences window, this also saves preferences
	[self closePreferencesWindow:self];
    
    // Post a notification that it was requested to reload start URL
	[[NSNotificationCenter defaultCenter]
     postNotificationName:@"requestRestartNotification" object:self];
}


// Action for the Quit button in preferences
// Save preferences and quit SEB
- (IBAction) quitSEB:(id)sender {
    [self savePrefs:self];	//save preferences
    // Close preferences window
	[self closePreferencesWindow:self];
	[[NSNotificationCenter defaultCenter]
     postNotificationName:@"requestQuitNotification" object:self];
}


- (void) closePreferencesWindow:(id)sender {
    [[MBPreferencesController sharedController].window orderOut:self];
    [[NSApplication sharedApplication] stopModal];
}


// Action for the About button in preferences
- (IBAction) aboutSEB:(id)sender {
	[[NSNotificationCenter defaultCenter]
     postNotificationName:@"requestShowAboutNotification" object:self];
}


// Action for the Help button in preferences
- (IBAction) showHelp:(id)sender {
    [self savePrefs:self];	//save preferences
    //stop the preferences window to be modal, so help page can be viewed properly
    [[NSApplication sharedApplication] stopModal];
    //but put it again above other windows
    //[[MBPreferencesController sharedController].window setLevel:NSModalPanelWindowLevel];
    // Load manual page URL into browser window
	[[NSNotificationCenter defaultCenter]
     postNotificationName:@"requestShowHelpNotification" object:self];
    
}


- (NSData*) generateSHAHash:(NSString*)inputString {
    unsigned char hashedChars[32];
    CC_SHA256([inputString UTF8String],
              [inputString lengthOfBytesUsingEncoding:NSUTF8StringEncoding], 
              hashedChars);
    NSData *hashedData = [NSData dataWithBytes:hashedChars length:32];
    return hashedData;
}


- (void)windowDidBecomeKey:(NSNotification *)notification
{
    [self loadPrefs:self];
}


- (void)windowWillClose:(NSNotification *)notification
{
    [self savePrefs:self];	//save preferences
    [[NSApplication sharedApplication] stopModal];
#ifdef DEBUG
    NSLog(@"windowWillClose: stopModal")
#endif

}

@end
