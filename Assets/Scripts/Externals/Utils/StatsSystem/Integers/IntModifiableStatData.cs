using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Externals.Utils.StatsSystem.Modifiers
{
    public sealed class IntModifiableStatData : IModifiableStatData, IValueCallback<int>
    {
        public event Action<IValueCallback<int>, int> OnValueChanged;

        private readonly StatObject _statObject;
        private readonly StatModifiersCollection _modifiersCollection;

        private readonly int _source;
        private int _value;
        private Vector2Int? _clamps;

        public IntModifiableStatData(StatObject statObject, int modifyingSourceValue, IEnumerable<StatModifier> modifiers)
        {
            _statObject = statObject;
            _source = modifyingSourceValue;
            _modifiersCollection = new();

            if (modifiers != null)
            {
                foreach (var m in modifiers)
                {
                    _modifiersCollection.AddModifier(m, 1);
                }

                _modifiersCollection.FinishAddingModifiers();
            }

            _value = MathModule.ClampLongToInt((long)_modifiersCollection.ModifyValue(_source));
            _modifiersCollection.OnModified += HandleModified;
        }


        public StatObject StatObject => _statObject;
        public int Value => _value;
        public int SourceValue => _source;
        public StatModifiersCollection StatModifiers => _modifiersCollection;

        public Vector2Int? Clamps
        {
            get => _clamps;
            set
            {
                _clamps = value;

                if (_clamps.HasValue)
                {
                    var clamps = _clamps.Value;
                    var cv = System.Math.Clamp(_value, clamps.x, clamps.y);

                    if (cv != _value)
                    {
                        int delta = cv - _value;
                        _value = cv;
                        OnValueChanged?.Invoke(this, delta);
                    }
                }
                else
                {
                    HandleModified(_modifiersCollection);
                }
            }
        }


        private void HandleModified(StatModifiersCollection modifiersCollection)
        {
            var tmp = _value;
            _value = MathModule.ClampLongToInt((long)_modifiersCollection.ModifyValue(_source));

            if (Clamps.HasValue)
            {
                var clamps = Clamps.Value;
                _value = System.Math.Clamp(_value, clamps.x, clamps.y);
            }

            if (tmp != _value)
                OnValueChanged?.Invoke(this, _value - tmp);
        }
    }
}
