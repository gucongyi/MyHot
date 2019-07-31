using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramePlayer : MonoBehaviour {
    public int len = 51;
    public int frameRate = 30;
    public RawImage rawImage;
    public string preFix = "t/";
    int index = 0;
    float totalTime;
    float lenTime;
	void Start () {
        lenTime = ((float)len) / frameRate;
        index = 0; 
        rawImage.texture = Resources.Load<Texture>($"{preFix}{index.ToString("d2")}");
    }
	
	// Update is called once per frame
	void Update () { 
        totalTime += Time.deltaTime;
        var c = totalTime % lenTime;
        c = len*( c / lenTime);
        var nextIndex = (int)c;
        if (nextIndex == index)
            return;
        index = nextIndex;
        var next = $"{preFix}{index.ToString("d2")}"; 
        rawImage.texture = Resources.Load<Texture>(next);
    }
}
