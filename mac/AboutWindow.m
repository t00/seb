//
//  AboutWindow.m
//  SafeExamBrowser
//
//  Created by Daniel R. Schneider on 30.10.10.
//  Copyright (c) 2010 __MyCompanyName__. All rights reserved.
//

#import "AboutWindow.h"


@implementation AboutWindow



- (void) awakeFromNib
{
	// Write application version and localized copyright into text label fields 
	NSString* versionString = [self infoValueForKey:@"CFBundleVersion"];
	versionString = [NSString stringWithFormat:@"%@ %@", NSLocalizedString(@"Version",nil), versionString];
	[version setStringValue: versionString];
	
	NSString* copyrightString = [self infoValueForKey:@"NSHumanReadableCopyright"];
	copyrightString = [NSString stringWithFormat:@"%@\n%@", copyrightString, NSLocalizedString(@"CopyrightPart2",nil)];

	[copyright setStringValue: copyrightString];
}	


// Read Info.plist values from bundle
- (id) infoValueForKey:(NSString*)key
{
    if ([[[NSBundle mainBundle] localizedInfoDictionary] objectForKey:key])
        return [[[NSBundle mainBundle] localizedInfoDictionary] objectForKey:key];
	
    return [[[NSBundle mainBundle] infoDictionary] objectForKey:key];
}


// Overriding this method to return NO prevents that the Prefereces Window 
// looses key state when the About Window is openend
- (BOOL)canBecomeKeyWindow {
    return NO;
}


// When clicked into the window, close it!
- (void)mouseDown:(NSEvent *)theEvent {
	[self orderOut:self];
    [[NSApplication sharedApplication] stopModal];
}


@end
