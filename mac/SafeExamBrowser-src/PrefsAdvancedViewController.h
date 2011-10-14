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

// Preferences Advanced Pane
// Settings use of third party applications together with SEB

#import <Cocoa/Cocoa.h>
#import "MBPreferencesController.h"

@interface PrefsAdvancedViewController : NSViewController <MBPreferencesModule> {

    //IBOutlet NSButton *allowThirdPartyApps;
    //NSButton *allowDownUploads;
    //IBOutlet NSPopUpButton *downloadPath;
    //IBOutlet NSButton *openDownloads;
    IBOutlet NSPopUpButton *chooseDownloadDirectory;
    IBOutlet NSMenuItem *downloadDirectory;

}

- (NSString *)identifier;
- (NSImage *)image;

- (void) setDownloadDirectory;

- (IBAction) chooseDirectory:(id)sender;


@end
