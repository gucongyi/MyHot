using UnityEngine;

namespace Company {
    public class Vector3Helper {
        // gets positive/negative angle inside unit circle
        public static float GetAngleInXYPlane(Vector3 from, Vector3 to) {
            from.z = 0;
            to.z = 0;

            from.Normalize();
            to.Normalize();

            float angle = Vector2.Angle(from, to);

            if(Vector3.Cross(from, to).z > 0) {
                angle = -angle;
            }

            return angle;
        }

        public static float GetAngleInYZPlane(Vector3 from, Vector3 to) {
            from.x = 0;
            to.x = 0;

            from.Normalize();
            to.Normalize();

            float angle = Vector2.Angle(from, to);

            if(Vector3.Cross(from, to).x > 0) {
                angle = -angle;
            }

            return angle;
        }

        public static float GetAngleInXZPlane(Vector3 from, Vector3 to) {
            from.y = 0;
            to.y = 0;

            from.Normalize();
            to.Normalize();

            float angle = Vector2.Angle(from, to);

            if(Vector3.Cross(from, to).y > 0) {
                angle = -angle;
            }

            return angle;
        }

        public static float GetAngleInPlane(Vector3 from, Vector3 to, Vector3 normal) {
            from.Normalize();
            to.Normalize();

            float angle = Mathf.Acos(Vector3.Dot(from.normalized, to.normalized));

            float sign = Mathf.Sign(Vector3.Dot(normal, Vector3.Cross(from, to)));

            if(float.IsNaN(angle)) {
                angle = 0f;
            }

            return sign * angle * Mathf.Rad2Deg;
        }
    }
}