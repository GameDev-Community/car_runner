using System.Collections.Generic;
using Utils.Attributes;

namespace Externals.Utils.StatsSystem
{


    public class StatsCollection
    {
        public event System.Action<StatsCollection, IStatData> OnStatAdded;
        public event System.Action<StatsCollection, IStatData> OnStatRemoved;

        private readonly Dictionary<StatObject, IStatData> _stats;


        public StatsCollection()
        {
            _stats = new();
        }


        public bool ContainsStat(StatObject stat)
        {
            return _stats.ContainsKey(stat);
        }

        public void AddStat(StatObject stat, IStatData data)
        {
            _stats.Add(stat, data);
            OnStatAdded?.Invoke(this, data);
        }

        public bool TryAddStat(StatObject stat, IStatData data)
        {
            if (_stats.TryAdd(stat, data))
            {
                OnStatAdded?.Invoke(this, data);
                return true;
            }

            return false;
        }

        public bool TryAddStat(StatDataInitializer creator)
        {
            if (!ContainsStat(creator.StatObject))
            {
                AddStat(creator.StatObject, creator.Create());
                return true;
            }

            return false;
        }

        public bool TryRemoveStat(StatObject stat, out IStatData data)
        {
            if (_stats.Remove(stat, out data))
            {
                OnStatRemoved?.Invoke(this, data);
                return true;
            }

            return false;
        }




        public bool TryGetStatData(StatObject stat, out IStatData data)
        {
            return _stats.TryGetValue(stat, out data);
        }

        /// <summary>
        /// юнити (мб старый дотнет) плохо работает с методами одного названия
        /// с единственным различием сигнатуры в генерике одного из
        /// них и когда тип параметра нон-генерика подходит под
        /// ограничение генерика (IStatData is IStatData), поэтому
        /// я добавил T в название для явного отличия.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stat"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool TryGetStatDataT<T>(StatObject stat, out T data) where T : IStatData
        {
            if (TryGetStatData(stat, out var rawData))
            {
                if (rawData is T targetData)
                {
                    data = targetData;
                    return true;
                }
            }

            data = default;
            return false;
        }




        public (StatObject stat, IStatData data)[] GetStatsArray()
        {
            var d = _stats;
            var c = d.Count;
            (StatObject stat, IStatData data)[] arr = new (StatObject stat, IStatData data)[c];
            int i = -1;

            foreach (var kvp in d)
                arr[++i] = (kvp.Key, kvp.Value);

            return arr;
        }

        public Dictionary<StatObject, IStatData>.Enumerator GetEnumerator()
        {
            return _stats.GetEnumerator();
        }
    }
}
