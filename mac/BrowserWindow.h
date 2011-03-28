//
//  BrowserWindow.h
//  SafeExamBrowser
//
//  Created by Daniel R. Schneider on 06.12.10.
//  Copyright 2010 ETH Zurich LET. All rights reserved.
//

// Browser window class, also containing all the web view delegates

#import <Cocoa/Cocoa.h>
#import <WebKit/WebKit.h>


@interface BrowserWindow : NSWindow <NSWindowDelegate> {
    IBOutlet WebView *webView;
    NSString *currentURL;
	NSString *currentHost;

}

@end
