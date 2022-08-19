using System.Runtime.CompilerServices;

namespace Utils
{
    public static class MathModule
    {
        /// <param name="oV">old value</param>
        /// <param name="oa">old a (from)</param>
        /// <param name="ob">old b (to)</param>
        /// <param name="na">new a (from)</param>
        /// <param name="nb">new b (to)</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SaveRatio(float oV, float oa, float ob, float na, float nb)
        {
            var t = (oV - oa) / (ob - oa);
            return na + (nb - na) * t;
        }

        /// <param name="oV">old value</param>
        /// <param name="oa">old a (from)</param>
        /// <param name="ob">old b (to)</param>
        /// <param name="na">new a (from)</param>
        /// <param name="nb">new b (to)</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SaveRatio(int oV, int oa, int ob, int na, int nb)
        {
            var floatCalcsRes = SaveRatio((float)oV, (float)oa, (float)ob, (float)na, (float)nb);
#if UNITY_EDITOR
            var t = (oV - oa) / (ob - oa);
            var intCalcsRes = na + (nb - na) * t;
            UnityEngine.Debug.Log($"INT SAVE RATIO: int calcs: {intCalcsRes}, float calcs:" +
                $" {(int)floatCalcsRes} ({floatCalcsRes}).");
#endif

            if (oV == oa)
            {
                return na;
            }

            if (oV == ob)
            {
                return nb;
            }

            return (int)floatCalcsRes;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClampLongToInt(long value) => ClampLongToInt(value, int.MinValue, int.MaxValue);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClampLongToInt(long value, int min, int max)
        {
            if (value <= min)
            {
                return min;
            }

            if (value >= max)
            {
                return max;
            }

            return (int)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClampLongToInt(long value, int min, int max, out bool reachedMin, out bool reachedMax)
        {
            if (value <= min)
            {
                reachedMin = true;
                reachedMax = false;
                return min;
            }

            if (value >= max)
            {
                reachedMin = false;
                reachedMax = true;
                return max;
            }

            reachedMin = reachedMax = false;
            return (int)value;
        }

        /// <summary>
        /// Clamps <paramref name="value"/> between <paramref name="min"/> and
        /// <paramref name="max"/> and registrate hitting lower or higher bounds
        /// </summary>
        /// <returns><paramref name="min"/> if <paramref name="value"/> is lower
        /// or equal to <paramref name="min"/>;
        /// <paramref name="max"/> if <paramref name="value"/> is higher or
        /// equal to <paramref name="max"/>;
        /// if bounds are equals to each other, <paramref name="reachedMin"/>
        /// will be registered</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float value, float min, float max, out bool reachedMin, out bool reachedMax)
        {
            if (value <= min)
            {
                reachedMin = true;
                reachedMax = false;
                return min;
            }

            if (value >= max)
            {
                reachedMin = false;
                reachedMax = true;
                return max;
            }

            reachedMin = reachedMax = false;
            return value;
        }

        /// <summary>
        /// Clamps <paramref name="value"/> between <paramref name="min"/> and
        /// <paramref name="max"/> and registrate hitting lower or higher bounds
        /// </summary>
        /// <returns><paramref name="min"/> if <paramref name="value"/> is lower
        /// or equal to <paramref name="min"/>;
        /// <paramref name="max"/> if <paramref name="value"/> is higher or
        /// equal to <paramref name="max"/>;
        /// if bounds are equals to each other, <paramref name="reachedMin"/>
        /// will be registered</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int value, int min, int max, out bool reachedMin, out bool reachedMax)
        {
            if (value <= min)
            {
                reachedMin = true;
                reachedMax = false;
                return min;
            }

            if (value >= max)
            {
                reachedMin = false;
                reachedMax = true;
                return max;
            }

            reachedMin = reachedMax = false;
            return value;
        }
    }
}
