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
        [Tooltip("если включено - модификаторы не смогут вывести МАКСИМАЛЬНОЕ значение за рамки")]
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
}
