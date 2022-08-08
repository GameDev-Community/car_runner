using Game.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Game.Core
{
    public class InitialStatsBalanceController : MonoBehaviour
    {
        [System.Serializable]
        private struct StatIniter
        {
            public StatObject StatObject;
            public ClampedValueCreator ClampedValueCreator;
        }


        [SerializeField, NonReorderable] private StatIniter[] _initialStats;


        private static InitialStatsBalanceController _inst;


        private void Awake()
        {
            _inst = this;
        }


        public static bool TryGetStatInitData(StatObject sobj, out IClampedValue data)
        {
            //no need to create dictionary from array for 1 usage

            var initer = Array.Find(_inst._initialStats, (x) => x.StatObject.Equals(sobj));

            if (initer.StatObject == null)
            {
                data = default!;
                return false;
            }

            data = initer.ClampedValueCreator.Create();
            return true;
        }


        public static Dictionary<StatObject, IClampedValue> GetInitialStats_Prototype()
        {
            var inst = _inst;
            var arr = inst._initialStats;
            var c = arr.Length;
            Dictionary<StatObject, IClampedValue> res = new(c);

            for (int i = -1; ++i < c;)
            {
                var v = arr[i];
                res.Add(v.StatObject, v.ClampedValueCreator.Create());
            }

            return res;
        }
    }
}
