using UnityEngine;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

public enum UILocalizeType
{
    Text,

    /// <summary>
    /// dynamic load image
    /// </summary>
    ImageLoad,
    
    Font,

    Audio,
}


public class UILocalize : MonoBehaviour
{
    public delegate void OnLocalizeEvent();
    public OnLocalizeEvent onLocalizeEvent;

    public UILocalizeType localizeType;

    public string languageIdBase;

    /// <summary>
    /// format param :string.format("xxxxxx{0}",xxxx);
    /// </summary>
    public int[] languageIdParams;
    
    private bool mStarted = false;

    /// <summary>
    /// Localize the widget on enable, but only if it has been started already.
    /// </summary>

    private void OnEnable()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif
        if (mStarted) OnLocalize();
    }

    /// <summary>
    /// Localize the widget on start.
    /// </summary>

    private void Start()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif
        mStarted = true;
        OnLocalize();
    }

    /// <summary>
    /// This function is called by the Localization manager via a broadcast SendMessage.
    /// </summary>

    private void OnLocalize()
    {
        onLocalizeEvent?.Invoke();
    }

    public void SetText(string text)
    {
        Text lbl = GetComponent<Text>();
        if (lbl != null)
        {
            // If this is a label used by input, we should localize its default value instead
            InputField input = lbl.gameObject.GetComponentInParent<InputField>();
            if (input != null && input.textComponent == lbl) input.text = text;
            else lbl.text = text;
#if UNITY_EDITOR
            if (!Application.isPlaying) EditorUtility.SetDirty(lbl);
#endif
        }
    }

    public void SetImage(Sprite sprite)
    {
        Image sp = GetComponent<Image>();
        if (sp != null)
        {
            Button btn = sp.gameObject.GetComponentInParent<Button>();
            if (btn != null && btn.targetGraphic == sp.gameObject)
                btn.image.sprite = sprite;

            sp.sprite = sprite;
            sp.SetNativeSize();
#if UNITY_EDITOR
            if (!Application.isPlaying) EditorUtility.SetDirty(sp);
#endif
        }
    }

    public void SetFont(Font font)
    {
        Text lbl = GetComponent<Text>();
        if (lbl != null)
        {
            lbl.font = font;
#if UNITY_EDITOR
            if (!Application.isPlaying) EditorUtility.SetDirty(lbl);
#endif
        }
    }

    public void SetAudio(AudioClip audioClip)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.clip = audioClip;
#if UNITY_EDITOR
            if (!Application.isPlaying) EditorUtility.SetDirty(audioSource);
#endif
        }
    }
}