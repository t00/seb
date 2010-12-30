//
//  AboutWindow.h
//  SafeExamBrowser
//
//  Created by Daniel R. Schneider on 30.10.10.
//  Copyright (c) 2010 __MyCompanyName__. All rights reserved.
//

#import <Cocoa/Cocoa.h>


@interface AboutWindow : NSWindow {

  	IBOutlet NSTextField *version;
	IBOutlet NSTextField *copyright;
  
}

- (id) infoValueForKey:(NSString*)key;

@end
