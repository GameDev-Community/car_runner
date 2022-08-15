using System.Runtime.CompilerServices;

namespace DevourDev.Unity.Utils.SimpleStats.Modifiers
{
    public class ModifierOptimizer
    {
        public const long Coefficient = 1000;

        private long _flat;
        private long _mult;


        public ModifierOptimizer()
        {

        }

        public ModifierOptimizer(long flatData, long multData)
        {
            _flat = flatData;
            _mult = multData;
        }


        public float ModifyValue(float v)
        {
            ToSingles(out var flat, out var mult);
            return (v + flat) * (1 + mult);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetData(out long flat, out long mult)
        {
            flat = _flat;
            mult = _mult;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeFlatData(long delta)
        {
            _flat += delta;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeMultData(long delta)
        {
            _mult += delta;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeData(long flatDelta, long multDelta)
        {
            ChangeFlat(flatDelta);
            ChangeMult(multDelta);
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToDoubles(out double flat, out double mult)
        {
            flat = (double)_flat / Coefficient;
            mult = (double)_mult / Coefficient;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeFlat(double delta)
        {
            _flat += (long)(delta * Coefficient);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeMult(double delta)
        {
            _mult += (long)(delta * Coefficient);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Change(double flatDelta, double multDelta)
        {
            ChangeFlat(flatDelta);
            ChangeMult(multDelta);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToSingles(out float flat, out float mult)
        {
            flat = (float)_flat / Coefficient;
            mult = (float)_mult / Coefficient;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeFlat(float delta)
        {
            _flat += (long)(delta * Coefficient);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeMult(float delta)
        {
            _mult += (long)(delta * Coefficient);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Change(float flatDelta, float multDelta)
        {
            ChangeFlat(flatDelta);
            ChangeMult(multDelta);
        }


        /// <summary>
        /// writes 16 bytes to memory from index
        /// </summary>
        /// <param name="mem"></param>
        /// <param name="index"></param>
        public unsafe void GetBytes(System.Memory<byte> mem, int index)
        {
            byte* array = stackalloc byte[8];
            var data = mem.Span;
            *(long*)array = _flat;

            for (int i = -1; ++i < 8;)
                data[index + i] = *(array + i);

            *(long*)array = _mult;

            for (int i = -1; ++i < 8;)
                data[index + i + 8] = *(array + i);
        }

        /// <summary>
        /// 16 bytes
        /// </summary>
        /// <returns></returns>
        public unsafe byte[] GetBytes()
        {
            byte* array0 = stackalloc byte[8];
            byte* array1 = stackalloc byte[8];
            *(long*)array0 = _flat;
            *(long*)array1 = _mult;

            var data = new byte[16];

            fixed (byte* d = data)
            {
                for (int i = -1; ++i < 8;)
                {
                    *(d + i) = *(array0 + i);
                }

                for (int i = -1; ++i < 8;)
                {
                    *(d + 8 + i) = *(array1 + i);
                }
            }

            return data;
        }

        public static ModifierOptimizer FromBytes(System.Memory<byte> mem, int index)
        {
            var x = mem.Slice(index, 16).Span;
            long flat = System.BitConverter.ToInt64(x);
            long mult = System.BitConverter.ToInt64(x[8..]);
            return new ModifierOptimizer(flat, mult);
        }

    }
}

