//
//  BrowserWindow.m
//  Safe Exam Browser
//
//  Created by Daniel R. Schneider on 06.12.10.
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

#import "BrowserWindow.h"


@implementation BrowserWindow


// Setup browser window and webView delegates
- (void) awakeFromNib
{
	// Suppress right-click with own delegate method for context menu
	[webView setUIDelegate:self];
    
    // Set group name to group related frames (so not to open several new windows)
    [webView setGroupName:@"MyDocument"];
	
	// The Policy Delegate is needed to catch opening links in new windows
	[webView setPolicyDelegate:self];
	
	// The Policy Delegate is needed to catch opening links in new windows
	[webView setFrameLoadDelegate:self];
    
    [webView setShouldCloseWithWindow:YES];
    
    // Setup bindings to the preferences window close button
    NSButton *closeButton = [self standardWindowButton:NSWindowCloseButton];
    //NSDictionary *bindingOptions = [NSDictionary dictionaryWithObjectsAndKeys:nil];
    //NSDictionary *bindingOptions = [NSDictionary dictionaryWithObjectsAndKeys:
                                    //@"NSIsNil",NSValueTransformerNameBindingOption,nil];
    [closeButton bind:@"enabled" 
             toObject:[NSUserDefaultsController sharedUserDefaultsController] 
          withKeyPath:@"values.allowQuit" 
              options:nil];

/*#ifdef DEBUG
    // Display all MIME types the WebView can display as HTML
    NSArray* MIMETypes = [WebView MIMETypesShownAsHTML];
    int i, count = [MIMETypes count];
    for (i=0; i<count; i++) {
        NSLog(@"MIME type shown as HTML: %@", [MIMETypes objectAtIndex:i]);
    }
#endif*/
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


// Overriding this method without calling super in OS X 10.7 Lion
// prevents the windows' position and size to be restored on restarting the app
- (void)restoreStateWithCoder:(NSCoder *)coder
{
    NSLog(@"Prevented windows' position and size to be restored!");
    return;
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


// Opening of Links in New Windows //


// Handling of requests to open a link in a new window (including Javascript commands)
- (WebView *)webView:(WebView *)sender createWebViewWithRequest:(NSURLRequest *)request {

    // Single browser window: [[webView mainFrame] loadRequest:request];
    // Multiple browser windows
    id myDocument = [[NSDocumentController sharedDocumentController] openUntitledDocumentOfType:@"DocumentType" display:YES];
    [[[myDocument webView] mainFrame] loadRequest:request];
    return [myDocument webView];

    /*
    WebView *tempWebView = [[WebView alloc] init];
    //create a new temporary, invisible WebView
    [tempWebView setPolicyDelegate:self];
    [tempWebView setUIDelegate:self];
    [tempWebView setGroupName:@"MyDocument"];
	[tempWebView setFrameLoadDelegate:self];

     return tempWebView;
     */
}

/*
// Handling of requests from web plugins to open a link in a new window 
- (void)webView:(WebView *)sender decidePolicyForNavigationAction:(NSDictionary *)actionInformation 
		request:(NSURLRequest *)request 
          frame:(WebFrame *)frame 
decisionListener:(id <WebPolicyDecisionListener>)listener {
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
        [[[myDocument webView] mainFrame] loadRequest:request];
    //}
    [listener ignore];
}


// Show new window containing webView
- (void)webViewShow:(WebView *)sender
{
    id myDocument = [[NSDocumentController sharedDocumentController] documentForWindow:[sender window]];
    [myDocument showWindows];
    // Order new browser window to the front
    [[sender window] makeKeyAndOrderFront:self];
}


- (void)webViewClose:(WebView*)sender
{
    // Get document for web view
    id myDocument = [[NSDocumentController sharedDocumentController] documentForWindow:[sender window]];
    // Close window
    [myDocument close];
}


// Downloading and Uploading of Files //


- (void)webView:(WebView *)sender runOpenPanelForFileButtonWithResultListener:(id < WebOpenPanelResultListener >)resultListener
// Upload chosen file
{
    NSUserDefaults *preferences = [NSUserDefaults standardUserDefaults];
    if ([preferences boolForKey:@"allowDownUploads"] == YES) {
        // Create the File Open Dialog class.
        NSOpenPanel* openFilePanel = [NSOpenPanel openPanel];
        
        // Disable the selection of files in the dialog.
        [openFilePanel setCanChooseFiles:YES];
        
        // Enable the selection of directories in the dialog.
        [openFilePanel setCanChooseDirectories:NO];
        
        // Change text of the open button in file dialog
        [openFilePanel setPrompt:NSLocalizedString(@"Choose",nil)];
        
        // Change text of the open button in file dialog
        [openFilePanel setDirectoryURL:[NSURL URLWithString:[preferences stringForKey:@"downloadDirectory"]]];
        
        // Display the dialog.  If the OK button was pressed,
        // process the files.
        [openFilePanel beginSheetModalForWindow:self
                              completionHandler:^(NSInteger result) {
                                  if (result == NSFileHandlingPanelOKButton) {
                                      // Get an array containing the full filenames of all
                                      // files and directories selected.
                                      NSArray* files = [openFilePanel URLs];
                                      NSString* fileName = [[files objectAtIndex:0] path];
                                      [resultListener chooseFilename:fileName];
                                  }
                              }];
    }
}


- (void)webView:(WebView *)sender decidePolicyForMIMEType:(NSString*)type 
        request:(NSURLRequest *)request 
          frame:(WebFrame *)frame
decisionListener:(id < WebPolicyDecisionListener >)listener
{
    //if ([type isEqualToString:@"application/vnd.ms-excel"]) {
    if (![WebView canShowMIMEType:type]) {
        // If MIME type cannot be displayed by the WebView, then we download it
        NSLog(@"MIME type to download is %@", type);
        [listener download];
        [self startDownloadingURL:request.URL];
        return;
    }
    [listener use];
}


// Handle WebView load errors
- (void)webView:(WebView *)sender didFailProvisionalLoadWithError:(NSError *)error
       forFrame:(WebFrame *)frame {
    
	if ([error code] != -999) {
        
        if ([error code] !=  WebKitErrorFrameLoadInterruptedByPolicyChange) //this error can be ignored
        {
            NSString *titleString = NSLocalizedString(@"Error Loading Page",nil);
            NSString *messageString = [error localizedDescription];
            int answer = NSRunAlertPanel(titleString, messageString, NSLocalizedString(@"Retry",nil), NSLocalizedString(@"Cancel",nil), nil, nil);
            switch(answer) {
                case NSAlertDefaultReturn:
                    //Retry: try reloading
                    [[webView mainFrame] loadRequest:
                     [NSURLRequest requestWithURL:[NSURL URLWithString:currentURL]]];
                    return;
                default:
                    return;
            }
        }
	}
}


- (void)startDownloadingURL:(NSURL *)url
{
    NSUserDefaults *preferences = [NSUserDefaults standardUserDefaults];
    if ([preferences boolForKey:@"allowDownUploads"] == YES) {
        // If downloading is allowed
        // Create the request.
        NSURLRequest *theRequest = [NSURLRequest requestWithURL:url
                                                cachePolicy:NSURLRequestUseProtocolCachePolicy
                                            timeoutInterval:60.0];
        // Create the download with the request and start loading the data.
        NSURLDownload  *theDownload = [[NSURLDownload alloc] initWithRequest:theRequest delegate:self];
        if (!theDownload) {
            NSLog(@"Starting the download failed!"); //Inform the user that the download failed.
        }
    }
}


- (void)download:(NSURLDownload *)download decideDestinationWithSuggestedFilename:(NSString *)filename
{
    NSUserDefaults *preferences = [NSUserDefaults standardUserDefaults];
    downloadPath = [preferences stringForKey:@"downloadDirectory"];
    if (!downloadPath) {
        //if there's no path saved in preferences, set standard path
        downloadPath = [NSHomeDirectory() stringByAppendingPathComponent:@"Downloads"];
        [preferences setObject:downloadPath forKey:@"downloadDirectory"];
    }
    NSString *destinationFilename = [downloadPath stringByAppendingPathComponent:filename];
    [download setDestination:destinationFilename allowOverwrite:NO];
}


- (void)download:(NSURLDownload *)download didFailWithError:(NSError *)error
{
    // Release the download.
    [download release];
    
    // Inform the user
    //[self presentError:error modalForWindow:[self windowForSheet] delegate:nil didPresentSelector:NULL contextInfo:NULL];

    NSLog(@"Download failed! Error - %@ %@",
          [error localizedDescription],
          [[error userInfo] objectForKey:NSURLErrorFailingURLStringErrorKey]);
}


- (void)downloadDidFinish:(NSURLDownload *)download
{
    // Release the download.
    [download release];
    
    NSLog(@"Download of File %@ did finish.",downloadPath);
    NSUserDefaults *preferences = [NSUserDefaults standardUserDefaults];
    if ([preferences boolForKey:@"openDownloads"] == YES) {
    // Open downloaded file
    [[NSWorkspace sharedWorkspace] openFile:downloadPath];
    }
}


-(void)download:(NSURLDownload *)download didCreateDestination:(NSString *)path
{
    // path now contains the destination path
    // of the download, taking into account any
    // unique naming caused by -setDestination:allowOverwrite:
    NSLog(@"Final file destination: %@",path);
    downloadPath = path;
}


// Closing of Browser Window //

- (BOOL)windowShouldClose:(id)sender
{
    return YES;
    // Post a notification that SEB should conditionally quit
    [[NSNotificationCenter defaultCenter]
     postNotificationName:@"requestExitNotification" object:self];
    
    return NO; //but don't close the window (that will happen anyways in case quitting is confirmed)
}


@end
