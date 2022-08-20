using System;
using System.Collections.Generic;
using UnityEngine;

namespace Externals.Utils.StatsSystem.Modifiers
{
    [System.Serializable]
    public sealed class FloatDynamicStatDataCreator
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private float _maxSource;
        [Tooltip("optional")]
        [SerializeField] private StatModifierCreator[] _initialModifiers;
        [SerializeField] private float _initialRatio;
        [SerializeField] private bool _saveRatio;
        [Tooltip("если включено - модификаторы не смогут вывести значение за рамки")]
        [SerializeField] private bool _clamp;
        [SerializeField] private Vector2 _clamping;


        public FloatDynamicStatDataCreator(StatObject statObject, float modifyingSourceValue, StatModifierCreator[] initialModifiers)
        {
            _statObject = statObject;
            _maxSource = modifyingSourceValue;
            _initialModifiers = initialModifiers;
        }


        public StatObject StatObject => _statObject;


        public FloatDynamicStatData Create()
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

            var sd = new FloatDynamicStatData(_statObject, _maxSource, modifiers, _initialRatio, _saveRatio);

            if (_clamp)
                sd.MaxBoundClamps = _clamping;

            return sd;
        }
    }


    public sealed class FloatDynamicStatData : ClampedFloatStatData, IModifiableStatData
    {
        private readonly FloatModifiableStatData _maxStat;


        public FloatDynamicStatData(StatObject statObject, float maxSource, IEnumerable<StatModifier> modifiers, float initialRatio, bool saveRatio, float minBoundsDelta = 1E-10F)
            : base(statObject, 0, minBoundsDelta, 0, false, saveRatio, minBoundsDelta)
        {
            if (maxSource <= 0 || float.IsInfinity(maxSource))
                throw new Exception("maxSource should be positive finite value");

            _maxStat = new(statObject, maxSource, modifiers);
            var max = _maxStat.Value;
            SetBounds(0, max, max * initialRatio);

            _maxStat.OnFloatValueChanged += HandleMaxStatChanged;
        }


        public StatModifiersCollection StatModifiers => _maxStat.StatModifiers;


        public Vector2? MaxBoundClamps { get => _maxStat.Clamps; set => _maxStat.Clamps = value; }


        private void HandleMaxStatChanged(IFloatValueCallback sender, float delta)
        {
            SetBounds(0, sender.Value);
        }


        public override string ToString()
        {
            return $"{GetType()}, min: {Min}, max: {Max}, Value: {Value}";
        }
    }
}
