using DevourDev.Unity.Utils.SimpleStats.Modifiers;
using System;
using System.Collections.Generic;

namespace DevourDev.Unity.Utils.SimpleStats
{
    /// <summary>
    /// Данный класс представляет собой коллекцию Статов.
    /// Существует интерфейс IStatsHolder, который требует
    /// ссылку на StatsCollection для реализации. Рекомендуется
    /// выделение как минимум 1 колекции на 1 сущность (даже в
    /// случае, когда 2 сущности пользуются разными объектами
    /// Статов и не имеют коллизий друг с другом).
    /// </summary>
    public class StatsCollection
    {
        //При добавлении/удалении Стат Объекта, в аргументы передаётся данный
        //экземпляр StatsCollection в качестве отправителя и StatData, содержащая
        //в себе StatObject, поэтому StatObject отдельно не передаётся.

        public event System.Action<StatsCollection, IStatData> OnStatObjectAdded;
        public event System.Action<StatsCollection, IStatData> OnStatObjectRemoved;

        /// <summary>
        /// sender, stat data, dirty delta, safe delta
        /// </summary>
        public event System.Action<StatsCollection, IStatData, float, float> OnStatValueChanged;

        private readonly Dictionary<StatObject, IStatData> _stats;
        private readonly Dictionary<StatObject, CountingDictionary<StatModifier>> _unappliedModifiers;

        private readonly Dictionary<StatObject, CountingDictionary<StatModifier>> _addQueue;
        private readonly Dictionary<StatObject, CountingDictionary<StatModifier>> _removeQueue;


        public StatsCollection()
        {
            _stats = new();
            _unappliedModifiers = new();

            _addQueue = new();
            _removeQueue = new();
        }


        public bool TryAddStatObject(StatObject statObject, IStatData statData)
        {
            if (!_stats.TryAdd(statObject, statData))
                return false;

            AddStatObject(statObject, statData);
            return true;
        }

        public bool TryRemoveStatObject(StatObject statObject)
        {
            if (!_stats.TryGetValue(statObject, out var statData))
                return false;

            RemoveStatObject(statObject, statData);
            return true;
        }


        public void AddModifier(Modifiers.StatModifierCreator mc, int amount)
        {
            //just a public helper

            AddModifier(mc.StatObject, mc.Create(), amount);
        }

        public void AddModifier(StatObject statObject, Modifiers.StatModifier m, int amount)
        {
            if (!_stats.TryGetValue(statObject, out var statData))
                AddUnappliedModifier(statObject, m, amount);

            statData.AddModifier(m, amount);
        }


        /// <summary>
        /// no need to call finish after this method
        /// </summary>
        /// <returns>true if StatObject and  modifier exists and
        /// have atl <paramref name="amount"/> of amount</returns>
        public bool TryRemoveModifier(Modifiers.StatModifierCreator mc, int amount)
        {
            //just a public helper

            return TryRemoveModifier(mc.StatObject, mc.Create(), amount);
        }

        /// <summary>
        /// no need to call finish after this method
        /// </summary>
        /// <returns>true if <paramref name="statObject"/> and
        /// modifier exists and have atl <paramref name="amount"/>
        /// of amount</returns>
        public bool TryRemoveModifier(StatObject statObject, Modifiers.StatModifier m, int amount)
        {
            if (!_stats.TryGetValue(statObject, out var statData))
                return TryRemoveUnappliedModifier(statObject, m, amount);

            return statData.TryRemoveModifier(m, amount);
        }


        private void AddStatObject(StatObject statObject, IStatData statData)
        {
            if (_unappliedModifiers.TryGetValue(statObject, out var umcd))
            {
                foreach (KeyValuePair<StatModifier, RefInt> item in umcd)
                {
                    statData.AddModifier(item.Key, item.Value);
                }

                statData.FinishAddingModifiers();
                _unappliedModifiers.Remove(statObject);
            }

            //Подписываемся после инициализации, т.к. реагировать на изменения
            //некому. Слушатели OnStatObjectAdded могут получить нужные им
            //значения через аргумент statData события.
            //Если декоратор желает получать события с целью структуризации
            //(вывести на экран поп-апы с изменениями значения Стата), то он
            //должен получить новую коллекцию модификаторов (GetModifiers()),
            //либо ссылку на используемую (GetMdoifiersDictionary(). 

            SubscribeToStatData(statData);
            OnStatObjectAdded?.Invoke(this, statData);
        }

        private void RemoveStatObject(StatObject statObject, IStatData statData)
        {
            //Отписываемся ДО деинициализации, так как слушателям OnStatValueChanged
            //нет смысла реагировать на возможные изменения Стата, который исчезнет
            //в этом же кадре. Слушатели OnStatObjectRemoved получат всю необходимую
            //им информацию, включая бывшую СтатДату (например, чтобы они могли
            //отписаться от событий в том числе пользовательских реализаций IStatData.

            UnsubscribeFromStatData(statData);

            _unappliedModifiers.Add(statObject, statData.GetModifiersDictionary());
            _stats.Remove(statObject);
            OnStatObjectRemoved?.Invoke(this, statData);
        }


        private void AddUnappliedModifier(StatObject statObject, StatModifier m, int amount)
        {
            if (!_unappliedModifiers.TryGetValue(statObject, out var umcd))
            {
                umcd = new CountingDictionary<StatModifier>(false);
            }

            var added = umcd.TryAdd(m, amount);

#if DEVOUR_DEBUG || UNITY_EDITOR
            if (added)
                throw new Exception($"unable to add {statObject}, {m}, {amount}");
#endif
        }

        private bool TryRemoveUnappliedModifier(StatObject statObject, StatModifier m, int amount)
        {
            if (!_unappliedModifiers.TryGetValue(statObject, out var umcd))
                return false;

            return umcd.TryRemove(m, amount);
        }


        #region subscribtions
        private void SubscribeToStatData(IStatData statData)
        {
            statData.OnValueChanged += HandleStatValueChanged;
        }

        private void UnsubscribeFromStatData(IStatData statData)
        {
            statData.OnValueChanged -= HandleStatValueChanged;
        }


        private void HandleStatValueChanged(IStatData sender, float dirtyDelta, float safeDelta)
        {
            OnStatValueChanged?.Invoke(this, sender, dirtyDelta, safeDelta);
        }

        #endregion
    }
}