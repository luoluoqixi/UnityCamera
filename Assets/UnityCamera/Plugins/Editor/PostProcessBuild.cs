//using UnityEngine;
//using UnityEditor;
//using System.IO;

#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
#endif


public class PostProcessBuild
{
    //是否开启IOS端打包处理
    private const bool ENABLED = true;
    //构建版本是否最低为IOS8
    private const bool MINIMUM_TARGET_8_OR_ABOVE = false;
    //申请相机权限提示文字
	private const string CAMERA_USAGE_DESCRIPTION = "Capture media with camera";
    //申请相册权限提示文字
    private const string Photo_USAGE_DESCRIPTION = "Access to photos";
    //申请视频权限提示文字
    private const string MICROPHONE_USAGE_DESCRIPTION = "Capture microphone input in videos";
    //添加到相册权限
    private const string PhotoAdd = "Add To Photo";
    //语言类型 默认为en  中文为china
    private const string language = "china";


    ////删除过时插件
    //[InitializeOnLoadMethod]
    //public static void ValidatePlugin()
    //{
    //    string jarPath = "Assets/Plugins/NativeCamera/Android/NativeCamera.jar";
    //    if (File.Exists(jarPath))
    //    {
    //        Debug.Log("删除过时的 " + jarPath);
    //        AssetDatabase.DeleteAsset(jarPath);
    //    }
    //}

#if UNITY_IOS
#pragma warning disable 0162
    [PostProcessBuild]
	public static void OnPostprocessBuild( BuildTarget target, string buildPath )
	{
		if( !ENABLED)
			return;

		if( target == BuildTarget.iOS )
		{
			string pbxProjectPath = PBXProject.GetPBXProjectPath( buildPath );
			string plistPath = Path.Combine( buildPath, "Info.plist" );

			PBXProject pbxProject = new PBXProject();
			pbxProject.ReadFromFile( pbxProjectPath );
#if UNITY_2019_3_OR_NEWER
			string targetGUID = pbxProject.GetUnityFrameworkTargetGuid();
#else
			string targetGUID = pbxProject.TargetGuidByName(PBXProject.GetUnityTargetName() );
#endif
    		if( MINIMUM_TARGET_8_OR_ABOVE )
			{
				pbxProject.AddBuildProperty( targetGUID, "OTHER_LDFLAGS", "-framework Photos" );
				pbxProject.AddBuildProperty( targetGUID, "OTHER_LDFLAGS", "-framework MobileCoreServices" );
				pbxProject.AddBuildProperty( targetGUID, "OTHER_LDFLAGS", "-framework ImageIO" );
			}
			else
			{
				pbxProject.AddBuildProperty( targetGUID, "OTHER_LDFLAGS", "-weak_framework Photos" );
				pbxProject.AddBuildProperty( targetGUID, "OTHER_LDFLAGS", "-framework AssetsLibrary" );
				pbxProject.AddBuildProperty( targetGUID, "OTHER_LDFLAGS", "-framework MobileCoreServices" );
				pbxProject.AddBuildProperty( targetGUID, "OTHER_LDFLAGS", "-framework ImageIO" );
			}
			//pbxProject.AddBuildProperty(targetGUID, "OTHER_LDFLAGS", "-framework MobileCoreServices");
			//pbxProject.AddBuildProperty(targetGUID, "OTHER_LDFLAGS", "-framework ImageIO");

            pbxProject.RemoveFrameworkFromProject( targetGUID, "Photos.framework" );

			File.WriteAllText( pbxProjectPath, pbxProject.WriteToString() );

			PlistDocument plist = new PlistDocument();
			plist.ReadFromString( File.ReadAllText( plistPath ) );

			PlistElementDict rootDict = plist.root;
            //相机权限
			rootDict.SetString( "NSCameraUsageDescription", CAMERA_USAGE_DESCRIPTION );
            //调用相册权限
            rootDict.SetString("NSPhotoLibraryUsageDescription", Photo_USAGE_DESCRIPTION);
            //调用录制视频权限
            rootDict.SetString( "NSMicrophoneUsageDescription", MICROPHONE_USAGE_DESCRIPTION );
            //保存到相册权限
            rootDict.SetString("NSPhotoLibraryAddUsageDescription", PhotoAdd);
            //语言  用来处理打开相机后的文字显示为中文
            rootDict.SetString("CFBundleDevelopmentRegion", language);
            //本地化资源语言
            rootDict.SetBoolean("CFBundleAllowMixedLocalizations", true);
            //删除UIApplicationExitsOnSuspend
            if (rootDict.values.ContainsKey("UIApplicationExitsOnSuspend"))
                rootDict.values.Remove("UIApplicationExitsOnSuspend");

            File.WriteAllText( plistPath, plist.WriteToString() );
		}
	}
#pragma warning restore 0162
#endif
}