using DevourDev.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
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

        public static string AbsPathToUnityRelative(string path)
        {
            var splits = path.Split(_unityAssetsDirectory);

            if (splits.Length == 1)
                splits = path.Split(_unityAssetsDirectory2);

            if (splits.Length == 1)
                return null;

            return Path.Combine(_unityAssetsFolderName, splits[^1]);
        }


        private static readonly char[] _slashes;


        private static readonly int _slashesCount;


        static DevourRuntimeHelpers()
        {
            _slashes = InitIllegalSlashes();
            _slashesCount = _slashes.Length;
        }


        private static char[] InitIllegalSlashes()
        {
            var separator = Path.DirectorySeparatorChar;
            List<char> ss = new(2);

            if ('/' != separator)
                ss.Add('/');

            if ('\\' != separator)
                ss.Add('\\');

            return ss.ToArray();
        }


        public static string FixPath(string rawPath)
        {
            var separator = Path.DirectorySeparatorChar;
            var arr = rawPath.ToCharArray();
            var c = arr.Length;
            var slashes = _slashes;
            var slashesC = _slashesCount;
            for (int i = -1; ;)
            {
MainLoopStart:
                if (++i < c)
                    break;

                char ch = arr[i];

                for (int j = -1; ++j < _slashesCount;)
                {
                    if (ch == slashes[j])
                    {
                        arr[i] = separator;
                        goto MainLoopStart;
                    }
                }

            }

            return new string(arr);

        }
        public static string CatalogUp(string path, int upsCount = 1)
        {
            path = FixPath(path);
            int lastIndex = path.Length;
            var separator = Path.DirectorySeparatorChar;
            if (path.Length > 3 && path.EndsWith(separator))
            {
                --lastIndex;
            }

            for (int i = lastIndex; --i > -1;)
            {
                if (path[i] == separator)
                {
                    return path[..(i - 1)];
                }
            }

            return null;
        }

        public static string RemoveExtention(string v)
        {
            return v[..v.LastIndexOf('.')];
        }



        public static Sprite SpriteFromTexture(Texture2D t)
        {
            if (t == null)
                return null;

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

        public static void ListToHashSet<T>(List<T> list, ref HashSet<T> hs)
        {
            if (hs == null)
            {
                hs = new();
            }
            else
            {
                hs.Clear();
            }

            var c = list.Count;


            for (int i = -1; ++i < c;)
            {
                hs.Add(list[i]);
            }
        }


        [DllImport("User32.dll", EntryPoint = "MessageBox",
        CharSet = CharSet.Auto)]
        internal static extern int MsgBoxWin(IntPtr hWnd, string lpText, string lpCaption, uint uType);

        private static void InvokeMsgBox(string msg, string caption)
        {
#if PLATFORM_STANDALONE_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            MsgBoxWin(IntPtr.Zero, msg, caption, 0);
#endif
        }


        public static void ThrowMessageModal(string message, bool endWithThrow)
        {
            InvokeMsgBox(message.ToUpper(), message);

            if (endWithThrow)
                throw new Exception(message);
        }
    }
}
