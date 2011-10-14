//
//  MainBrowserWindow.m
//  SafeExamBrowser
//
//  Created by Daniel R. Schneider on 03.10.11.
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

#import "MainBrowserWindow.h"
#import "MyGlobals.h"
#import "AllowBlankBrowserWindow.h"


@implementation MainBrowserWindow


// Closing of SEB Browser Window //
- (BOOL)windowShouldClose:(id)sender
{
    // Post a notification that SEB should conditionally quit
    [[NSNotificationCenter defaultCenter]
     postNotificationName:@"requestExitNotification" object:self];
    
    return NO; //but don't close the window (that will happen anyways in case quitting is confirmed)
}


// Get the URL of the page being loaded
- (void)webView:(WebView *)sender didStartProvisionalLoadForFrame:(WebFrame *)frame {
    // Only report feedback for the main frame.
    if (frame == [sender mainFrame]){
        [[MyGlobals sharedMyGlobals] setCurrentMainHost:[[[[frame provisionalDataSource] request] URL] host]];
    }
}


// Update the URL of the current page in case of a server redirect
- (void)webView:(WebView *)sender didReceiveServerRedirectForProvisionalLoadForFrame:(WebFrame *)frame {
    // Only report feedback for the main frame.
    if (frame == [sender mainFrame]){
        [[MyGlobals sharedMyGlobals] setCurrentMainHost:[[[[frame provisionalDataSource] request] URL] host]];
    }
}


// Handling of requests from web plugins to open a link in a new window 
- (void)webView:(WebView *)sender decidePolicyForNavigationAction:(NSDictionary *)actionInformation 
        request:(NSURLRequest *)request 
          frame:(WebFrame *)frame 
decisionListener:(id <WebPolicyDecisionListener>)listener {
    
    // Get navigation type
    int navigationType;
    navigationType = [[actionInformation objectForKey:WebActionNavigationTypeKey] intValue];
    
    // Get action element
    NSDictionary*   element;
    NSURL*          linkURL;
    NSString*       linkTargetFrame;
    element = [actionInformation objectForKey:WebActionElementKey];
    linkURL = [actionInformation objectForKey:WebActionOriginalURLKey];;
    linkTargetFrame = [[element objectForKey:WebElementLinkTargetFrameKey] name];
    NSLog(@"Original URL-Key: %@", linkURL); 
    NSLog(@"Link Target Frame: %@", linkTargetFrame); 
    NSLog(@"Current Host: %@", [[MyGlobals sharedMyGlobals] currentMainHost]); 
    NSLog(@"Requested Host: %@", [[request mainDocumentURL] host]); 
    if (linkURL) {
        if (![[linkURL host] isEqualToString:[[request mainDocumentURL] host]]) {
            //[listener ignore];
        }
    }
    [listener use];
    
}

/*
 if (!currentHost) {
 [listener use];
 } else {
 [listener ignore];
 #ifdef DEBUG
 NSLog(@"Current Host: %@", currentHost); 
 NSLog(@"Requested Host: %@", [[request mainDocumentURL] host]); 
 #endif
 // load link only if it's on the same host like the one of the current page
 if ([currentHost isEqualToString:[[request mainDocumentURL] host]]) {
 // Single browser window: [[webView mainFrame] loadRequest:request];
 // Multiple browser windows
 id myDocument = [[NSDocumentController sharedDocumentController] openUntitledDocumentOfType:@"DocumentType" display:YES];
 [[[myDocument webView] mainFrame] loadRequest:request];
 //return [myDocument webView];
 }
 }
 }
 */


// Open the link requesting to be opened in a new window according to settings
- (void)webView:(WebView *)sender decidePolicyForNewWindowAction:(NSDictionary *)actionInformation 
		request:(NSURLRequest *)request 
   newFrameName:(NSString *)frameName 
decisionListener:(id <WebPolicyDecisionListener>)listener {
    // load link only if it's on the same host like the one of the current page
    //if ([currentHost isEqualToString:[[request mainDocumentURL] host]]) {
    // Single browser window: [[webView mainFrame] loadRequest:request];
    // Multiple browser windows
    id myDocument = [[NSDocumentController sharedDocumentController] openUntitledDocumentOfType:@"DocumentType" display:YES];
    WebView *abWebView = [myDocument webView];
    //NSWindow *browserWindow = [[myDocument webView] window]; 
    // Set window and webView delegates to the AllowBlankBrowserWindow
    AllowBlankBrowserWindow *allowBlankBrowserWindow = [[AllowBlankBrowserWindow alloc]init];
	[abWebView setPolicyDelegate:allowBlankBrowserWindow];

    [[[myDocument webView] mainFrame] loadRequest:request];
    //}
    [listener ignore];
}


@end
