using DevourDev.Base.Collections.Generic;
using DevourDev.Unity.Utils.SimpleStats;
using DevourDev.Unity.Utils.SimpleStats.Modifiers;
using Game.Core;
using System;
using UnityEngine;

namespace Game.StatsUtils
{
    public class ClampedStatData : IStatData
    {
        private class ClampedStatX : ModifiableStatData
        {
            public ClampedStatX(StatObject statObject) : base(statObject)
            {
            }
        }


        public event Action<IModifiableStatData, float, float> OnMinValueChanged;
        public event Action<IModifiableStatData, float, float> OnMaxValueChanged;
        public event Action<IModifiableStatData, float, float> OnValueChanged;

        private readonly StatObject _statObject;

        private readonly IModifiableStatData _min;
        private readonly IModifiableStatData _max;
        private readonly IModifiableStatData _cur;


        public StatObject StatObject => _statObject;


        public float MinValue => _min.Value;
        public float MaxValue => _max.Value;
        public float Value => _cur.Value;


        public void ChangeValue(float delta)
        {
            _cur.ChangeValue(delta);
        }
    }


    public class DelayedModifierChanger : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private bool _add;
        [SerializeField] private StatModifierCreator[] _modifiers;
        [SerializeField] private float _time;

        private bool _started;
        private float _left;


        private void Start()
        {
            if (_started)
                return;

            Init(_player, _add, _time, _modifiers);
        }


        public void Init(Player player, bool add, float time, params StatModifierCreator[] modifiers)
        {
            _started = true;
            _player = player;
            _add = add;
            _modifiers = modifiers;
            _left = _time = time;
        }


        private void Update()
        {
            if ((_left -= Time.deltaTime) <= 0)
            {
                ChangeModifiers();
                Destroy(gameObject);
            }
        }


        private void ChangeModifiers()
        {
            var stats = _player.StatsHolder.Stats;
            foreach (var m in _modifiers)
            {
                if (_add)
                    stats.AddModifier(m);
                else
                    stats.RemoveModifier(m);
            }

            if (_add)
                stats.FinishAddingModifiers();
            else
                stats.FinishRemovingModifiers();
        }
    }


}
