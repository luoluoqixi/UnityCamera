#import <Foundation/Foundation.h>
//#import <MobileCoreServices/UTCoreTypes.h>
//#import <ImageIO/ImageIO.h>
//#import <AVFoundation/AVFoundation.h>
//#import <UIKit/UIKit.h>

//#ifdef UNITY_4_0 || UNITY_5_0
//#import "iPhone_View.h"
//#else
//extern UIViewController* UnityGetGLViewController();
//#endif

@interface UNativeTool:NSObject
//+ (void *)getVideoProperties:(NSString *)errorMsg;
@end

@implementation UNativeTool

// 提示错误信息
+ (void)showError:(NSString *)errorMsg {
    // 1.弹框提醒
    // 初始化对话框
    UIAlertController *alert = [UIAlertController alertControllerWithTitle:@"提示" message:errorMsg preferredStyle:UIAlertControllerStyleAlert];
    [alert addAction:[UIAlertAction actionWithTitle:@"好" style:UIAlertActionStyleDefault handler:nil]];
    UIViewController *vc = UnityGetGLViewController();
    // 弹出对话框
    [vc presentViewController:alert animated:true completion:nil];
}

@end


extern "C" void _showDialog(const char* msg){
    if (msg != nil){
        [UNativeTool showError:[NSString stringWithUTF8String:msg]];
    }else{
        [UNativeTool showError:@"null"];
    }
}
