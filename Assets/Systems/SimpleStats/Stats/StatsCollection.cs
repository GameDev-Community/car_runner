using DevourDev.Base.Collections.Generic;
using DevourDev.Base.Numerics;
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

        public event System.Action<StatsCollection, IModifiableStatData> OnStatObjectAdded;
        public event System.Action<StatsCollection, IModifiableStatData> OnStatObjectRemoved;

        /// <summary>
        /// sender, stat data, dirty delta, safe delta
        /// </summary>
        public event System.Action<StatsCollection, IModifiableStatData, float, float> OnStatValueChanged;

        private readonly Dictionary<StatObject, IModifiableStatData> _stats;
        private readonly Dictionary<StatObject, CountingDictionary<StatModifier>> _unappliedModifiers;

        private readonly HashSet<StatObject> _addQueue;
        private readonly HashSet<StatObject> _removeQueue;


        public StatsCollection()
        {
            _stats = new();
            _unappliedModifiers = new();

            _addQueue = new();
            _removeQueue = new();
        }


        public bool TryAddStatObject(StatObject statObject, IModifiableStatData statData)
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


        public bool ContainsStat(StatObject statObject)
        {
            return _stats.ContainsKey(statObject);
        }

        public bool TryGetStatData(StatObject statObject, out IModifiableStatData statData)
        {
            return _stats.TryGetValue(statObject, out statData);
        }

        /// <summary>
        /// Requires FinsihAdding
        /// </summary>
        public void AddModifier(Modifiers.StatModifierCreator mc)
        {
            //just a public helper

            AddModifier(mc.StatObject, mc.Create(), mc.Amount);
        }

        /// <summary>
        /// Requires FinsihAdding
        /// </summary>
        public void AddModifier(Modifiers.StatModifierCreator mc, int amount)
        {
            //just a public helper

            AddModifier(mc.StatObject, mc.Create(), amount);
        }

        /// <summary>
        /// Requires FinsihAdding
        /// </summary>
        public void AddModifier(StatObject statObject, Modifiers.StatModifier m, int amount)
        {
            if (!_stats.TryGetValue(statObject, out var statData))
            {
                AddUnappliedModifier(statObject, m, amount);
                return;
            }

            _addQueue.Add(statObject);
            statData.AddModifier(m, amount);
        }


        //public void RemoveModifier(StatObject statObject, Predicate<StatModifier> predicate, int amount)
        //{
        //     eyrtshdfg dfeg
        //}

        /// <summary>
        /// Requires FinsihAdding
        /// </summary>
        public void RemoveModifier(Modifiers.StatModifierCreator mc)
        {
            //just a public helper

            RemoveModifier(mc.StatObject, mc.Create(), mc.Amount);
        }

        /// <summary>
        /// Requires FinsihAdding
        /// </summary>
        public void RemoveModifier(Modifiers.StatModifierCreator mc, int amount)
        {
            //just a public helper

            RemoveModifier(mc.StatObject, mc.Create(), amount);
        }

        /// <summary>
        /// Requires FinsihAdding
        /// Removing all requested or all existing if less or nothing if stat object not exists
        /// </summary>
        public void RemoveModifier(StatObject statObject, StatModifier m, int amount)
        {
            if (!_stats.TryGetValue(statObject, out var statData))
            {
                RemoveUnappliedModifier(statObject, m, amount, false);
                return;
            }

            _removeQueue.Add(statObject);
            statData.RemoveModifier(m, amount);
        }

        public void FinishAddingModifiers()
        {
            foreach (var item in _addQueue)
            {
                _stats[item].FinishAddingModifiers();
            }

            _addQueue.Clear();
        }

        public void FinishRemovingModifiers()
        {
            foreach (var item in _removeQueue)
            {
                _stats[item].FinishRemovingModifiers();
            }

            _removeQueue.Clear();
        }

        /// <summary>
        /// no need to call finish after this method
        /// </summary>
        /// <returns>true if StatObject and  modifier exists</returns>
        //public bool TryRemoveModifier(StatObject statObject, Predicate<StatModifier> predicate, int amount)
        //{
        //    if (!_stats.TryGetValue(statObject, out var statData))
        //        return TryRemoveUnappliedModifier(statObject, predicate, amount);
        //}


        /// <summary>
        /// no need to call finish after this method
        /// </summary>
        /// <returns>true if StatObject and  modifier exists and
        /// have atl <paramref name="amount"/> of amount</returns>
        public bool TryRemoveModifier(Modifiers.StatModifierCreator mc)
        {
            //just a public helper

            return TryRemoveModifier(mc.StatObject, mc.Create(), mc.Amount);
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
                return RemoveUnappliedModifier(statObject, m, amount, true) == amount;

            return statData.TryRemoveModifier(m, amount);
        }


        private void AddStatObject(StatObject statObject, IModifiableStatData statData)
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

        private void RemoveStatObject(StatObject statObject, IModifiableStatData statData)
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

            var added = umcd.TryAddAmount(m, amount);

#if DEVOUR_DEBUG || UNITY_EDITOR
            if (added)
                throw new Exception($"unable to add {statObject}, {m}, {amount}");
#endif
        }

        //private bool TryRemoveUnappliedModifier(StatObject statObject, Predicate<StatModifier> predicate, int amount, bool allOrNothing)
        //{
        //    if (!_unappliedModifiers.TryGetValue(statObject, out var umcd))
        //        return false;

        //    return umcd.TryRemove(predicate, amount, allOrNothing);
        //}

        private int RemoveUnappliedModifier(StatObject statObject, StatModifier m, int amount, bool allOrNothing)
        {
            if (!_unappliedModifiers.TryGetValue(statObject, out var umcd))
                return -1;

            return umcd.RemoveAmount(m, amount, allOrNothing);
        }


        #region subscribtions
        private void SubscribeToStatData(IModifiableStatData statData)
        {
            statData.OnValueChanged += HandleStatValueChanged;
        }

        private void UnsubscribeFromStatData(IModifiableStatData statData)
        {
            statData.OnValueChanged -= HandleStatValueChanged;
        }


        private void HandleStatValueChanged(IModifiableStatData sender, float dirtyDelta, float safeDelta)
        {
            OnStatValueChanged?.Invoke(this, sender, dirtyDelta, safeDelta);
        }

        #endregion
    }
}