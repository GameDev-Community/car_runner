using System.Runtime.CompilerServices;

namespace DevourDev.Unity.Utils.SimpleStats.Modifiers
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
    }
}

