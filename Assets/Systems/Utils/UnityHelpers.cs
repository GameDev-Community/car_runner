using System.Runtime.CompilerServices;
using UnityEngine;

namespace Utils
{
    public static class UnityHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MinMax(float a, float b, out float min, out float max)
        {
            if (a > b)
            {
                min = b;
                max = a;
                return;
            }

            min = a;
            max = b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetBoundsSquare(Bounds b)
        {
            var s = b.size;
            return s.x * s.z;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetBoundsSquare(Collider c)
        {
            return GetBoundsSquare(c.bounds);
        }
    }

}