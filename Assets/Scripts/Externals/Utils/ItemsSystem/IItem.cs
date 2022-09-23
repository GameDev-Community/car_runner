using Externals.Utils.StatsSystem;
using Externals.Utils.Valuables;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Externals.Utils.Items
{
    public interface IItem
    {
        public ItemType ItemType { get; }
    }

    public interface IShopItem
    {
        public int Cost { get; }
    }


    [System.Serializable]
    public class ShopItem
    {
        //Externals.Utils.StatsSystem.StatObject
    }

    public interface ICondition<T>
    {
        public bool CheckCondition(T context);
    }


    [System.Serializable]
    public class SpendStatsCondition : ICondition<StatsCollection>
    {
        private struct R
        {
            private readonly bool _isInteger;
            private readonly IAmountManipulatable<int> _amMan;
            private readonly IAmountManipulatable<float> _amManF;
            private readonly int _val;
            private readonly float _valF;


            public R(IAmountManipulatable<int> amMan, int intValue)
            {
                _isInteger = true;
                _amMan = amMan;
                _amManF = default;
                _val = intValue;
                _valF = default;
                //_amount = &intValue;
            }

            public R(IAmountManipulatable<float> amManF, float floatValue)
            {
                _isInteger = false;
                _amMan = default;
                _amManF = amManF;
                _val = default;
                _valF = floatValue;
            }


            public void Execute()
            {
                if (_isInteger)
                {
                    _amMan.Set(_val);
                    return;
                }

                _amManF.Set(_valF);
            }

        }
        //[SerializeField] StatsAmount.StatsAmountCreator _creator;

        [SerializeField] private StatsAmount _cost;



        /// <returns>True, если StatsCollection содержит
        /// все статы из _cost в требуемом количестве.
        /// При успехе, эти значения убираются.</returns>
        public bool CheckCondition(StatsCollection sc)
        {
            var ams = _cost.Amounts;


            var pool = System.Buffers.ArrayPool<R>.Shared;

            var c = ams.Count;
            var rs = pool.Rent(c);

            try
            {
                int i = 0;
                foreach (KeyValuePair<StatObject, StatsAmount.StatAmount> sa in ams)
                {
                    var statObj = sa.Key;
                    if (!sc.TryGetStatData(statObj, out var sdRaw))
                        return false;

                    var info = statObj.GetStatDataInfo();

                    if (info.NumericsType == StatObject.NumericsType.Integer)
                    {
                        if (sdRaw is not IAmountManipulatable<int> intAmMan)
                            throw new System.InvalidCastException($"{sdRaw.GetType().FullName} ({nameof(IAmountManipulatable<int>)} expected)");

                        if (!intAmMan.CanRemove(sa.Value.Amount, out var manRes))
                            return false;

                        rs[i] = new(amMan: intAmMan, intValue: manRes);
                    }
                    else
                    {
                        if (sdRaw is not IAmountManipulatable<float> floatAmMan)
                            throw new System.InvalidCastException($"{sdRaw.GetType().FullName} ({nameof(IAmountManipulatable<float>)} expected)");

                        if (!floatAmMan.CanRemove(sa.Value.AmountF, out var manRes))
                            return false;

                        rs[i] = new(amManF: floatAmMan, floatValue: manRes);
                    }

                    ++i;
                }

                //applying removing
                foreach (var r in rs)
                {
                    r.Execute();
                }

                return true;
            }
            finally
            {
                pool.Return(rs, false);
            }
        }
    }


    [System.Serializable]
    public class StatsAmount
    {
        [System.Serializable]
        public class StatsAmountCreator
        {
            [SerializeField] private StatAmount[] _statAmounts;


            public StatsAmountCreator(StatAmount[] statAmounts)
            {
                _statAmounts = statAmounts;
            }


            public StatsAmount Create()
            {
                return Sum(_statAmounts);
            }
        }


        [SerializeField] private Dictionary<StatObject, StatAmount> _amounts;


        public Dictionary<StatObject, StatAmount> Amounts => _amounts;


        public static StatsAmount Sum(params StatAmount[] statAmounts)
        {
            var res = new StatsAmount();
            Dictionary<StatObject, StatAmount> dic = new();
            res._amounts = dic;

            foreach (var item in statAmounts)
            {
                var sobj = item.StatObject;
                if (dic.TryGetValue(sobj, out var sa))
                {
                    if (sobj.GetStatDataInfo().NumericsType == StatObject.NumericsType.Integer)
                        sa.Amount += item.Amount;
                    else
                        sa.AmountF += item.AmountF;
                }
                else
                {
                    dic.Add(sobj, item);
                }
            }

            return res;
        }

        public static StatsAmount Max(params StatAmount[] statAmounts)
        {
            var res = new StatsAmount();
            Dictionary<StatObject, StatAmount> dic = new();
            res._amounts = dic;

            foreach (var item in statAmounts)
            {
                var sobj = item.StatObject;
                if (dic.TryGetValue(sobj, out var sa))
                {
                    if (sobj.GetStatDataInfo().NumericsType == StatObject.NumericsType.Integer)
                    {
                        if (sa.Amount < item.Amount)
                            sa.Amount += item.Amount;
                    }
                    else
                    {
                        if (sa.AmountF < item.AmountF)
                            sa.AmountF += item.AmountF;
                    }
                }
                else
                {
                    dic.Add(sobj, item);
                }
            }

            return res;
        }


        [System.Serializable]
        public class StatAmount
        {
            [SerializeField] private StatObject _statObject;
            [SerializeField] private int _amount;
            [SerializeField] private float _amountF;


            public StatObject StatObject => _statObject;


            public int Amount { get => _amount; set => _amount = value; }
            public float AmountF { get => _amountF; set => _amountF = value; }


            public static StatAmount Sum(params StatAmount[] items)
            {
                StatObject.StatDataInfo info = GetStandardInfo(items);

                if (info.NumericsType == StatObject.NumericsType.Integer)
                {
                    return IntegerSum(ref items);
                }
                else
                {
                    return FloatSum(ref items);
                }
            }

            public static StatAmount Max(params StatAmount[] items)
            {
                StatObject.StatDataInfo info = GetStandardInfo(items);

                if (info.NumericsType == StatObject.NumericsType.Integer)
                {
                    return IntegerMax(ref items);
                }
                else
                {
                    return FloatMax(ref items);
                }
            }


            private static StatAmount FloatSum(ref StatAmount[] items)
            {
                float sum = 0;

                foreach (var item in items)
                {
                    sum += item.AmountF;
                }

                return new StatAmount()
                {
                    _statObject = items[0]._statObject,
                    _amountF = sum,
                };
            }

            private static StatAmount IntegerSum(ref StatAmount[] items)
            {
                int sum = 0;

                foreach (var item in items)
                {
                    sum += item.Amount;
                }

                return new StatAmount()
                {
                    _statObject = items[0]._statObject,
                    _amount = sum,
                };
            }


            private static StatAmount IntegerMax(ref StatAmount[] items)
            {
                var maxI = items[0];
                int maxV = maxI._amount;

                var c = items.Length;

                for (int i = 1; i < c; i++)
                {
                    var it = items[i];
                    var itV = it._amount;

                    if (maxV < itV)
                    {
                        maxI = it;
                        maxV = itV;
                    }
                }

                return maxI;
            }

            private static StatAmount FloatMax(ref StatAmount[] items)
            {
                var maxI = items[0];
                float maxV = maxI._amountF;

                var c = items.Length;

                for (int i = 1; i < c; i++)
                {
                    var it = items[i];
                    var itV = it._amountF;

                    if (maxV < itV)
                    {
                        maxI = it;
                        maxV = itV;
                    }
                }

                return maxI;
            }


            private static StatObject.StatDataInfo GetStandardInfo(StatAmount[] items)
            {
                if (items == null || items.Length == 0)
                    throw new System.ArgumentNullException(nameof(items));

                var standard = items[0];
                var info = standard._statObject.GetStatDataInfo();
                return info;
            }
        }
    }
}