using DevourDev.Unity.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Externals.Utils.Extentions
{
    public static class BinaryWriterExtentions
    {
        public static void Write(this BinaryWriter bw, int[] v)
        {
            WriteUnsafe(bw, v);
        }

        public static void Write(this BinaryWriter bw, float[] v)
        {
            WriteUnsafe(bw, v);
        }


        public static void WriteGameDatabaseElements<T>(this BinaryWriter bw, IList<T> elements)
            where T : GameDatabaseElement
        {
            var c = elements.Count;

            if (c > 128)
            {
                int[] ids = System.Buffers.ArrayPool<int>.Shared.Rent(c);

                for (int i = -1; ++i < c;)
                {
                    ids[i] = elements[i].DatabaseElementID;
                }

                WriteUnsafe<int>(bw, ids);
                System.Buffers.ArrayPool<int>.Shared.Return(ids, false);
            }
            else
            {
                Span<int> ids = stackalloc int[c];

                for (int i = -1; ++i < c;)
                {
                    ids[i] = elements[i].DatabaseElementID;
                }

                WriteUnsafe<int>(bw, ids);
            }

        }


        /// <summary>
        /// не все структуры могут преобразовываться в байты
        /// корректно, особенно пользовательские структуры с
        /// размером не кратным 4 (байтам)
        /// </summary>
        public static unsafe void WriteUnsafe<TFrom>(this BinaryWriter bw, TFrom[] v)
            where TFrom : struct
        {
            var size = v.Length * Marshal.SizeOf<TFrom>();
            bw.Write(size);
            var bytes = MemoryMarshal.Cast<TFrom, byte>(v);
            bw.BaseStream.Write(bytes);
        }

        /// <summary>
        /// не все структуры могут преобразовываться в байты
        /// корректно, особенно пользовательские структуры с
        /// размером не кратным 4 (байтам)
        /// </summary>
        public static unsafe void WriteUnsafe<TFrom>(this BinaryWriter bw, Span<TFrom> v)
            where TFrom : struct
        {
            var size = v.Length * Marshal.SizeOf<TFrom>();
            bw.Write(size);
            var bytes = MemoryMarshal.Cast<TFrom, byte>(v);
            bw.BaseStream.Write(bytes);
        }
    }
}
