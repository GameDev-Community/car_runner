using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Utils;
using Utils.Attributes;

namespace Systems.WorldGen
{
    public class RoadGenerator : MonoBehaviour
    {
        [System.Serializable]
        private class ItemWithChance<T> : IRandomItem<T>
            where T : ISquareBoundedItem
        {
            [SerializeField] private T _item;
            [SerializeField, Range(1, 10_000)] private int _chance;


            public T Item => _item;
            public int Chance => _chance;
        }


        [SerializeField] private SquareBoundedItemDefault _roadBlock;
        [Tooltip("blocks, spawning as is, without generating any objects on it.")]
        [SerializeField] private ItemWithChance<SquareBoundedItemDefault>[] _predefinedBlocks;
        [Tooltip("Every predef block can be spawned only 1 time (no dublicates)")]
        [SerializeField] private bool _uniquesPredefinedsOnly;
        [SerializeField] private Vector3 _roadLength;
        [SerializeField] private Transform _initialPos;
        [SerializeField, NonReorderable] private ItemWithChance<SquareBoundedItemDefault>[] _miscs;
        [SerializeField, MinMax(0, 10)] private Vector2 _miscsAmount = new Vector2Int(4, 8);
        [SerializeField, NonReorderable] private ItemWithChance<SquareBoundedInteractableItem>[] _interactables;
        [SerializeField, MinMax(0, 10)] private Vector2 _interactablesAmount = new Vector2(4, 8);
        [SerializeField, Min(32)] private int _chunksBufferSize = 128;
#if UNITY_EDITOR
        [SerializeField] private bool _sortInteractables;
#endif
        [Tooltip("v: from 0 to 1. Chances to positive interactables adjuster.")]
        [SerializeField] private AnimationCurve _interactablesGoodChanceCurve;

        private System.Random _miscsRandom;
        private System.Random _interactablesRandom;
        private System.Random _positionsRandom;

        private int[] _miscsMap;
        private int[] _interactablesMap;

        private Vector3 _nextPoint;

        private Vector2 _roadXRange;
        private Vector2 _roadZRange;

        private Chunk[] _chunks;
        private int _tailerChunkIndex;
        private int _headerChunkIndex;
        private int _chunksCount;


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_sortInteractables)
            {
                _sortInteractables = false;
                Array.Sort(_interactables, (x, y) => x.Item.InteractableObject.GoodnessValue
                .CompareTo(y.Item.InteractableObject.GoodnessValue));
            }
        }
#endif

        private void Start()
        {
            _miscsRandom = new(UnityEngine.Random.Range(0, int.MaxValue));
            _interactablesRandom = new(UnityEngine.Random.Range(0, int.MaxValue));
            _positionsRandom = new(UnityEngine.Random.Range(0, int.MaxValue));

            _miscsMap = RandomHelpers.GetRandomMap(_miscs);
            _interactablesMap = RandomHelpers.GetRandomMap(_interactables);
            _nextPoint = _initialPos.position;
            var bounds = _roadBlock.SquareBounds;
            _roadXRange = new(bounds.MinCorner.x, bounds.MaxCorner.x);
            _roadZRange = new(bounds.MinCorner.y, bounds.MaxCorner.y);
            UnityEngine.Debug.Log(_roadXRange);
            UnityEngine.Debug.Log(_roadZRange);

            _chunks = new Chunk[_chunksBufferSize];
            _tailerChunkIndex = 0;
            _headerChunkIndex = -1;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
                GenerateBlock();
        }

        public void GenerateBlock()
        {
            var p = _nextPoint;
            IncrNextPoint();

            List<GameObject> content = ListPool<GameObject>.Get();
            var block = _roadBlock.GetItemInstance();
            content.Add(block.gameObject);
            var blockTr = block.transform;

            float y = _roadBlock.SquareBounds.LocalPivot.y;

            //GenerateItems<ItemWithChance<SquareBoundedItemDefault>,
            //SquareBoundedItemDefault>(content, blockTr, y,
            //_miscsAmount, _miscs, _miscsMap, _miscsRandom, true);

            GenerateItems<ItemWithChance<SquareBoundedInteractableItem>,
            SquareBoundedInteractableItem>(content, blockTr, y,
            _interactablesAmount, _interactables, _interactablesMap,
            _interactablesRandom, false);

            blockTr.position = p;
            Chunk chunk = new(content, p);
            AddChunk(chunk);

            if (_chunksCount == _chunksBufferSize)
                RemoveLastChunk();
        }

        static int _id = 0;
        [Obsolete("ПРоверка коллизий не работает")]
        private void GenerateItems<T, T1>(List<GameObject> content, Transform blockTr, float y,
            Vector2 itemsAmountRange, T[] itemsArr, int[] itemsRandomMap, System.Random r, bool randomRot)
            where T : ItemWithChance<T1>
            where T1 : ISquareBoundedItem
        {
            using var pooledItem = ListPool<Vector4>.Get(out var itemsBounds);
            int itemsAmount = r.Next((int)itemsAmountRange.x, (int)itemsAmountRange.y);

            //itemsAmount = 1; // debug

            int randomMapLen = itemsRandomMap.Length;
            int i = -1;
            O4ko:
            for (; ++i < itemsAmount;)
            {
                int itemIndex = itemsRandomMap[r.Next(0, randomMapLen)];
                var item = itemsArr[itemIndex].Item;
                var bounds = item.SquareBounds;

                var rangeX = _roadXRange;
                rangeX.x -= bounds.PivotFromBoundsOffsetX.x;
                rangeX.y -= bounds.PivotFromBoundsOffsetX.y;

                var rangeZ = _roadZRange;
                rangeZ.x -= bounds.PivotFromBoundsOffsetY.x;
                rangeZ.y -= bounds.PivotFromBoundsOffsetY.y;

                Vector2 pos;
                pos.x = RandomHelpers.RandomFloat(r, rangeX.x, rangeX.y);
                pos.y = RandomHelpers.RandomFloat(r, rangeZ.x, rangeZ.y);

                bounds.GetCornersAtPosition_Vector2(pos, out var bmin, out var bmax);
                var newItemBounds = VectorHelpers.MergeVectors(bmin, bmax);

                Debug.Log($"v4: {newItemBounds}");
                foreach (var otherBounds in itemsBounds)
                {
                    if (VectorHelpers.DetectCollision(newItemBounds.x, newItemBounds.z, otherBounds.x, otherBounds.z)
                     && VectorHelpers.DetectCollision(newItemBounds.y, newItemBounds.w, otherBounds.y, otherBounds.w))
                    {
                        //collision!
                        UnityEngine.Debug.Log("collision");
                        goto O4ko;
                    }
                }


                var inst = item.GetItemInstance(blockTr);
                inst.gameObject.name = _id++.ToString();
                inst.transform.localPosition = new Vector3(pos.x, y, pos.y) + bounds.CenterOffset;
                UnityEngine.Debug.Log($"local: {inst.transform.localPosition}\n" +
                    $"global: {inst.transform.position}\n" +
                    $"target: {new Vector3(pos.x, y, pos.y)}, offset: {bounds.CenterOffset}\n" +
                    $"name: {inst.name}");

                if (randomRot)
                    inst.transform.localRotation = RandomHelpers.RandomIdentityLikeRotation();

                content.Add(inst.gameObject);

                if (i + 1 == itemsAmount)
                    return;

                itemsBounds.Add(newItemBounds);
            }
        }

        private void AddChunk(Chunk c)
        {
            _chunks[IncrChunkIndex(ref _headerChunkIndex)] = c;
            ++_chunksCount;
        }


        private void RemoveLastChunk()
        {
            if (_chunksCount == 0)
                return;

            _chunks[_tailerChunkIndex].DestroyChunk();
            _chunks[_tailerChunkIndex] = null;
            IncrChunkIndex(ref _tailerChunkIndex);
            --_chunksCount;
        }


        private int IncrChunkIndex(ref int i)
        {
            if (++i == _chunksBufferSize)
                i = 0;

            return i;
        }
        private void IncrNextPoint()
        {
            _nextPoint += _roadLength;
        }
    }
}