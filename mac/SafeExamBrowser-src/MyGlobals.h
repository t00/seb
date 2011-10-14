//
//  MyGlobals.h
//  SafeExamBrowser
//
//  Created by Daniel R. Schneider on 13.10.11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface MyGlobals : NSObject {
    
    NSString *currentMainHost;
}

+ (MyGlobals*)sharedMyGlobals;

@property(copy, readwrite) NSString *currentMainHost;

@end
