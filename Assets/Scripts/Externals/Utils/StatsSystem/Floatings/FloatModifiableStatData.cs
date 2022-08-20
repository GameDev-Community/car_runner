using System;
using System.Collections.Generic;
using UnityEngine;

namespace Externals.Utils.StatsSystem.Modifiers
{
    [System.Serializable]
    public sealed class FloatModifiableStatDataCreator
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private float _modifyingSourceValue;
        [Tooltip("optional")]
        [SerializeField] private StatModifierCreator[] _initialModifiers;
        [Tooltip("если включено - модификаторы не смогут вывести значение за рамки")]
        [SerializeField] private bool _clamp;
        [SerializeField] private Vector2 _clamping;


        public FloatModifiableStatDataCreator(StatObject statObject, float modifyingSourceValue, StatModifierCreator[] initialModifiers)
        {
            _statObject = statObject;
            _modifyingSourceValue = modifyingSourceValue;
            _initialModifiers = initialModifiers;
        }


        public StatObject StatObject => _statObject;


        public FloatModifiableStatData Create()
        {
            StatModifier[] modifiers = null;

            var ims = _initialModifiers;
            if (ims != null)
            {
                int imsC = ims.Length;
                if (imsC > 0)
                {
                    modifiers = new StatModifier[imsC];

                    for (int i = -1; ++i < imsC;)
                        modifiers[i] = ims[i].Create();
                }
            }

            var sd = new FloatModifiableStatData(_statObject, _modifyingSourceValue, modifiers);

            if (_clamp)
                sd.Clamps = _clamping;

            return sd;
        }
    }

    public sealed class FloatModifiableStatData : IModifiableStatData, IFloatValueCallback
    {
        public event Action<IFloatValueCallback, float> OnFloatValueChanged;

        private readonly StatObject _statObject;
        private readonly StatModifiersCollection _modifiersCollection;

        private readonly float _source;
        private float _value;

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

            _value = _modifiersCollection.ModifyValue(_source);
            _modifiersCollection.OnModified += HandleModified;
        }


        public StatObject StatObject => _statObject;
        public float Value => _value;
        public float SourceValue => _source;
        public StatModifiersCollection StatModifiers => _modifiersCollection;

        public Vector2? Clamps { get; set; }


        private void HandleModified(StatModifiersCollection statsCollection)
        {
            var tmp = _value;
            _value = statsCollection.ModifyValue(_source);

            if (Clamps.HasValue)
            {
                var clamps = Clamps.Value;
                _value = System.Math.Clamp(_value, clamps.x, clamps.y);
            }

            OnFloatValueChanged?.Invoke(this, _value - tmp);
        }
    }
}
