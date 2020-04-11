using System;
using UnityEngine;

//封装IOS与安卓调用原生相机相关功能
public static class NativeCall
{
    #region 打开相册选择照片
    /// <summary>
    /// 打开相册 选择一张照片
    /// </summary>
    public static void OpenPhoto(Action<Texture2D> callBack)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((string path) =>
        {
            if (!string.IsNullOrEmpty(path) && callBack != null)
            {
                callBack(NativeGallery.LoadImageAtPath(path));
            }
        });
        if (permission != NativeGallery.Permission.Granted)
        {
            ShowToast("当前没有相册访问权限，请在设置中打开");
            //打开应用程序设置
            if (NativeGallery.CanOpenSettings()) NativeGallery.OpenSettings();
        }
    }
    /// <summary>
    /// 打开相册 选择一张照片
    /// </summary>
    public static void OpenPhoto(Action<string> callBack)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((string path) =>
        {
            if (!string.IsNullOrEmpty(path) && callBack != null)
            {
                callBack(path);
            }
        });
        if (permission != NativeGallery.Permission.Granted)
        {
            ShowToast("当前没有相册访问权限，请在设置中打开");
            //打开应用程序设置
            if (NativeGallery.CanOpenSettings()) NativeGallery.OpenSettings();
        }
    }
    /// <summary>
    /// 打开相册 选择多张照片
    /// </summary>
    public static void OpenPhotos(Action<Texture2D[]> callBack)
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
                catch(Exception ex) { Debug.LogWarning("第" + i + "张图片处理失败 : " + ex.Message + "\n" + path[i]); }
            }
            if (callBack != null) callBack(texs);
        });
        if (permission != NativeGallery.Permission.Granted)
        {
            ShowToast("当前没有相册访问权限，请在设置中打开");
            //打开应用程序设置
            if (NativeGallery.CanOpenSettings()) NativeGallery.OpenSettings();
        }
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
    public static void OpenVideo(Action<string> callBack)
    {
        NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((string path) =>
        {
            if (!string.IsNullOrEmpty(path) && callBack != null) callBack(path);
        });
        if (permission != NativeGallery.Permission.Granted)
        {
            ShowToast("当前没有相册访问权限，请在设置中打开");
            //打开应用程序设置
            if (NativeGallery.CanOpenSettings()) NativeGallery.OpenSettings();
        }
    }
    /// <summary>
    /// 打开相册 选择多个视频
    /// </summary>
    public static void OpenVideo(Action<string[]> callBack)
    {
        NativeGallery.Permission permission = NativeGallery.GetVideosFromGallery((string[] path) =>
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

    #region 打开相机拍照或录制
    /// <summary>
    /// 打开相机拍照
    /// </summary>
    public static void OpenCamera(Action<Texture2D> callBack)
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture((string path) =>
        {
            if (!string.IsNullOrEmpty(path) && callBack != null)
                callBack(NativeCamera.LoadImageAtPath(path));
        });
        if (permission != NativeCamera.Permission.Granted)
        {
            ShowToast("当前没有相机访问权限，请在设置中打开");
            //打开应用程序设置
            if (NativeCamera.CanOpenSettings()) NativeCamera.OpenSettings();
        }
    }
    /// <summary>
    /// 打开相机拍照
    /// </summary>
    public static void OpenCamera(Action<string> callBack)
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture((string path) =>
        {
            if (!string.IsNullOrEmpty(path) && callBack != null)
                callBack(path);
        });
        if (permission != NativeCamera.Permission.Granted)
        {
            ShowToast("当前没有相机访问权限，请在设置中打开");
            //打开应用程序设置
            if (NativeCamera.CanOpenSettings()) NativeCamera.OpenSettings();
        }
    }
    /// <summary>
    /// 打开相机录像
    /// </summary>
    public static void OpenCameraVideo(Action<string> callBack)
    {
        NativeCamera.Permission permission = NativeCamera.RecordVideo((string path) =>
        {
            if (!string.IsNullOrEmpty(path) && callBack != null)
                callBack(path);
        });
        if (permission != NativeCamera.Permission.Granted)
        {
            ShowToast("当前没有相机访问权限，请在设置中打开");
            //打开应用程序设置
            if (NativeCamera.CanOpenSettings()) NativeCamera.OpenSettings();
        }
    }
    #endregion

    #region 保存图片到相册
    /// <summary>
    /// 保存图片到相册 : 从Texture2D
    /// </summary>
    public static void SavePhoto(Texture2D tex, Action<bool> callBack = null)
    {
        try
        {
            NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(tex, Application.productName, DateTime.Now.ToFileTime() + ".jpg", (string info) =>
            {
                if (info == null) ShowToast("保存图片成功 ! ");
                else ShowToast("保存图片失败 : " + info);

                if (callBack != null) callBack(info == null);
            });
            if (permission != NativeGallery.Permission.Granted)
            {
                ShowToast("保存图片失败 : 没有权限");
                //打开应用程序设置
                if (NativeGallery.CanOpenSettings()) NativeGallery.OpenSettings();

                if (callBack != null) callBack(false);
            }
        }
        catch (Exception ex)
        {
            ShowToast("保存图片失败 : " + ex.Message);

            if (callBack != null) callBack(false);
        }
    }
    /// <summary>
    /// 保存图片到相册 : 从路径
    /// </summary>
    public static void SavePhoto(string path, Action<bool> callBack = null)
    {
        try
        {
            NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(path, Application.productName, DateTime.Now.ToFileTime() + ".jpg", (string info) =>
            {
                if (info == null) ShowToast("保存图片成功 ! ");
                else ShowToast("保存图片失败 : " + info);

                if (callBack != null) callBack(info == null);
            });
            if (permission != NativeGallery.Permission.Granted)
            {
                ShowToast("保存图片失败 : 没有权限");
                //打开应用程序设置
                if (NativeGallery.CanOpenSettings()) NativeGallery.OpenSettings();

                if (callBack != null) callBack(false);
            }
        }
        catch (Exception ex)
        {
            ShowToast("保存图片失败 : " + ex.Message);

            if (callBack != null) callBack(false);
        }
    }
    /// <summary>
    /// 保存图片到相册 : 从byte[]
    /// </summary>
    public static void SavePhoto(byte[] data, Action<bool> callBack = null)
    {
        try
        {
            NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(data, Application.productName, DateTime.Now.ToFileTime() + ".jpg", (string info) =>
            {
                if (info == null) ShowToast("保存图片成功 ! ");
                else ShowToast("保存图片失败 : " + info);

                if (callBack != null) callBack(info == null);
            });
            if (permission != NativeGallery.Permission.Granted)
            {
                ShowToast("保存图片失败 : 没有权限");
                //打开应用程序设置
                if (NativeGallery.CanOpenSettings()) NativeGallery.OpenSettings();

                if (callBack != null) callBack(false);
            }
        }
        catch (Exception ex)
        {
            ShowToast("保存图片失败 : " + ex.Message);

            if (callBack != null) callBack(false);
        }
    }
    #endregion

    #region 保存视频到相册
    /// <summary>
    /// 保存视频到相册 : 从路径
    /// </summary>
    public static void SaveVideo(string path, Action<bool> callBack = null)
    {
        try
        {
            NativeGallery.Permission permission = NativeGallery.SaveVideoToGallery(path, Application.productName, DateTime.Now.ToFileTime() + ".jpg", (string info) =>
            {
                if (info == null) ShowToast("保存视频成功 ! ");
                else ShowToast("保存视频失败 : " + info);

                if (callBack != null) callBack(info == null);
            });
            if (permission != NativeGallery.Permission.Granted)
            {
                ShowToast("保存视频失败 : 没有权限");
                //打开应用程序设置
                if (NativeGallery.CanOpenSettings()) NativeGallery.OpenSettings();

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
    public static void SaveVideo(byte[] data, Action<bool> callBack = null)
    {
        try
        {
            NativeGallery.Permission permission = NativeGallery.SaveVideoToGallery(data, Application.productName, DateTime.Now.ToFileTime() + ".jpg", (string info) =>
            {
                if (info == null) ShowToast("保存视频成功 ! ");
                else ShowToast("保存视频失败 : " + info);

                if (callBack != null) callBack(info == null);
            });
            if (permission != NativeGallery.Permission.Granted)
            {
                ShowToast("保存视频失败 : 没有权限");
                //打开应用程序设置
                if (NativeGallery.CanOpenSettings()) NativeGallery.OpenSettings();

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

    #region 显示一个Toast

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
    #endregion

}
