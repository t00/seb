//
//  MyGlobals.m
//  SafeExamBrowser
//
//  Created by Daniel R. Schneider on 13.10.11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import "MyGlobals.h"
#import "SynthesizeSingleton.h"


@implementation MyGlobals

SYNTHESIZE_SINGLETON_FOR_CLASS(MyGlobals);

@synthesize currentMainHost;    //create getter and setter for current host

@end
