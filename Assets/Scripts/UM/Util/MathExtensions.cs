#nullable enable

using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UM.Util
{
    public static class MathExtensions
    {
        public static Vector2 ProjectedXY(this Vector3 v) => new Vector2(v.x, v.y);
        public static Vector2 ProjectedXZ(this Vector3 v) => new Vector2(v.x, v.z);
        public static Vector2 ProjectedYX(this Vector3 v) => new Vector2(v.y, v.x);
        public static Vector2 ProjectedYZ(this Vector3 v) => new Vector2(v.y, v.z);
        public static Vector2 ProjectedZX(this Vector3 v) => new Vector2(v.z, v.x);
        public static Vector2 ProjectedZY(this Vector3 v) => new Vector2(v.z, v.y);
        
        public static Vector3 ReProjectedXY(this Vector2 v, float newZ = 0) => new Vector3(v.x, v.y, newZ);
        public static Vector3 ReProjectedXZ(this Vector2 v, float newY = 0) => new Vector3(v.x, newY, v.y);
        public static Vector3 ReProjectedYX(this Vector2 v, float newZ = 0) => new Vector3(v.y, v.x, newZ);
        public static Vector3 ReProjectedYZ(this Vector2 v, float newX = 0) => new Vector3(newX, v.x, v.y);
        public static Vector3 ReProjectedZX(this Vector2 v, float newY = 0) => new Vector3(v.y, newY, v.x);
        public static Vector3 ReProjectedZY(this Vector2 v, float newX = 0) => new Vector3(newX, v.y, v.x);

        public static Vector3 DroppedY(this Vector3 v, float newY = 0) => new Vector3(v.x, newY, v.z);
        
        public static Vector2 NormalizedDiamond(this Vector2 v) => v / (Mathf.Abs(v.x) + Mathf.Abs(v.y));

        public static Vector2 ClampedDiamond(this Vector2 v, float maxClamp)
        {
            if (maxClamp <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            Vector2 normalized = v.NormalizedDiamond() * maxClamp;

            return v.magnitude > normalized.magnitude ? normalized : v;
        }

        public static void Deconstruct(this Vector2 v, out float x, out float y)
        {
            x = v.x;
            y = v.y;
        }
        
        public static void Deconstruct(this Vector2Int v, out int x, out int y)
        {
            x = v.x;
            y = v.y;
        }

        public static void Deconstruct(this Vector3 v, out float x, out float y, out float z)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public static void Deconstruct(this Vector3Int v, out int x, out int y, out int z)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public static Quaternion SmoothDamp(
            this Quaternion current, Quaternion target, ref Quaternion velocity, float smoothTime
        )
        {
            float x = Mathf.SmoothDamp(current.x, target.x, ref velocity.x, smoothTime);
            float y = Mathf.SmoothDamp(current.y, target.y, ref velocity.y, smoothTime);
            float z = Mathf.SmoothDamp(current.z, target.z, ref velocity.z, smoothTime);
            float w = Mathf.SmoothDamp(current.w, target.w, ref velocity.w, smoothTime);

            return new Quaternion(x, y, z, w);
        }

        public static Quaternion To(this Quaternion origin, Quaternion destination)
            => destination * Quaternion.Inverse(origin);

        public static Vector3 DivideBy(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        public static int GetOdd(this int n) => n | 1;
        public static void MakeOdd(this ref int n) => n |= 1;

        public static int GetEven(this int n) => n & ~1;
        public static void MakeEven(this ref int n) => n &= ~1;

        public static void MakeCycleDegrees360(this ref float angle) => angle = GetCycleDegrees360(angle);
        public static float GetCycleDegrees360(this float angle) => GetCycleDegrees(angle, 360f);
        public static void MakeCycleDegrees180(this ref float angle) => angle = GetCycleDegrees180(angle);
        public static float GetCycleDegrees180(this float angle) => GetCycleDegrees(angle, 180f);

        public static void MakeCycleDegrees(this ref float angle, float cycleMaxAngle)
            => angle = GetCycleDegrees(angle, cycleMaxAngle);

        public static float GetCycleDegrees(this float angle, float cycleMaxAngle)
        {
            float fullCyclesAngle = 360f * Mathf.Floor((angle + 360f - cycleMaxAngle) * (1f / 360f));

            return angle - fullCyclesAngle;
        }

        public static float Average(params float[] nums) => nums.Sum() / nums.Length;

        public static bool EvaluateProbability(float randomValue, float probability) => randomValue < probability;
        public static bool EvaluateProbability(float probability) => EvaluateProbability(Random.value, probability);
    }
}
