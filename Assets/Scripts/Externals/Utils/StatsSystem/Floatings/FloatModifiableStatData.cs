﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Externals.Utils.StatsSystem.Modifiers
{
    public sealed class FloatModifiableStatData : IModifiableStatData, IValueCallback<float>
    {
        public event Action<IValueCallback<float>, float> OnValueChanged;

        private readonly StatObject _statObject;
        private readonly StatModifiersCollection _modifiersCollection;

        private readonly float _source;
        private float _value;
        private Vector2? _clamps;

        public FloatModifiableStatData(StatObject statObject, float modifyingSourceValue, IEnumerable<StatModifier> modifiers)
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

            _value = System.Math.Clamp(_modifiersCollection.ModifyValue(_source), float.MinValue, float.MaxValue);
            _modifiersCollection.OnModified += HandleModified;
        }


        public StatObject StatObject => _statObject;
        public float Value => _value;
        public float SourceValue => _source;
        public StatModifiersCollection StatModifiers => _modifiersCollection;

        public Vector2? Clamps
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
                        float delta = cv - _value;
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
            _value = modifiersCollection.ModifyValue(_source);

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
