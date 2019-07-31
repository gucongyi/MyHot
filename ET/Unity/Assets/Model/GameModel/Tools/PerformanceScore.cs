using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PerformanceScore : MonoBehaviour {

    public Mesh mesh;
    public Material mat;

    // Use this for initialization
    
     
    [HideInInspector]
    public float TestScore = 0;// 性能测试评分

    private int _TotalDrawTimes = 0;
    private long _TotalDrawMilliseconds = 0;

    private int _State = 0;
    private int _FrameCount = 0;
    public int delayCount = 60;
    public const string P_SCORE = "P_SCORE";

    static float score = -1;
    public static float Score
    {
        get
        {
            if (score > 0)
                return score;

            if(PlayerPrefs.HasKey(P_SCORE))
            {
                score = PlayerPrefs.GetFloat(P_SCORE);
            }
            return score;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.HasKey(P_SCORE))
        {
            _State = 2;
        }
        switch (_State)
        {
            case 0:
                {
                    if (delayCount-- > 0)
                        return;
                    UnityEngine.Debug.LogFormat("Performance test begin----------------");
                    _State = 1;
                }
                break;
            case 1:
                {
                    // set first shader pass of the material
                    mat.SetPass(0);

                    int draw_times = 0;
                    long milliseconds = 0;

                    Stopwatch sw = new Stopwatch();

                    for (int n = 0; n < 100; ++n)
                    {
                        sw.Start();

                        for (int i = 0; i < 100; ++i)
                        {
                            Graphics.DrawMeshNow(mesh, new Vector3(0, 0, -10000), Quaternion.identity);
                            draw_times++;
                        }

                        sw.Stop();

                        milliseconds += sw.ElapsedMilliseconds;
                        if (milliseconds > 8)
                        {
                            break;
                        }
                    }

                    _TotalDrawTimes += draw_times;
                    _TotalDrawMilliseconds += milliseconds;

                    this.TestScore = 1.0f * _TotalDrawTimes / _TotalDrawMilliseconds;

                    _FrameCount++;
                    if (_FrameCount > 3)// 采样N帧数据
                    {
                        PlayerPrefs.SetFloat(P_SCORE, this.TestScore);
                        UnityEngine.Debug.LogFormat("Performance test draw mesh time(MS): {0}, draw times: {1}, score: {2}", _TotalDrawMilliseconds / 10, _TotalDrawTimes / 10, this.TestScore);
                        _State = 2;
                    }
                }
                break;
            default:
                {
                    UnityEngine.Debug.LogFormat("Performance test   score: {0}",PlayerPrefs.GetFloat(P_SCORE));
                    this.gameObject.SafeDestroy();
                }
                break;
        }


    } 
}
