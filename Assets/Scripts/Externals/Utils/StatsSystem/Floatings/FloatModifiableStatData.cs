using System;
using System.Collections.Generic;
using UnityEngine;

namespace Externals.Utils.StatsSystem.Modifiers
{
    public sealed class FloatModifiableStatData : IModifiableStatData, IValueCallback<float>, IClampedModifiable<float>
    {
        public event Action<IValueCallback<float>, float> OnValueChanged;

        private readonly StatObject _statObject;
        private readonly StatModifiersCollection _modifiersCollection;

        private readonly float _source;
        private float _value;
        private float? _minModVal;
        private float? _maxModVal;


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


        public float? MinModifiableValue
        {
            get => _minModVal;
            set => SetModifiableClamps(value, _maxModVal);
        }

        public float? MaxModifiableValue
        {
            get => _minModVal;
            set => SetModifiableClamps(value, _maxModVal);
        }


        public void SetModifiableClamps(float? min, float? max)
        {
            //todo: add min > max checks (with nulls)
            bool flag = false;

            if (_minModVal != min)
                _minModVal = min;
            else
                flag = true;

            if (_maxModVal != max)
                _maxModVal = max;
            else if (flag)
                return;


            float minR = min ?? float.MinValue;
            float maxR = max ?? float.MaxValue;

            float cv = System.Math.Clamp(_value, minR, maxR);

            if (cv == _value)
                return;

            var delta = cv - _value;
            _value = cv;
            OnValueChanged?.Invoke(this, delta);
        }

        private void HandleModified(StatModifiersCollection modifiersCollection)
        {
            var v = modifiersCollection.ModifyValue(_source);

            if (_minModVal.HasValue || _maxModVal.HasValue)
            {
                //todo: optimize to 1 constraint (if x > y -> x = y)

                float min = _minModVal ?? float.MinValue;
                float max = _maxModVal ?? float.MaxValue;

                v = System.Math.Clamp(v, min, max);
            }

            if (v != _value)
            {
                var delta = v - _value;
                _value = v;
                OnValueChanged?.Invoke(this, delta);
            }
        }
    }
}
