using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailClearOnEnable : MonoBehaviour {

    TrailRenderer trailRenderer; 
    bool isFirst = true;
    float trailTime;
    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        //pos = this.transform.position;
        trailTime = trailRenderer.time;
    } 
     
    private void OnEnable()
    {
        if (isFirst)
            isFirst = false;
        else
        {
            trailRenderer.Reset(this,trailTime);
        }
        
    } 
}


