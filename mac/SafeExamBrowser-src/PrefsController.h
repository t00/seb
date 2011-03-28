//
//  PrefsController.h
//  SEB
//
//  Created by Daniel R. Schneider on 06.06.10.
//  Copyright 2010 ETH Zurich LET. All rights reserved.
//

// Controller for the preferences window, implementing cocoa bindings

#import <Cocoa/Cocoa.h>
#import <WebKit/WebKit.h>
#import <CommonCrypto/CommonDigest.h>
#import "SEBController.h"


@interface PrefsController : NSObject <NSWindowDelegate> {
	
	IBOutlet NSTextField *startURL;
	NSMutableString *adminPassword;
	NSMutableString *confirmAdminPassword;
	IBOutlet NSTextField *compareAdminPasswords;
    IBOutlet NSButton *allowQuit;
    NSMutableString *quitPassword;
    NSMutableString *confirmQuitPassword;
	IBOutlet NSTextField *compareQuitPasswords;
	IBOutlet NSButton *prefsOk;
	IBOutlet NSButton *prefsRestartSEB;
	IBOutlet NSButton *prefsQuitSEB;

	IBOutlet id sebController;
	IBOutlet WebView *webView;
	IBOutlet NSWindow *prefsWindow;
	IBOutlet NSWindow *aboutWindow;

}

- (NSString*) compareAdminPasswords;

- (void) loadPrefs:(id)sender;
- (void) savePrefs:(id)sender;
- (void) openPreferencesWindow:(id)sender;
- (void) closePreferencesWindow:(id)sender;
- (BOOL) preferencesAreOpen;

- (IBAction) prefsOK:(id)sender;
- (IBAction) prefsCancel:(id)sender;
- (IBAction) restartSEB:(id)sender;
- (IBAction) quitSEB:(id)sender;
- (IBAction) aboutSEB:(id)sender;
- (IBAction) showHelp:(id)sender;

- (NSData*) generateSHAHash:(NSString*)inputString;


@end
