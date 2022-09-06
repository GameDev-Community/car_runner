using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Externals.Utils.Runtime
{
    public static class UnityAsyncHelpers
    {
        private static bool _cancellationRequested;


        public static bool CancellationRequested => _cancellationRequested;


        public static void CancelAllTasks() => _cancellationRequested = true;

        public static void ThrowIfCancellationRequested()
        {
#if UNITY_EDITOR //для билда нужно сделать AsyncHandler монобех, который будет CancellAllTasks в OnDestroy/OnQuit
            _cancellationRequested = !UnityEngine.Application.isPlaying;
#endif

            if (_cancellationRequested)
            {
                throw new System.Threading.Tasks.TaskCanceledException("Task cancelled from " + nameof(UnityAsyncHelpers));
            }

        }


    }

    public static class UnityAsyncExtentions
    {
        public static async Task TranslateAsync(this Transform tr, Vector3 fullTranslation, float time, Space relativeTo)
        {
            if (relativeTo == Space.Self)
                fullTranslation = tr.TransformDirection(fullTranslation);

            float x = fullTranslation.x;
            float y = fullTranslation.y;
            float z = fullTranslation.z;

            float xx = x * x;
            float yy = y * y;
            float zz = z * z;

            double left = System.Math.Sqrt((double)(xx + yy + zz));

            float sdx = x / time;
            float sdy = y / time;
            float sdz = z / time;

            Vector3 dest = tr.position + fullTranslation;

            for (; ; )
            {
                UnityAsyncHelpers.ThrowIfCancellationRequested();

                float dt = UnityEngine.Time.deltaTime;
                float dx = sdx * dt;
                float dy = sdy * dt;
                float dz = sdz * dt;

                float dxx = dx * dx;
                float dyy = dy * dy;
                float dzz = dz * dz;

                left -= System.Math.Sqrt((double)(dxx + dyy + dzz));

                if (left <= 0)
                    break;

                Vector3 newPos = tr.position;
                newPos.x += dx;
                newPos.y += dy;
                newPos.z += dz;

                await Task.Yield();
            }

            tr.position = dest;
        }

        public static async Task TranslateAsync(this Transform tr, Vector3 fullTranslation, float time, Space relativeTo, CancellationToken token)
        {
            if (relativeTo == Space.Self)
                fullTranslation = tr.TransformDirection(fullTranslation);

            float x = fullTranslation.x;
            float y = fullTranslation.y;
            float z = fullTranslation.z;

            float xx = x * x;
            float yy = y * y;
            float zz = z * z;

            double left = System.Math.Sqrt((double)(xx + yy + zz));

            float sdx = x / time;
            float sdy = y / time;
            float sdz = z / time;

            Vector3 dest = tr.position + fullTranslation;

            for (; ; )
            {
                UnityAsyncHelpers.ThrowIfCancellationRequested();
                token.ThrowIfCancellationRequested();

                float dt = UnityEngine.Time.deltaTime;
                float dx = sdx * dt;
                float dy = sdy * dt;
                float dz = sdz * dt;

                float dxx = dx * dx;
                float dyy = dy * dy;
                float dzz = dz * dz;

                left -= System.Math.Sqrt((double)(dxx + dyy + dzz));

                if (left <= 0)
                    break;

                Vector3 newPos = tr.position;
                newPos.x += dx;
                newPos.y += dy;
                newPos.z += dz;

                await Task.Yield();
            }

            tr.position = dest;
        }
    }
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
