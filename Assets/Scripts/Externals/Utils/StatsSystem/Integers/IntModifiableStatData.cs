using System;
using System.Collections.Generic;
using Utils;

namespace Externals.Utils.StatsSystem.Modifiers
{
    public sealed class IntModifiableStatData : IModifiableStatData, IValueCallback<int>, IClampedModifiable<int>
    {
        public event Action<IValueCallback<int>, int> OnValueChanged;

        private readonly StatObject _statObject;
        private readonly StatModifiersCollection _modifiersCollection;

        private readonly int _source;
        private int _value;
        private int? _minModVal;
        private int? _maxModVal;


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

        public int? MinModifiableValue
        {
            get => _minModVal;
            set => SetModifiableClamps(value, _maxModVal);
        }

        public int? MaxModifiableValue
        {
            get => _minModVal;
            set => SetModifiableClamps(value, _maxModVal);
        }


        public void SetModifiableClamps(int? min, int? max)
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


            int minR = min ?? int.MinValue;
            int maxR = max ?? int.MaxValue;

            var cv = System.Math.Clamp(_value, minR, maxR);

            if (cv == _value)
                return;

            var delta = cv - _value;
            _value = cv;
            OnValueChanged?.Invoke(this, delta);
        }


        private void HandleModified(StatModifiersCollection modifiersCollection)
        {
            var v = MathModule.ClampLongToInt((long)_modifiersCollection.ModifyValue(_source));

            if (_minModVal.HasValue || _maxModVal.HasValue)
            {
                //todo: optimize to 1 constraint (if x > y -> x = y)

                int min = _minModVal ?? int.MinValue;
                int max = _maxModVal ?? int.MaxValue;

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
