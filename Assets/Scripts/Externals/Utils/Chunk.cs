using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace Utils
{
    public class Chunk
    {
        private readonly List<GameObject> _content;
        private readonly Vector3 _position;


        public Chunk(List<GameObject> content, Vector3 pos)
        {
            _content = content;
            _position = pos;
        }


        public Vector3 EndPoint => _position;


        public void DestroyChunk()
        {
            var list = _content;
            var c = list.Count;

            for (int i = -1; ++i < c;)
                GameObject.Destroy(list[i]);

            ListPool<GameObject>.Release(list);
        }

        public async Task DestroyChunkAsync(int maxProcessTime)
        {
            var list = _content;
            var c = list.Count;

            var ts0 = Stopwatch.GetTimestamp();
            for (int i = -1; ++i < c;)
            {
                GameObject.Destroy(list[i]);

                if (Stopwatch.GetTimestamp() - ts0 > maxProcessTime)
                {
                    await Task.Yield();
                    Utils.UnityTaskCostil.ThrowIfCancellationRequested();
                    ts0 = Stopwatch.GetTimestamp();
                }
            }

            ListPool<GameObject>.Release(list);
        }
    }

}