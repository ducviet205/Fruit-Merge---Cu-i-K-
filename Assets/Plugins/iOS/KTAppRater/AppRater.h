//
//  AppRater.h
//  AppRater
//
//  Created by Kashif Tasneem on 14/10/2013.
//  Copyright (c) 2013 Kashif Tasneem. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface AppRater : NSObject<UIAlertViewDelegate>
{
    
}

@property (nonatomic,retain) NSString *appId;
@property (nonatomic,retain) NSString *reviewTitle;
@property (nonatomic,retain) NSString *reviewMessage;
@property (nonatomic,retain) NSString *reviewNowTitle;
@property (nonatomic,retain) NSString *reviewLaterTitle;
@property (nonatomic,retain) NSString *neverRemindTitle;
@property (nonatomic,retain) NSString *objectName;

@property (nonatomic,assign) int buttonCount;
@property (nonatomic,assign) int numberOfDays;
@property (nonatomic,assign) int numberOfGamePlays;
@property (nonatomic,assign) BOOL shouldShowEachTime;
@property (nonatomic,assign) BOOL shouldAutoShow;

+(AppRater*) sharedController;

-(void) initialize;
-(void) setUp;
-(void) showReviewAlert;
-(void) openURLForced;

@end
