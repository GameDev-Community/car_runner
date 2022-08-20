using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Utils
{

    public static class RandomHelpers
    {
        private const int _quaternionsAmount = 1024;
        private static readonly System.Random _identityLikeRotationsRandom;
        private static readonly Quaternion[] _identityLikeRotations;


        static RandomHelpers()
        {
            var qs = new Quaternion[_quaternionsAmount];
            var r = new System.Random();

            for (int i = -1; ++i < _quaternionsAmount;)
            {
                var eu = new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());
                qs[i] = Quaternion.Euler(eu);
            }

            _identityLikeRotationsRandom = r;
            _identityLikeRotations = qs;

        }


        public static void Shuffle<T>(IList<T> col, System.Random r)
        {
            int n = col.Count;
            while (n > 1)
            {
                int k = r.Next(n--);
                (col[k], col[n]) = (col[n], col[k]);
            }
        }

        public static int[] GetRandomMap<T>(IRandomItem<T>[] ritems)
        {
            var c = ritems.Length;
            int mapSize = 0;

            for (int i = -1; ++i < c;)
            {
                mapSize += ritems[i].Chance;
            }

            int[] rmap = new int[mapSize];

            for (int i = -1, j = -1; ++i < c;)
            {
                var ritem = ritems[i];
                var chance = ritem.Chance;

                for (int x = -1; ++x < chance;)
                {
                    rmap[++j] = i;
                }
            }

            return rmap;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RandomFloat(System.Random r, float min, float max)
        {
            return (float)(r.NextDouble() * (max - min) + min);
        }


        public static float RandomFloat(System.Random r, float min, float max, AnimationCurve curve)
        {
            var mantissa = curve.Evaluate((float)r.NextDouble());
            return (float)(mantissa * (max - min) + min);
        }

        public static float RandomFloat(float min, float max, AnimationCurve curve)
        {
            float mantissa = UnityEngine.Random.value;

            if (curve != null)
                mantissa = curve.Evaluate(mantissa);

            return (float)(mantissa * (max - min) + min);
        }


        public static Quaternion RandomIdentityLikeRotation()
        {
            return RandomIdentityLikeRotation(_identityLikeRotationsRandom);
        }

        public static Quaternion RandomIdentityLikeRotation(System.Random r)
        {
            return _identityLikeRotations[r.Next(0, _quaternionsAmount)];
        }

        public static Vector3[] SpreadOverSquare_Vector3(Vector3 min, Vector3 max, int amount,
           System.Random r, float y = 0)
        {
            Vector3[] poses = new Vector3[amount];

            for (int i = -1; ++i < amount;)
            {
                float x = RandomFloat(r, min.x, max.x);
                float z = RandomFloat(r, min.z, max.z);

                poses[i] = new Vector3(x, y, z);
            }

            return poses;
        }


    }

}