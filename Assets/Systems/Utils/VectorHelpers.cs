using System.Runtime.CompilerServices;
using UnityEngine;

namespace Utils
{
    public static class VectorHelpers
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DetectCollision(float minA, float maxA, float minB, float maxB)
        {
            return (minA > minB && minA < maxB)
                || (minA < minB && maxA > minB);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 MergeVectors(Vector2 a, Vector2 b)
            => new(a.x, a.y, b.x, b.y);

        public static Vector3 NumericsToUnity(System.Numerics.Vector3 nv)
        {
            Vector3 uv;
            uv.x = nv.X;
            uv.y = nv.Y;
            uv.z = nv.Z;
            return uv;
        }

        public static Vector2 NumericsToUnity(System.Numerics.Vector2 nv)
        {
            Vector2 uv;
            uv.x = nv.X;
            uv.y = nv.Y;
            return uv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 FlipVector(Vector2 v2)
        {
            return new Vector3(v2.x, 0, v2.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 FlipVector(Vector3 v3)
        {
            return new Vector3(v3.x, v3.z, v3.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Vector3ToFlippedVector2(Vector3 v3)
        {
            return new Vector2(v3.x, v3.z);
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MinMaxCorners(Vector3 a, Vector3 b, out Vector3 min, out Vector3 max)
        {
            UnityHelpers.MinMax(a.x, b.x, out var mi, out var ma);
            min.x = mi;
            max.x = ma;
            UnityHelpers.MinMax(a.y, b.y, out mi, out ma);
            min.y = mi;
            max.y = ma;
            UnityHelpers.MinMax(a.z, b.z, out mi, out ma);
            min.z = mi;
            max.z = ma;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MinMaxCornersVector3FromVector2(Vector2 a, Vector2 b, float y, out Vector3 min, out Vector3 max)
        {
            UnityHelpers.MinMax(a.x, b.x, out var mi, out var ma);
            min.x = mi;
            max.x = ma;
            min.y = y;
            max.y = y;
            UnityHelpers.MinMax(a.y, b.y, out mi, out ma);
            min.z = mi;
            max.z = ma;
        }
    }

}