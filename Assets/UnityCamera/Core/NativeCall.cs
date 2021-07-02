using System;
using UnityEngine;

//封装IOS与安卓调用原生相机相关功能
public static class NativeCall
{
    #region 打开相册选择照片
    /// <summary>
    /// 打开相册 选择一张照片
    /// </summary>
    public static void OpenPhoto(Action<Texture2D> callBack, bool isShowToast = true)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((string path) =>
        {
            if (!string.IsNullOrEmpty(path) && callBack != null)
            {
                callBack(NativeGallery.LoadImageAtPath(path));
            }
        });
        CheckPermission(permission, isShowToast);
    }
    /// <summary>
    /// 打开相册 选择一张照片
    /// </summary>
    public static void OpenPhoto(Action<string> callBack, bool isShowToast = true)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((string path) =>
        {
            if (!string.IsNullOrEmpty(path) && callBack != null)
            {
                callBack(path);
            }
        });
        CheckPermission(permission, isShowToast);
    }
    /// <summary>
    /// 打开相册 选择多张照片
    /// </summary>
    public static void OpenPhotos(Action<Texture2D[]> callBack, bool isShowToast = true)
    {
        NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((string[] path) =>
        {
            if (path == null || path.Length == 0) return;
            Texture2D[] texs = new Texture2D[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                try
                {
                    if (!string.IsNullOrEmpty(path[i]))
                        texs[i] = NativeGallery.LoadImageAtPath(path[i]);
                }
                catch (Exception ex) { Debug.LogWarning("第" + i + "张图片处理失败 : " + ex.Message + "\n" + path[i]); }
            }
            if (callBack != null) callBack(texs);
        });
        CheckPermission(permission, isShowToast);
    }
    /// <summary>
    /// 打开相册 选择多张照片
    /// </summary>
    public static void OpenPhotos(Action<string[]> callBack)
    {
        NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((string[] path) =>
        {
            if (path == null || path.Length == 0) return;
            if (callBack != null) callBack(path);
        });
        if (permission != NativeGallery.Permission.Granted)
        {
            ShowToast("当前没有相册访问权限，请在设置中打开");
            //打开应用程序设置
            if (NativeGallery.CanOpenSettings()) NativeGallery.OpenSettings();
        }
    }
    #endregion

    #region 打开相册选择视频
    /// <summary>
    /// 打开相册 选择一个视频
    /// </summary>
    public static void OpenVideo(Action<string> callBack, bool isShowToast = true)
    {
        NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((string path) =>
        {
            if (!string.IsNullOrEmpty(path) && callBack != null) callBack(path);
        });
        CheckPermission(permission, isShowToast);
    }
    /// <summary>
    /// 打开相册 选择多个视频
    /// </summary>
    public static void OpenVideo(Action<string[]> callBack, bool isShowToast = true)
    {
        NativeGallery.Permission permission = NativeGallery.GetVideosFromGallery((string[] path) =>
        {
            if (path == null || path.Length == 0) return;
            if (callBack != null) callBack(path);
        });
        CheckPermission(permission, isShowToast);
    }
    #endregion

    #region 打开相机拍照或录制
    /// <summary>
    /// 打开相机拍照
    /// </summary>
    public static void OpenCamera(Action<Texture2D> callBack, bool isShowToast = true)
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture((string path) =>
        {
            if (!string.IsNullOrEmpty(path) && callBack != null)
                callBack(NativeCamera.LoadImageAtPath(path));
        });
        CheckPermission(permission, isShowToast);
    }
    /// <summary>
    /// 打开相机拍照
    /// </summary>
    public static void OpenCamera(Action<string> callBack, bool isShowToast = true)
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture((string path) =>
        {
            if (!string.IsNullOrEmpty(path) && callBack != null)
                callBack(path);
        });
        CheckPermission(permission, isShowToast);
    }
    /// <summary>
    /// 打开相机录像
    /// </summary>
    public static void OpenCameraVideo(Action<string> callBack, bool isShowToast = true)
    {
        NativeCamera.Permission permission = NativeCamera.RecordVideo((string path) =>
        {
            if (!string.IsNullOrEmpty(path) && callBack != null)
                callBack(path);
        });
        CheckPermission(permission, isShowToast);
    }
    #endregion

    #region 保存图片到相册
    /// <summary>
    /// 保存图片到相册 : 从Texture2D
    /// </summary>
    public static void SavePhoto(Texture2D tex, Action<bool> callBack = null, bool isShowToast = true)
    {
        try
        {
            var permission = NativeGallery.SaveImageToGallery(tex, Application.productName, DateTime.Now.ToFileTime() + ".jpg",
                (success, p) => PhotoCallBack(success, p, callBack, isShowToast));
            if (!CheckPermission(permission))
            {
                if (callBack != null) callBack(false);
            }
        }
        catch (Exception ex)
        {
            ShowLog("保存图片失败 : " + ex.Message, isShowToast);
            if (callBack != null) callBack(false);
        }
    }
    /// <summary>
    /// 保存图片到相册 : 从路径
    /// </summary>
    public static void SavePhoto(string path, Action<bool> callBack = null, bool isShowToast = true)
    {
        try
        {
            NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(path, Application.productName, DateTime.Now.ToFileTime() + ".jpg",
                (success, p) => PhotoCallBack(success, p, callBack, isShowToast));
            if (!CheckPermission(permission))
            {
                if (callBack != null) callBack(false);
            }
        }
        catch (Exception ex)
        {
            ShowLog("保存图片失败 : " + ex.Message, isShowToast);
            if (callBack != null) callBack(false);
        }
    }
    /// <summary>
    /// 保存图片到相册 : 从byte[]
    /// </summary>
    public static void SavePhoto(byte[] data, Action<bool> callBack = null, bool isShowToast = true)
    {
        try
        {
            var permission = NativeGallery.SaveImageToGallery(data, Application.productName, DateTime.Now.ToFileTime() + ".jpg",
                (success, p) => PhotoCallBack(success, p, callBack, isShowToast));
            if (!CheckPermission(permission))
            {
                if (callBack != null) callBack(false);
            }
        }
        catch (Exception ex)
        {
            ShowLog("保存图片失败 : " + ex.Message, isShowToast);
            if (callBack != null) callBack(false);
        }
    }
    #endregion

    #region 保存视频到相册
    /// <summary>
    /// 保存视频到相册 : 从路径
    /// </summary>
    public static void SaveVideo(string path, Action<bool> callBack = null, bool isShowToast = true)
    {
        try
        {
            NativeGallery.Permission permission = NativeGallery.SaveVideoToGallery(path, Application.productName, DateTime.Now.ToFileTime() + ".jpg",
                (success, p) => PhotoCallBack(success, p, callBack, isShowToast));

            if (!CheckPermission(permission))
            {
                if (callBack != null) callBack(false);
            }
        }
        catch (Exception ex)
        {
            ShowToast("保存视频失败 : " + ex.Message);
            if (callBack != null) callBack(false);
        }
    }
    /// <summary>
    /// 保存视频到相册 : 从byte[]
    /// </summary>
    public static void SaveVideo(byte[] data, Action<bool> callBack = null, bool isShowToast = true)
    {
        try
        {
            NativeGallery.Permission permission = NativeGallery.SaveVideoToGallery(data, Application.productName, DateTime.Now.ToFileTime() + ".jpg",
                (success, p) => PhotoCallBack(success, p, callBack, isShowToast));
            if (!CheckPermission(permission))
            {
                if (callBack != null) callBack(false);
            }
        }
        catch (Exception ex)
        {
            ShowToast("保存视频失败 : " + ex.Message);
            if (callBack != null) callBack(false);
        }
    }
    #endregion

    #region Tools

#if !UNITY_EDITOR && UNITY_IOS
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _showDialog(string msg);
#endif

    /// <summary>
    /// 显示Toast
    /// </summary>
    /// <param name="msg">显示的信息</param>
    public static void ShowToast(string msg)
    {
        if (msg == "") msg = "empty";
        else if (msg == null) msg = "null";
#if UNITY_EDITOR
        Debug.Log("Toast:" + msg);
#elif UNITY_ANDROID
        //拿到Activity
        AndroidJavaObject currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", msg);
            Toast.CallStatic<AndroidJavaObject>("makeText", currentActivity, javaString, Toast.GetStatic<int>("LENGTH_SHORT")).Call("show");
        }
        ));
#elif UNITY_IOS
        _showDialog(msg);
#endif
    }

    private static void ShowLog(string log, bool isShowToast)
    {
        if (isShowToast)
        {
            ShowToast(log);
#if !UNITY_EDITOR
        Debug.Log(log);
#endif
        }
        else
        {
            Debug.Log(log);
        }
    }
    private static void ShowLogError(string log, bool isShowToast)
    {
        if (isShowToast)
        {
            ShowToast(log);
#if !UNITY_EDITOR
            Debug.LogError(log);
#endif
        }
        else
        {
            Debug.LogError(log);
        }
    }
    #endregion

    #region 检查权限

    public static bool CheckPermission(NativeGallery.Permission permission, bool isShowToast = true)
    {
        if (permission != NativeGallery.Permission.Granted)
        {
            ShowLog("当前没有相册访问权限，请在设置中打开", isShowToast);
            //打开应用程序设置
            if (NativeGallery.CanOpenSettings()) NativeGallery.OpenSettings();
            return false;
        }
        return true;
    }
    public static bool CheckPermission(NativeCamera.Permission permission, bool isShowToast = true)
    {
        if (permission != NativeCamera.Permission.Granted)
        {
            ShowLog("当前没有相机访问权限，请在设置中打开", isShowToast);
            //打开应用程序设置
            if (NativeGallery.CanOpenSettings()) NativeGallery.OpenSettings();
            return false;
        }
        return true;
    }

    #endregion

    #region 保存图片与视频回调
    private static void PhotoCallBack(bool success, string path, Action<bool> callback, bool isShowToast)
    {
        if (success)
        {
            ShowLog("保存图片成功 ! ", isShowToast);
        }
        else
        {
            ShowLog("保存图片失败 : " + path, isShowToast);
        }
        if (callback != null) callback(success);
    }

    private static void VideoCallBack(bool success, string path, Action<bool> callback, bool isShowToast)
    {
        if (success)
        {
            ShowLog("保存视频成功 ! ", isShowToast);
        }
        else
        {
            ShowLog("保存视频失败 : " + path, isShowToast);
        }
        if (callback != null) callback(success);
    }
    #endregion
}
