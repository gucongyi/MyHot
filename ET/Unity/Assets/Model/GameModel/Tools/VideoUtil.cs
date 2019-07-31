using ETModel;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UniRx.Async;
//需要重构
public class VideoUtil
{
    static VideoPlayer videoPlayer;
    static RenderTexture renderTexture;
    public static bool videoFinished = false;
    public async static void PlayLogoVideo(VideoPlayer v, RawImage rawImage)
    {
        if (videoFinished)
        {
            rawImage.gameObject.SetActive(false);
            return;
        }

        videoPlayer = v;
        string p = Path.Combine(PathHelper.AppHotfixResPath, "logo.unity3d");
        if (!File.Exists(p))
        {
            p = Path.Combine(PathHelper.AppResPath, "logo.unity3d");
        }

        if (!File.Exists(p) && Application.isEditor)
        {
            p = Path.Combine(Application.dataPath, "Videos/StandaloneWindows/logo.unity3d");
        }

        VideoClip vc=null;
        try
        {
            Log.Debug(p);
            foreach (var ab in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (ab.Contains("logo"))
                {
                    vc = ab.LoadAsset<VideoClip>("logo");
                    break;
                }
            }
            if (vc == null)
            {
                vc = AssetBundle.LoadFromFile(p).LoadAsset<VideoClip>("logo");
            }
            
        }
        catch (System.Exception e)
        {
            videoFinished = true;
            rawImage.gameObject.SetActive(false);
            Log.Debug(e.ToString());
            return;
        }

        v.clip = vc;
        //v.url = p;  
        v.Prepare();

        v.playOnAwake = false;
        int count = 0;
        while (v != null && !v.isPrepared)
        {
            count++;
            if (count > 60)
            {
                rawImage.gameObject.SetActive(false);
                videoFinished = true;
                Log.Debug("video Finished 2");
                return;
            }
            await UniTask.DelayFrame(1);
        }
        if (v == null)
            return;

        renderTexture?.DiscardContents();
        renderTexture?.Release();

        renderTexture = new RenderTexture(1280, 720, 0, RenderTextureFormat.ARGB32);
        v.targetTexture = renderTexture;
        var image = v.GetComponent<RawImage>();
        image.color = Color.white;
        image.texture = renderTexture;
        v.Play();
        rawImage.texture = renderTexture;
        rawImage.color = Color.white;
        while (v != null && v.isPlaying)
        {
            await UniTask.DelayFrame(1);
        }

        //rawImage.gameObject.SetActive(false); 

        videoFinished = true;
        Log.Debug("video finished 3");
    }
    public static bool IsPlaying
    {
        get
        {
            if (videoPlayer == null)
                return false;
            return videoPlayer.isPlaying;
        }
    }
    public static void DisposeVideo()
    {
        renderTexture?.DiscardContents();
        renderTexture?.Release();
        renderTexture = null;
    }


}
