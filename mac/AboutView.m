//
//  aboutView.m
//  SafeExamBrowser
//
//  Created by Daniel R. Schneider on 30.10.10.
//  Copyright (c) 2010 __MyCompanyName__. All rights reserved.
//

#import "AboutView.h"


@implementation AboutView

- (id)initWithFrame:(NSRect)frame {
    if ((self = [super initWithFrame:frame])) {
        // Initialization code here.
    }
    
    return self;
}

- (void)dealloc {
    // Clean-up code here.
    
    [super dealloc];
}

- (void)drawRect:(NSRect)dirtyRect {
    // Load the image.
    NSImage *anImage = [NSImage imageNamed:@"AboutSEB"];
    
    // Find the point at which to draw it.
    NSPoint backgroundCenter;
    backgroundCenter.x = [self bounds].size.width / 2;
    backgroundCenter.y = [self bounds].size.height / 2;
    
    NSPoint drawPoint = backgroundCenter;
    drawPoint.x -= [anImage size].width / 2;
    drawPoint.y -= [anImage size].height / 2;
    
    // Draw it.
    [anImage drawAtPoint:drawPoint
                fromRect:NSZeroRect
               operation:NSCompositeSourceOver
                fraction:1.0];
}

@end
