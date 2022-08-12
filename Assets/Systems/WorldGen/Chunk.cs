using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Systems.WorldGen
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
    }
}