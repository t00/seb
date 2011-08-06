//
//  MyDocument.h
//  SafeExamBrowser
//
//  Created by Daniel R. Schneider on 27.07.11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import <WebKit/WebView.h>

@interface MyDocument : NSDocument {
//@private
    IBOutlet WebView *webView;

}

- (id)webView;


@end
