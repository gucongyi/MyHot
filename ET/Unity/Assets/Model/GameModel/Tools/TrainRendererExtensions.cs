using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TrainRendererExtensions
{
    /// <summary>
    /// Reset the trail so it can be moved without streaking
    /// </summary>
    public static void Reset(this TrailRenderer trail, MonoBehaviour instance, float trailTime)
    {
        instance.StartCoroutine(ResetTrail(trail, trailTime));
    }

    /// <summary>
    /// Coroutine to reset a trail renderer trail
    /// </summary>
    /// <param name="trail"></param>
    /// <returns></returns>
    static IEnumerator ResetTrail(TrailRenderer trail, float trailTime)
    {
        trail.Clear();
        trail.time = -1f;
        yield return new WaitForEndOfFrame();
        trail.time = trailTime;
    }
}
