using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Utils
{
    [System.Obsolete("WIP. Todo: add expanding. Ok to use", false)]
    public class AdaptiveCollection<T> : IDisposable, IEnumerable<T>
    {
        [Serializable]
        private sealed class ArrayEnumeratorT : IEnumerator<T>
        {
            private T[] _arr;
            private int _index;
            private int _endIndex;
            private int _startIndex;
            private bool _complete;


            public T Current
            {
                get
                {
                    if (_index < _startIndex)
                    {
                        throw new InvalidOperationException("InvalidOperation_EnumNotStarted");
                    }

                    if (_complete)
                    {
                        throw new InvalidOperationException("InvalidOperation_EnumEnded");
                    }

                    return _arr[_index];
                }
            }

            object IEnumerator.Current => _arr[_index];


            internal ArrayEnumeratorT(T[] array, int startIndex, int count)
            {
                _arr = array;
                _startIndex = startIndex;
                _endIndex = startIndex + count;
                _index = _startIndex - 1;

            }


            public bool MoveNext()
            {
                if (_complete)
                {
                    _index = _endIndex;
                    return false;
                }

                _index++;
                _complete = _index >= _endIndex;
                return !_complete;
            }


            public void Reset()
            {
                _index = _startIndex - 1;
                _complete = false;
            }

            public void Dispose()
            {
            }
        }


        private enum ActiveCollection
        {
            None,
            Array,
            HashSet
        }


        private ActiveCollection _activeCollection;
        private readonly bool _refType;
        private T[] _itemsArr;
        private int _arrIndex = -1;
        private HashSet<T> _itemsHS;


        public AdaptiveCollection(int maxExpectingSize)
        {
            _refType = RuntimeHelpers.IsReferenceOrContainsReferences<T>();

            if (maxExpectingSize > 64)
            {
                if (maxExpectingSize > (_refType ? 128_000 : 32_000))
                    maxExpectingSize = 1024;

                _itemsHS = new(maxExpectingSize);
                _activeCollection = ActiveCollection.HashSet;
            }
            else
            {
                _itemsArr = System.Buffers.ArrayPool<T>.Shared.Rent(maxExpectingSize);
                _activeCollection = ActiveCollection.Array;
            }
        }


        public int Count
        {
            get
            {
                return _activeCollection switch
                {
                    ActiveCollection.Array => _arrIndex + 1,
                    ActiveCollection.HashSet => _itemsHS.Count,
                    _ => throw new System.NotImplementedException(_activeCollection.ToString()),
                };
            }
        }

        public bool AddUnique(T item)
        {
            switch (_activeCollection)
            {
                case ActiveCollection.Array:
                    var index = Array.FindIndex(_itemsArr, (x) => x.Equals(item));

                    if (index < 0)
                    {
                        _itemsArr[++_arrIndex] = item;
                        return true;
                    }
                    return false;
                case ActiveCollection.HashSet:
                    return _itemsHS.Add(item);
                default:
                    throw new System.NotImplementedException(_activeCollection.ToString());
            }
        }

        public bool Remove(T item)
        {
            switch (_activeCollection)
            {
                case ActiveCollection.Array:
                    var index = Array.FindIndex(_itemsArr, (x) => x.Equals(item));

                    if (index <= 0)
                    {
                        _itemsArr[index] = _arrIndex > index ? _itemsArr[_arrIndex] : default!;
                        --_arrIndex;
                        return true;
                    }

                    return false;
                case ActiveCollection.HashSet:
                    return _itemsHS.Remove(item);
                default:
                    throw new System.NotImplementedException(_activeCollection.ToString());
            }
        }


        public void Dispose()
        {
            if (_activeCollection == ActiveCollection.Array)
            {
                if (_refType)
                    Array.Clear(_itemsArr, 0, Count);

                System.Buffers.ArrayPool<T>.Shared.Return(_itemsArr, false);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _activeCollection switch
            {
                ActiveCollection.Array => new ArrayEnumeratorT(_itemsArr, 0, Count),
                ActiveCollection.HashSet => _itemsHS.GetEnumerator(),
                _ => throw new System.NotImplementedException(_activeCollection.ToString()),
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _activeCollection switch
            {
                ActiveCollection.Array => _itemsArr.GetEnumerator(),
                ActiveCollection.HashSet => _itemsHS.GetEnumerator(),
                _ => throw new System.NotImplementedException(_activeCollection.ToString()),
            };
        }
    }

}