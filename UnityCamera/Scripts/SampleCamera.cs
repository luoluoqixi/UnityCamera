using UnityEngine;
using UnityEngine.UI;

public class SampleCamera : MonoBehaviour
{
    public Button openCamera, openPhoto;
    public Button savePhoto;
    public RawImage rawImage;
	void Start ()
    {
        openCamera.onClick.AddListener(OpenCamera);
        openPhoto.onClick.AddListener(OpenPhoto);
        savePhoto.onClick.AddListener(SavePhoto);
    }
    //打开相机
    private void OpenCamera()
    {
        NativeCall.OpenCamera((Texture2D tex)=>
        {
            rawImage.texture = tex;
            rawImage.rectTransform.sizeDelta = new Vector2(tex.width, tex.height);
        });
    }
    //打开相册
    private void OpenPhoto()
    {
        NativeCall.OpenPhoto((Texture2D tex) =>
        {
            rawImage.texture = tex;
            rawImage.rectTransform.sizeDelta = new Vector2(tex.width, tex.height);
        });
    }
    //保存照片
    private void SavePhoto()
    {
        NativeCall.SavePhoto(rawImage.texture as Texture2D);
    }
}
