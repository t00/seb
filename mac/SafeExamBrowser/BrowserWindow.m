//
//  BrowserWindow.m
//  SafeExamBrowser
//
//  Created by Daniel R. Schneider on 06.12.10.
//  Copyright 2010 ETH Zurich LET. All rights reserved.
//

#import "BrowserWindow.h"


@implementation BrowserWindow


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

@end
