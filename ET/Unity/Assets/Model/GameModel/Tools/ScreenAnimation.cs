using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAnimation : MonoBehaviour
{
    private Material a;
    private bool isMoving;
    private float waitedTime;
    private int index;
    private float[] times;

    private void Awake()
    {
        a = GetComponent<SkinnedMeshRenderer>().material;
    }
    public void SetMove(bool isMoving)
    {
        this.isMoving = isMoving;
    }
    private void Update()
    {
        if (!isMoving) return;
        waitedTime += Time.deltaTime;
        if (waitedTime < 0.1f) return;
        waitedTime -= 0.1f;
        a.SetTextureOffset("_texColor", new Vector2(0, times[index]));
        index++;
        if (index > times.Length - 1)
        {
            index = 0;
        }
    }
    // Use this for initialization
    private void Start()
    {
        index = 0;
        times = new float[] {0, -0.1992188f, -0.3984375f, -0.5976563f, -0.796875f};
    }
}
