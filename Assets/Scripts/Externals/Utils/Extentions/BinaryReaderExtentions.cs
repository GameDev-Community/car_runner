using DevourDev.Unity.ScriptableObjects;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Externals.Utils.Extentions
{
    public static class BinaryReaderExtentions
    {
        public static int[] ReadInt32Array(this BinaryReader br)
        {
            return ReadArrayUnsafe<int>(br);
        }

        public static float[] ReadSinglesArray(this BinaryReader br)
        {
            return ReadArrayUnsafe<float>(br);
        }

        public static double[] ReadDoublesArray(this BinaryReader br)
        {
            return ReadArrayUnsafe<double>(br);
        }


        public static T[] ReadGameDatabaseElements<T>(this BinaryReader br, GameDatabase<T> database)
            where T : GameDatabaseElement
        {
            var ids = ReadArrayUnsafe<int>(br);
            var c = ids.Length;
            T[] els = new T[c];

            for (int i = -1; ++i < c;)
            {
                els[i] = database.GetElement(ids[i]);
            }

            return els;
        }



        /// <summary>
        /// не все структуры могут преобразовываться из байтов
        /// корректно, особенно пользовательские структуры с
        /// размером не кратным 4 (байтам)
        /// </summary>
        private static TTo[] ReadArrayUnsafe<TTo>(BinaryReader br)
            where TTo : struct
        {
            var size = br.ReadInt32();

            if (size > 512)
            {
                var rentedBuffer = System.Buffers.ArrayPool<byte>.Shared.Rent(size);
                var bytesRead = br.BaseStream.Read(rentedBuffer, 0, size);
#if UNITY_EDITOR
                if (bytesRead != size)
                    throw new Exception($"read {bytesRead} ({size} expected)");
#endif
                var ret = MemoryMarshal.Cast<byte, TTo>(new ReadOnlySpan<byte>(rentedBuffer, 0, size)).ToArray();
                System.Buffers.ArrayPool<byte>.Shared.Return(rentedBuffer);
                return ret;
            }
            else
            {
                Span<byte> bytes = stackalloc byte[size];
                var bytesRead = br.BaseStream.Read(bytes);
#if UNITY_EDITOR
                if (bytesRead != size)
                    throw new Exception($"read {bytesRead} ({size} expected)");
#endif
                var ret = MemoryMarshal.Cast<byte, TTo>(bytes).ToArray();
                return ret;
            }
        }

    }
}
