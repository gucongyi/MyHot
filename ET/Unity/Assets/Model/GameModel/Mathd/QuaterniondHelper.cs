using System;
using UnityEngine;
public class QuaterniondHelper
{
    public static Quaternion GetQuaternion(Quaterniond quaterniond)
    {
        return new Quaternion((float)quaterniond.x, (float)quaterniond.y, (float)quaterniond.z, (float)quaterniond.w);
    }
}
