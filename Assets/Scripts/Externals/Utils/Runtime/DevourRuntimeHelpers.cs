using System;
using System.IO;
using UnityEngine;

namespace Externals.Utils.Runtime
{
    public static class DevourRuntimeHelpers
    {
        private const string _unityAssetsFolderName = "Assets";
        private const string _unityAssetsDirectory = "/Assets/";
        private const string _unityAssetsDirectory2 = @"\Assets\";
        //это явно не отсюда, но мне похуй + я в ахуе
        public static string AbsPathToUnityRelative(DirectoryInfo directory)
        {
            var full = directory.FullName;
            var splits = full.Split(_unityAssetsDirectory);

            if (splits.Length == 1)
                splits = full.Split(_unityAssetsDirectory2);

            return Path.Combine(_unityAssetsFolderName, splits[^1]);
        }

        public static Sprite SpriteFromTexture(Texture2D t)
        {
            return Sprite.Create(t, Rect.MinMaxRect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
        }
        public static void ThrowIfNegative(float v)
        {
            if (v < 0)
                throw new Exception($"value should not be negative ({v})");
        }

        public static void ThrowIfNegative(double v)
        {
            if (v < 0)
                throw new Exception($"value should not be negative ({v})");
        }

        public static void ThrowIfNegative(int v)
        {
            if (v < 0)
                throw new Exception($"value should not be negative ({v})");
        }

        public static void ThrowIfNegative(long v)
        {
            if (v < 0)
                throw new Exception($"value should not be negative ({v})");
        }

        public static void ThrowIfNaN(float v)
        {
            if (float.IsNaN(v))
                throw new Exception($"value should not be NaN");
        }

        public static void ThrowIfNaN(double v)
        {
            if (double.IsNaN(v))
                throw new Exception($"value should not be NaN");
        }

        public static void ThrowIfInfinityOrNaN(double v)
        {
            ThrowIfNaN(v);
            ThrowIfInfinity(v);
        }

        public static void ThrowIfInfinity(double v)
        {
            if (double.IsInfinity(v))
                throw new Exception($"value should not be infinity");
        }
    }
}
