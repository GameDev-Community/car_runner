﻿using UnityEngine;

namespace Externals.Utils.StatsSystem
{
    [System.Serializable]
    public class DynamicStatValueChanger<T>
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private T _value;
        [Tooltip("stat value will be set to _value if checked," +
            "otherwise, _value is delta (can be negative)")]
        [SerializeField] private bool _setValue;


        public DynamicStatValueChanger(StatObject statObject, T delta)
        {
            _statObject = statObject;
            _value = delta;
        }


        public StatObject StatObject => _statObject;
        public T Delta => _value;


        public void Apply(StatsCollection statsCollection)
        {
            if (statsCollection.TryGetStatData(_statObject, out var data))
            {
                if (data is IClampedAmountManipulatable<T> clampedManipulatable)
                {
                    if (_setValue)
                        _ = clampedManipulatable.SetSafe(_value);
                    else
                        _ = clampedManipulatable.ChangeSafe(_value);
                }
            }
        }
    }
}
