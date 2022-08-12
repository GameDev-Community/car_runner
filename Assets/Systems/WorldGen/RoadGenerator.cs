using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
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
        [Tooltip("In ticks (1/10_000")]
        [SerializeField] private int _maxProcessTime = 5000;
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

            _chunks = new Chunk[_chunksBufferSize];
            _tailerChunkIndex = 0;
            _headerChunkIndex = -1;
        }


        #region Async tests
        float _generateDelta = 0.01f;
        float _timeToGenerateLeft;

        private readonly object _genLocker = new();
        private object _incrChunkIndexLocker = new();


        public enum GenerateBlockMode
        {
            Default,
            Coroutine,
            Async
        }

        [SerializeField] private GenerateBlockMode _generateBlockMode;

        private void Update()
        {
            if ((_timeToGenerateLeft -= Time.deltaTime) < 0)
            {
                _timeToGenerateLeft += _generateDelta;

                switch (_generateBlockMode)
                {
                    case GenerateBlockMode.Coroutine:
                        StartCoroutine(GenerateBlockCoroutine());
                        return;
                    case GenerateBlockMode.Async:
                        GenerateBlockAsync();
                        return;
                    default:
                        GenerateBlock();
                        return;
                }
            }
        }

        #endregion


        public void GenerateBlock()
        {
            var p = _nextPoint;
            IncrNextPoint();

            List<GameObject> content = ListPool<GameObject>.Get();
            var block = _roadBlock.GetItemInstance();
            content.Add(block.gameObject);
            var blockTr = block.transform;
            blockTr.position = p;

            float y = _roadBlock.SquareBounds.LocalPivot.y;

            GenerateItems<ItemWithChance<SquareBoundedItemDefault>,
            SquareBoundedItemDefault>(content, blockTr, y,
            _miscsAmount, _miscs, _miscsMap, _miscsRandom, _positionsRandom,
            null, true);

            GenerateItems<ItemWithChance<SquareBoundedInteractableItem>,
            SquareBoundedInteractableItem>(content, blockTr, y,
            _interactablesAmount, _interactables, _interactablesMap,
            _interactablesRandom, _positionsRandom, _interactablesGoodChanceCurve,
            false);

            Chunk chunk = new(content, p);
            AddChunk(chunk);

            if (_chunksCount == _chunksBufferSize)
                RemoveLastChunk();
        }


        public IEnumerator GenerateBlockCoroutine()
        {
            var p = _nextPoint;
            IncrNextPoint();

            List<GameObject> content = ListPool<GameObject>.Get();
            var block = _roadBlock.GetItemInstance();
            content.Add(block.gameObject);
            var blockTr = block.transform;
            blockTr.position = p;

            float y = _roadBlock.SquareBounds.LocalPivot.y;

            var co = StartCoroutine(GenerateItemsCoroutine<ItemWithChance<SquareBoundedItemDefault>,
                SquareBoundedItemDefault>(content, blockTr, y,
                _miscsAmount, _miscs, _miscsMap, _miscsRandom, _positionsRandom,
                null, true));

            yield return co;

            co = StartCoroutine(GenerateItemsCoroutine<ItemWithChance<SquareBoundedInteractableItem>,
             SquareBoundedInteractableItem>(content, blockTr, y,
             _interactablesAmount, _interactables, _interactablesMap,
             _interactablesRandom, _positionsRandom, _interactablesGoodChanceCurve,
             false));

            yield return co;

            Chunk chunk = new(content, p);
            AddChunk(chunk);

            if (_chunksCount == _chunksBufferSize)
                RemoveLastChunk();
        }

        public async Task GenerateBlockAsync()
        {
            var p = _nextPoint;
            IncrNextPoint();

            List<GameObject> content = ListPool<GameObject>.Get();
            var block = _roadBlock.GetItemInstance();
            content.Add(block.gameObject);
            var blockTr = block.transform;
            blockTr.position = p;

            float y = _roadBlock.SquareBounds.LocalPivot.y;

            await GenerateItemsAsync<ItemWithChance<SquareBoundedItemDefault>,
              SquareBoundedItemDefault>(content, blockTr, y,
              _miscsAmount, _miscs, _miscsMap, _miscsRandom, _positionsRandom,
              null, true);

            await GenerateItemsAsync<ItemWithChance<SquareBoundedInteractableItem>,
              SquareBoundedInteractableItem>(content, blockTr, y,
              _interactablesAmount, _interactables, _interactablesMap,
              _interactablesRandom, _positionsRandom, _interactablesGoodChanceCurve,
              false);

            lock (_genLocker)
            {
                Chunk chunk = new(content, p);
                AddChunk(chunk);
            }

            if (_chunksCount == _chunksBufferSize)
                await RemoveLastChunkAsync();

        }

        //static int _id = 0;
        private void GenerateItems<T, T1>(List<GameObject> content, Transform blockTr, float y,
            Vector2 itemsAmountRange, T[] itemsArr, int[] itemsRandomMap, System.Random genR,
            System.Random posR, AnimationCurve genCurve = null, bool randomRot = false)
            where T : ItemWithChance<T1>
            where T1 : ISquareBoundedItem
        {
            using var pooledItem = ListPool<Vector4>.Get(out var itemsBounds);
            int itemsAmount = genR.Next((int)itemsAmountRange.x, (int)itemsAmountRange.y);

            //itemsAmount = 1; // debug

            int randomMapLen = itemsRandomMap.Length;
            int i = -1;
LoopStart:
            for (; ++i < itemsAmount;)
            {
                int mapIndex;

                if (genCurve == null)
                {
                    mapIndex = genR.Next(0, randomMapLen);
                }
                else
                {
                    var rawV = RandomHelpers.RandomFloat(genR, 0, randomMapLen, genCurve);
                    mapIndex = (int)rawV;
                    if (mapIndex < 0 || mapIndex >= itemsRandomMap.Length)
                    {
                        UnityEngine.Debug.LogError($"irm: {itemsRandomMap.Length}, index: {mapIndex}, rawV: {rawV}");
                    }
                }

                int itemIndex = itemsRandomMap[mapIndex];
                var item = itemsArr[itemIndex].Item;
                var bounds = item.SquareBounds;

                var rangeX = _roadXRange;
                rangeX.x -= bounds.PivotFromBoundsOffsetX.x;
                rangeX.y -= bounds.PivotFromBoundsOffsetX.y;

                var rangeZ = _roadZRange;
                rangeZ.x -= bounds.PivotFromBoundsOffsetY.x;
                rangeZ.y -= bounds.PivotFromBoundsOffsetY.y;

                Vector2 pos;
                pos.x = RandomHelpers.RandomFloat(posR, rangeX.x, rangeX.y);
                pos.y = RandomHelpers.RandomFloat(posR, rangeZ.x, rangeZ.y);

                bounds.GetCornersAtPosition_Vector2(pos, out var bmin, out var bmax);
                var newItemBounds = VectorHelpers.MergeVectors(bmin, bmax);

                //Debug.Log($"v4: {newItemBounds}");
                foreach (var otherBounds in itemsBounds)
                {
                    if (VectorHelpers.DetectCollision(newItemBounds.x, newItemBounds.z, otherBounds.x, otherBounds.z)
                     && VectorHelpers.DetectCollision(newItemBounds.y, newItemBounds.w, otherBounds.y, otherBounds.w))
                    {
                        //collision!
                        //UnityEngine.Debug.Log("collision");
                        goto LoopStart;
                    }
                }


                var inst = item.GetItemInstance(blockTr);
                //inst.gameObject.name = _id++.ToString();
                inst.transform.localPosition = new Vector3(pos.x, y, pos.y) + bounds.CenterOffset;
                //UnityEngine.Debug.Log($"local: {inst.transform.localPosition}\n" +
                //    $"global: {inst.transform.position}\n" +
                //    $"target: {new Vector3(pos.x, y, pos.y)}, offset: {bounds.CenterOffset}\n" +
                //    $"name: {inst.name}");

                if (randomRot)
                    inst.transform.localRotation = RandomHelpers.RandomIdentityLikeRotation();

                content.Add(inst.gameObject);

                if (i + 1 == itemsAmount)
                    return;

                itemsBounds.Add(newItemBounds);
            }
        }

        private IEnumerator GenerateItemsCoroutine<T, T1>(List<GameObject> content, Transform blockTr, float y,
            Vector2 itemsAmountRange, T[] itemsArr, int[] itemsRandomMap, System.Random genR,
            System.Random posR, AnimationCurve genCurve = null, bool randomRot = false)
          where T : ItemWithChance<T1>
          where T1 : ISquareBoundedItem
        {
            using var pooledItem = ListPool<Vector4>.Get(out var itemsBounds);
            int itemsAmount = genR.Next((int)itemsAmountRange.x, (int)itemsAmountRange.y);

            //itemsAmount = 1; // debug

            int randomMapLen = itemsRandomMap.Length;
            int i = -1;
            var timeStamp0 = Stopwatch.GetTimestamp();
LoopStart:
            for (; ++i < itemsAmount;)
            {
                if (Stopwatch.GetTimestamp() - timeStamp0 > _maxProcessTime)
                {
                    yield return null;
                    timeStamp0 = Stopwatch.GetTimestamp();
                }

                int mapIndex;

                if (genCurve == null)
                {
                    mapIndex = genR.Next(0, randomMapLen);
                }
                else
                {
                    var rawV = RandomHelpers.RandomFloat(genR, 0, randomMapLen, genCurve);
                    mapIndex = (int)rawV;
                    if (mapIndex < 0 || mapIndex >= itemsRandomMap.Length)
                    {
                        UnityEngine.Debug.LogError($"irm: {itemsRandomMap.Length}, index: {mapIndex}, rawV: {rawV}");
                    }
                }


                int itemIndex = itemsRandomMap[mapIndex];
                var item = itemsArr[itemIndex].Item;
                var bounds = item.SquareBounds;

                var rangeX = _roadXRange;
                rangeX.x -= bounds.PivotFromBoundsOffsetX.x;
                rangeX.y -= bounds.PivotFromBoundsOffsetX.y;

                var rangeZ = _roadZRange;
                rangeZ.x -= bounds.PivotFromBoundsOffsetY.x;
                rangeZ.y -= bounds.PivotFromBoundsOffsetY.y;

                Vector2 pos;
                pos.x = RandomHelpers.RandomFloat(posR, rangeX.x, rangeX.y);
                pos.y = RandomHelpers.RandomFloat(posR, rangeZ.x, rangeZ.y);

                bounds.GetCornersAtPosition_Vector2(pos, out var bmin, out var bmax);
                var newItemBounds = VectorHelpers.MergeVectors(bmin, bmax);

                //Debug.Log($"v4: {newItemBounds}");
                foreach (var otherBounds in itemsBounds)
                {
                    if (VectorHelpers.DetectCollision(newItemBounds.x, newItemBounds.z, otherBounds.x, otherBounds.z)
                     && VectorHelpers.DetectCollision(newItemBounds.y, newItemBounds.w, otherBounds.y, otherBounds.w))
                    {
                        //collision!
                        //UnityEngine.Debug.Log("collision");
                        goto LoopStart;
                    }
                }


                var inst = item.GetItemInstance(blockTr);

                //inst.gameObject.name = _id++.ToString();

                inst.transform.localPosition = new Vector3(pos.x, y, pos.y) + bounds.CenterOffset;

                //UnityEngine.Debug.Log($"local: {inst.transform.localPosition}\n" +
                //    $"global: {inst.transform.position}\n" +
                //    $"target: {new Vector3(pos.x, y, pos.y)}, offset: {bounds.CenterOffset}\n" +
                //    $"name: {inst.name}");

                if (randomRot)
                    inst.transform.localRotation = RandomHelpers.RandomIdentityLikeRotation();

                content.Add(inst.gameObject);

                if (i + 1 == itemsAmount)
                    yield break;

                itemsBounds.Add(newItemBounds);
            }
        }


        private async Task GenerateItemsAsync<T, T1>(List<GameObject> content, Transform blockTr, float y,
            Vector2 itemsAmountRange, T[] itemsArr, int[] itemsRandomMap, System.Random genR,
            System.Random posR, AnimationCurve genCurve = null, bool randomRot = false)
       where T : ItemWithChance<T1>
       where T1 : ISquareBoundedItem
        {
            using var pooledItem = ListPool<Vector4>.Get(out var itemsBounds);
            int itemsAmount = genR.Next((int)itemsAmountRange.x, (int)itemsAmountRange.y);

            //itemsAmount = 1; // debug

            int randomMapLen = itemsRandomMap.Length;
            int i = -1;
            var timeStamp0 = Stopwatch.GetTimestamp();
LoopStart:
            for (; ++i < itemsAmount;)
            {
                if (Stopwatch.GetTimestamp() - timeStamp0 > _maxProcessTime)
                {
                    await Task.Yield();
                    Utils.UnityTaskCostil.ThrowIfCancellationRequested();
                    timeStamp0 = Stopwatch.GetTimestamp();
                }

                int mapIndex;

                if (genCurve == null)
                {
                    mapIndex = genR.Next(0, randomMapLen);
                }
                else
                {
                    var rawV = RandomHelpers.RandomFloat(genR, 0, randomMapLen, genCurve);
                    mapIndex = (int)rawV;
                    if (mapIndex < 0 || mapIndex >= itemsRandomMap.Length)
                    {
                        UnityEngine.Debug.LogError($"irm: {itemsRandomMap.Length}, index: {mapIndex}, rawV: {rawV}");
                    }
                }


                int itemIndex = itemsRandomMap[mapIndex];
                var item = itemsArr[itemIndex].Item;
                var bounds = item.SquareBounds;

                var rangeX = _roadXRange;
                rangeX.x -= bounds.PivotFromBoundsOffsetX.x;
                rangeX.y -= bounds.PivotFromBoundsOffsetX.y;

                var rangeZ = _roadZRange;
                rangeZ.x -= bounds.PivotFromBoundsOffsetY.x;
                rangeZ.y -= bounds.PivotFromBoundsOffsetY.y;

                Vector2 pos;
                pos.x = RandomHelpers.RandomFloat(posR, rangeX.x, rangeX.y);
                pos.y = RandomHelpers.RandomFloat(posR, rangeZ.x, rangeZ.y);

                bounds.GetCornersAtPosition_Vector2(pos, out var bmin, out var bmax);
                var newItemBounds = VectorHelpers.MergeVectors(bmin, bmax);

                //Debug.Log($"v4: {newItemBounds}");
                foreach (var otherBounds in itemsBounds)
                {
                    if (VectorHelpers.DetectCollision(newItemBounds.x, newItemBounds.z, otherBounds.x, otherBounds.z)
                     && VectorHelpers.DetectCollision(newItemBounds.y, newItemBounds.w, otherBounds.y, otherBounds.w))
                    {
                        //collision!
                        //UnityEngine.Debug.Log("collision");
                        goto LoopStart;
                    }
                }


                var inst = item.GetItemInstance(blockTr);

                //inst.gameObject.name = _id++.ToString();

                //inst.transform.localPosition = new Vector3(pos.x, y, pos.y) + bounds.CenterOffset;
                inst.transform.localPosition = new Vector3(pos.x, y, pos.y);

                //UnityEngine.Debug.Log($"local: {inst.transform.localPosition}\n" +
                //    $"global: {inst.transform.position}\n" +
                //    $"target: {new Vector3(pos.x, y, pos.y)}, offset: {bounds.CenterOffset}\n" +
                //    $"name: {inst.name}");

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
            --_chunksCount;
            var c = _chunks[_tailerChunkIndex];
            _chunks[_tailerChunkIndex] = null;
            IncrChunkIndex(ref _tailerChunkIndex);

            c.DestroyChunk();
        }

        private async Task RemoveLastChunkAsync()
        {
            if (_chunksCount == 0)
                return;
            --_chunksCount;
            var c = _chunks[_tailerChunkIndex];
            _chunks[_tailerChunkIndex] = null;
            IncrChunkIndex(ref _tailerChunkIndex);

            await c.DestroyChunkAsync(_maxProcessTime);
        }

        private int IncrChunkIndex(ref int i)
        {
            lock (_incrChunkIndexLocker)
            {
                if (++i == _chunksBufferSize)
                    i = 0;

                return i;
            }
            
        }
        private void IncrNextPoint()
        {
            _nextPoint += _roadLength;
        }
    }
}