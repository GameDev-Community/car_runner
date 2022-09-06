using DevourDev.Unity.ScriptableObjects;
using Externals.Utils.SaveManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

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


        public static void WriteSavable(this BinaryWriter bw, ISavable savable)
        {
            savable.Save(bw);
        }

        public static void WriteSavables(this BinaryWriter bw, IList<ISavable> savables)
        {
            var c = savables.Count;
            bw.Write(c);

            for (int i = -1; ++i < c;)
            {
                savables[i].Save(bw);
            }
        }

        public static void WriteSavablesT<T> (this BinaryWriter bw, ICollection<T> savables)
            where T : ISavable
        {
            var c = savables.Count;
            bw.Write(c);

            foreach (var item in savables)
            {
                item.Save(bw);
            }
        }

        public static void WriteGameDatabaseElement<T>(this BinaryWriter bw, T element)
            where T : GameDatabaseElement
        {
            bw.Write(element.DatabaseElementID);
        }

        public static void WriteGameDatabaseElements<T>(this BinaryWriter bw, ICollection<T> elements)
            where T : GameDatabaseElement
        {
            var c = elements.Count;

            if (c > 128)
            {
                int[] ids = System.Buffers.ArrayPool<int>.Shared.Rent(c);

                int i = -1;
                foreach (var item in elements)
                {
                    ids[++i] = item.DatabaseElementID;
                }

                WriteUnsafe<int>(bw, ids);
                System.Buffers.ArrayPool<int>.Shared.Return(ids, false);
            }
            else
            {
                Span<int> ids = stackalloc int[c];

                int i = -1;
                foreach (var item in elements)
                {
                    ids[++i] = item.DatabaseElementID;
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
