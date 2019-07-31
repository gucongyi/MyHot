using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpConfig : MonoBehaviour {

    /// <summary>
    /// 马匹速度
    /// </summary>
    public float speed=9f;
    /// <summary>
    /// 马匹跳跃上升时间
    /// </summary>
    public float jumpTime=0.3f;//half
    /// <summary>
    /// 马匹跳跃高度
    /// </summary>
    public float jumpHigh = 1;
    /// <summary>
    /// 马匹跳跃旋转角度，没有动作的临时替代方式
    /// </summary>
    public float jumpRotation = 10;
    /// <summary>
    /// 离最后一个跨栏的距离
    /// </summary>
    public float endPosition = 5;
    /// <summary>
    /// 起跑加速时间
    /// </summary>
    public float startTime = 0.5f;
    /// <summary>
    /// 起跳有效距离-最远
    /// </summary>
    public float jumpDistanceMax = 1.5f;
    /// <summary>
    /// 起跳有效距离-最近
    /// </summary>
    public float jumpDistanceMin = 1f;
    /// <summary>
    /// 碰撞发生距离
    /// </summary>
    public float hitDistance = 0.8f;
    /// <summary>
    /// 碰撞结束时间
    /// </summary>
    public float hitEndTime = 1f;

    /// <summary>
    /// 碰撞栏杆动作阀值
    /// </summary>
    public float fenceHitHigh = 0.15f;
}
