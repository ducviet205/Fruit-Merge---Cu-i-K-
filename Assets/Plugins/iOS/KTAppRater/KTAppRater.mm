#import "AppRater.h"

extern "C" {
    void setupKTAppRater(char *appid,char *title,char *message,char *button1Title,char *button2Title,char *button3Title,bool shouldAlwaysShow,int days,int gamePlays,bool shouldAutoShow,char *objectName) {
        [[AppRater sharedController] initialize];
        [[AppRater sharedController] setObjectName:[NSString stringWithUTF8String:objectName]];
        [[AppRater sharedController] setShouldAutoShow:shouldAutoShow];
        [[AppRater sharedController] setAppId:[NSString stringWithUTF8String:appid]];
        [[AppRater sharedController] setReviewTitle:[NSString stringWithUTF8String:title]];
        [[AppRater sharedController] setReviewMessage:[NSString stringWithUTF8String:message]];
        [[AppRater sharedController] setReviewNowTitle:[NSString stringWithUTF8String:button1Title]];
        [[AppRater sharedController] setReviewLaterTitle:[NSString stringWithUTF8String:button2Title]];
        if (button3Title == nil) {
            [[AppRater sharedController] setButtonCount:2];
        }
        else {
            [[AppRater sharedController] setButtonCount:3];
            [[AppRater sharedController] setNeverRemindTitle:[NSString stringWithUTF8String:button3Title]];
        }
        [[AppRater sharedController] setShouldShowEachTime:shouldAlwaysShow];
        [[AppRater sharedController] setNumberOfDays:days];
        [[AppRater sharedController] setNumberOfGamePlays:gamePlays];
        [[AppRater sharedController] setUp];
    }
    
    void presentRateAlert () {
        [[AppRater sharedController] showReviewAlert];
    }
    
    void openURL () {
        [[AppRater sharedController] openURLForced];
    }
}
