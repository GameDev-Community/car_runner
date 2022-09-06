using DevourDev.Unity.ScriptableObjects;
using Externals.Utils.SaveManager;
using System;
using System.Collections.Generic;
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

        public static T ReadSavable<T>(this BinaryReader br)
            where T : ISavable, new()
        {
            var savable = new T();
            savable.Load(br);
            return savable;
        }

        public static T[] ReadSavables<T>(this BinaryReader br)
            where T : ISavable, new()
        {
            var c = br.ReadInt32();
            var arr = new T[c];

            for (int i = -1; ++i < c;)
            {
                var x = new T();
                x.Load(br);
                arr[i] = x;
            }

            return arr;
        }

        public static void ReadSavablesNonAlloc<T>(this BinaryReader br, List<T> buffer)
           where T : ISavable, new()
        {
            var c = br.ReadInt32();

            var minCapacity = buffer.Count + c;

            if (buffer.Capacity < minCapacity)
            {
                int cap = buffer.Count * 2;

                if (cap < minCapacity)
                    cap = minCapacity;

                buffer.Capacity = cap;

            }

            for (int i = -1; ++i < c;)
            {
                var x = new T();
                x.Load(br);
                buffer.Add(x);
            }
        }

        public static void ReadSavablesNonAllocToCollection<T>(this BinaryReader br, ICollection<T> buffer)
         where T : ISavable, new()
        {
            var c = br.ReadInt32();

            for (int i = -1; ++i < c;)
            {
                var x = new T();
                x.Load(br);
                buffer.Add(x);
            }
        }

        public static void ReadSavablesNonAllocToRentedBuffer<T>(this BinaryReader br, out System.Buffers.ArrayPool<T> pool, out T[] arr, out int length)
         where T : ISavable, new()
        {
            length = br.ReadInt32();
            pool = System.Buffers.ArrayPool<T>.Shared;
            arr = pool.Rent(length);

            for (int i = -1; ++i < length;)
            {
                var x = new T();
                x.Load(br);
                arr[i] = x;
            }
        }

        public static T ReadGameDatabaseElement<T>(this BinaryReader br, GameDatabase<T> database)
            where T : GameDatabaseElement
        {
            return database.GetElement(br.ReadInt32());
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

        public static void ReadGameDatabaseElementsNonAlloc<T>(this BinaryReader br, GameDatabase<T> database, List<T> buffer)
            where T : GameDatabaseElement
        {
            var ids = ReadArrayUnsafe<int>(br);
            var c = ids.Length;

            var minCapacity = buffer.Count + c;

            if (buffer.Capacity < minCapacity)
            {
                int cap = buffer.Count * 2;

                if (cap < minCapacity)
                    cap = minCapacity;

                buffer.Capacity = cap;

            }

            for (int i = -1; ++i < c;)
            {
                var x = database.GetElement(ids[i]);
                buffer.Add(x);
            }
        }

        public static void ReadGameDatabaseElementsNonAllocToCollection<T>(this BinaryReader br, GameDatabase<T> database, ICollection<T> buffer)
            where T : GameDatabaseElement
        {
            var ids = ReadArrayUnsafe<int>(br);
            var c = ids.Length;

            for (int i = -1; ++i < c;)
            {
                var x = database.GetElement(ids[i]);
                buffer.Add(x);
            }
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
