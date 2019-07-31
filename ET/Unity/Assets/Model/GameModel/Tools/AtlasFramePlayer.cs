using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class AtlasFramePlayer : MonoBehaviour
{
    public int len = 51;
    public int frameRate = 30;
    public Image image;
    public string preFix = "";
    public SpriteAtlas spriteAtlas;
    int index = 0;
    float totalTime;
    float lenTime;
    string[] spriteNames;
    void Start()
    {
        spriteNames = new string[len];
        for (int i = 0; i < len; i++)
        {
            spriteNames[i] = $"{preFix}{i.ToString("d2")}";
        }
        lenTime = ((float)len) / frameRate;
        index = 0;
        image.sprite = spriteAtlas.GetSprite(spriteNames[index]);
    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;
        var c = totalTime % lenTime;
        c = len * (c / lenTime);
        var nextIndex = (int)c;
        if (nextIndex == index)
            return;
        index = nextIndex;
        image.sprite = spriteAtlas.GetSprite(spriteNames[index]);
    }
}
