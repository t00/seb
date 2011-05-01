//
//  BrowserWindow.m
//  Safe Exam Browser
//
//  Created by Daniel R. Schneider on 06.12.10.
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

#import "BrowserWindow.h"


@implementation BrowserWindow

// Setup browser window and webView delegates
- (void) awakeFromNib
{
	// Suppress right-click with own delegate method for context menu
	[webView setUIDelegate:self];
	
	// The Policy Delegate is needed to catch opening links in new windows
	[webView setPolicyDelegate:self];
	
	// The Policy Delegate is needed to catch opening links in new windows
	[webView setFrameLoadDelegate:self];
	
}

// Overriding the sendEvent method allows blocking the context menu
// in the whole WebView, even in plugins
- (void)sendEvent:(NSEvent *)theEvent
{
	int controlKeyDown = [theEvent modifierFlags] & NSControlKeyMask;
	
	// filter out the right clicks
	if (!(([theEvent type] == NSLeftMouseDown && controlKeyDown) ||
		[theEvent type] == NSRightMouseDown))
		[super sendEvent:theEvent];
}


#pragma mark Delegates

// Delegate method for disabling right-click context menu
- (NSArray *)webView:(WebView *)sender contextMenuItemsForElement:(NSDictionary *)element 
    defaultMenuItems:(NSArray *)defaultMenuItems {
    // disable right-click context menu
    return NO;
}


// Delegate method for JavaScript alert panel
- (void)webView:(WebView *)sender runJavaScriptAlertPanelWithMessage:(NSString *)message 
initiatedByFrame:(WebFrame *)frame {
	NSString *pageTitle = [sender stringByEvaluatingJavaScriptFromString:@"document.title"];
	NSRunAlertPanel(pageTitle, message, NSLocalizedString(@"OK",nil), nil, nil);
}


// Delegate method for JavaScript confirmation panel
- (BOOL)webView:(WebView *)sender runJavaScriptConfirmPanelWithMessage:(NSString *)message 
initiatedByFrame:(WebFrame *)frame {
	NSString *pageTitle = [sender stringByEvaluatingJavaScriptFromString:@"document.title"];
	return NSRunAlertPanel(pageTitle, message, NSLocalizedString(@"OK",nil), NSLocalizedString(@"Cancel",nil), nil);
}


// Get the URL of the page being loaded
- (void)webView:(WebView *)sender didStartProvisionalLoadForFrame:(WebFrame *)frame {
    // Only report feedback for the main frame.
    if (frame == [sender mainFrame]){
        currentURL = [[[[frame provisionalDataSource] request] URL] absoluteString];
		[currentURL retain];
		currentHost = [[[[frame provisionalDataSource] request] URL] host];
		[currentHost retain];
    }
}


// Update the URL of the current page in case of a server redirect
- (void)webView:(WebView *)sender didReceiveServerRedirectForProvisionalLoadForFrame:(WebFrame *)frame {
    // Only report feedback for the main frame.
    if (frame == [sender mainFrame]){
        currentURL = [[[[frame provisionalDataSource] request] URL] absoluteString];
		[currentURL retain];
		currentHost = [[[[frame provisionalDataSource] request] URL] host];
		[currentHost retain];
    }
}


// Handling of requests to open a link in a new window (including Javascript commands)
- (WebView *)webView:(WebView *)sender createWebViewWithRequest:(NSURLRequest *)request {
    WebView *tempWebView = [[WebView alloc] init];
    //create a new temporary, invisible WebView
    [tempWebView setPolicyDelegate:self];
    [tempWebView setUIDelegate:self];
    return tempWebView;
}


// Handling of requests from web plugins to open a link in a new window 
- (void)webView:(WebView *)sender decidePolicyForNavigationAction:(NSDictionary *)actionInformation 
		request:(NSURLRequest *)request 
          frame:(WebFrame *)frame 
decisionListener:(id <WebPolicyDecisionListener>)listener {
    if ([sender isEqual:webView]) {
        [listener use];
    } else {
        [listener ignore];
#ifdef DEBUG
		NSLog(@"Current Host: %@", currentHost); 
		NSLog(@"Request Host: %@", [[request mainDocumentURL] host]); 
#endif
        // load link only if it's on the same host like the one of the current page
		if ([currentHost isEqualToString:[[request mainDocumentURL] host]]) {
			[[webView mainFrame] loadRequest:request];
		}
    }
}


// Open the link originally requesting to be opened in a new window in the existing WebView
- (void)webView:(WebView *)sender decidePolicyForNewWindowAction:(NSDictionary *)actionInformation 
		request:(NSURLRequest *)request 
   newFrameName:(NSString *)frameName 
decisionListener:(id <WebPolicyDecisionListener>)listener {
    [[webView mainFrame] loadRequest:request];
}


// Handle WebView load errors
- (void)webView:(WebView *)sender didFailProvisionalLoadWithError:(NSError *)error
       forFrame:(WebFrame *)frame {
    
	if ([error code] != -999) {
        
		NSString *titleString = NSLocalizedString(@"Error Loading Page",nil);
		NSString *messageString = [error localizedDescription];
		NSRunAlertPanel(titleString, messageString, NSLocalizedString(@"OK",nil), nil, nil);
		/*
         int answer = NSRunAlertPanel(titleString, messageString,
         @"Retry", @"Cancel", nil);
         switch(answer)
         {
         case NSAlertDefaultReturn:
         [[webView mainFrame] reload]; //Retry: try reloading
         return;
         default:
         return;
         }*/
	}
}

@end
