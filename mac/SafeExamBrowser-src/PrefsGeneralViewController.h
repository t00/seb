//
//  SEBController.h
//  Safe Exam Browser
//
//  Created by Daniel R. Schneider on 29.04.10.
//  Copyright (c) 2010-2011 Daniel R. Schneider, ETH Zurich, 
//  Educational Development and Technology (LET), 
//  based on the original idea of Safe Exam Browser 
//  by Stefan Schneider, University of Giessen
//  Project concept: Dr. Thomas Piendl, Daniel R. Schneider, 
//  Dr. Dirk Bauer, Karsten Burger, Marco Lehre, 
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

#import <Cocoa/Cocoa.h>
#import "MBPreferencesController.h"
#import <CommonCrypto/CommonDigest.h>


@interface PrefsGeneralViewController : NSViewController <MBPreferencesModule, NSWindowDelegate> {

	IBOutlet NSTextField *startURL;
	NSMutableString *adminPassword;
	NSMutableString *confirmAdminPassword;
    IBOutlet NSButton *allowQuit;
    NSMutableString *quitPassword;
    NSMutableString *confirmQuitPassword;
	IBOutlet NSButton *prefsQuitSEB;
    
	IBOutlet NSObjectController *controller;
        
}

- (NSString *)identifier;
- (NSImage *)image;

- (NSString*) compareAdminPasswords;

- (void) loadPrefs:(id)sender;
- (void) savePrefs:(id)sender;

- (IBAction) restartSEB:(id)sender;
- (IBAction) quitSEB:(id)sender;
- (void) closePreferencesWindow:(id)sender;
- (IBAction) aboutSEB:(id)sender;
- (IBAction) showHelp:(id)sender;

- (NSData*) generateSHAHash:(NSString*)inputString;

@end
