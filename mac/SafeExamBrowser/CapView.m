//
//  CapView.m
//  SEB
//
//  Created by Daniel R. Schneider on 29.07.10.
//  Copyright (c) 2010 ETH Zurich LET. All rights reserved.
//

#import "CapView.h"


@implementation CapView

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
    // Drawing code here.
}


- (BOOL)shouldDelayWindowOrderingForEvent:(NSEvent *)theEvent {
	return YES;
}


- (void)mouseDown:(NSEvent *)theEvent {
    [NSApp preventWindowOrdering];  //prevent that the cap window is ordered front when clicked in
    return;
}


@end
