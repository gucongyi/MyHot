using UnityEngine;
using System.Collections;

namespace Company {
    /// <summary>
    /// Extensions for Vector3, adds extra functionality to the Vector3 class such as Vector3.CopyClampMagnitude
    /// </summary>
    public static class Vector3Extensions {
        /// <summary>
        /// Get positive/negative clockwise angle between 2 vectors
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float GetCWAngleInPlane(this Vector2 from, Vector2 to) {
            Vector3 from3 = from;
            Vector3 to3 = to;

            from3.Normalize();
            to3.Normalize();

            float angle = Vector2.Angle(from3, to3);

            if(Vector3.Cross(from3, to3).z > 0) {
                angle = -angle;
            }

            return angle;
        }

        /// <summary>
        /// Create a copy with a magnitude clamped inside a rnage
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static Vector3 CopyClampMagnitude(this Vector3 vector, float min, float max) {
            float newMagnitude = Mathf.Clamp(vector.magnitude, min, max);

            return vector.CopyChangeMagnitude(newMagnitude);
        }

        /// <summary>
        /// Create a copy with a modified magnitude
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="magnitude"></param>
        public static Vector3 CopyChangeMagnitude(this Vector3 vector, float magnitude) {
            Vector3 ret = vector;

            ret.Normalize();
            ret *= magnitude;

            return ret;
        }

        /// <summary>
        /// Copy a vector and set .x of the copy
        /// </summary>
        /// <param name="v"></param>
        /// <param name="x"></param>
        /// <returns>The copy</returns>
        public static Vector3 CopyChangeX(this Vector3 v, float x) {
            v.x = x;

            return v;
        }

        /// <summary>
        /// Copy a vector and set .y of the copy
        /// </summary>
        /// <param name="v"></param>
        /// <param name="y"></param>
        /// <returns>The copy</returns>
        public static Vector3 CopyChangeY(this Vector3 v, float y) {
            v.y = y;

            return v;
        }

        /// <summary>
        /// Copy a vector and set .z of the copy
        /// </summary>
        /// <param name="v"></param>
        /// <param name="z"></param>
        /// <returns>The copy</returns>
        public static Vector3 CopyChangeZ(this Vector3 v, float z) {
            v.z = z;

            return v;
        }
        /// <summary>
        /// Copy a vector and set .x and .y of the copy
        /// </summary>
        /// <param name="v"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>The copy</returns>
        public static Vector3 CopyChangeXY(this Vector3 v, float x, float y) {
            v.x = x;
            v.y = y;

            return v;
        }

        /// <summary>
        /// Copy a vector and set .y and .z of the copy
        /// </summary>
        /// <param name="v"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns>The copy</returns>
        public static Vector3 CopyChangeYZ(this Vector3 v, float y, float z) {
            v.y = y;
            v.z = z;

            return v;
        }

        /// <summary>
        /// Copy a vector and set .x and .z of the copy
        /// </summary>
        /// <param name="v"></param>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns>The copy</returns>
        public static Vector3 CopyChangeXZ(this Vector3 v, float x, float z) {
            v.x = x;
            v.z = z;

            return v;
        }
        /// <summary>
        /// Get distance between 2 points in the XY plane (z = 0)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float DistanceInXYPlane(this Vector3 a, Vector3 b) {
            a.z = 0;
            b.z = 0;

            return Vector3.Distance(a, b);
        }

        /// <summary>
        /// Get distance between 2 points in the XZ plane (y = 0)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float DistanceInXZPlane(this Vector3 a, Vector3 b) {
            a.y = 0;
            b.y = 0;

            return Vector3.Distance(a, b);
        }

        /// <summary>
        /// Get distance between 2 points in the YZ plane (x = 0)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float DistanceInYZPlane(this Vector3 a, Vector3 b) {
            a.x = 0;
            b.x = 0;

            return Vector3.Distance(a, b);
        } 
         
    }
}