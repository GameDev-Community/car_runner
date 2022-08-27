using UnityEngine;

namespace Externals.Utils.StatsSystem.Modifiers
{
    [System.Serializable]
    public sealed class IntDynamicStatDataCreator
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private int _maxSource;
        [Tooltip("optional")]
        [SerializeField] private StatModifierCreator[] _initialModifiers;
        [SerializeField] private float _initialRatio;
        [SerializeField] private bool _saveRatio;
        [Tooltip("если включено - модификаторы не смогут вывести МАКСИМАЛЬНОЕ значение за рамки")]
        [SerializeField] private bool _clampMin;
        [SerializeField] private int _minClamp;
        [SerializeField] private bool _clampMax;
        [SerializeField] private int _maxClamp;


        public IntDynamicStatDataCreator(StatObject statObject, int maxSource,
            StatModifierCreator[] initialModifiers, float initialRatio, bool saveRatio,
            bool clampMin, int minClamp, bool clampMax, int maxClamp)
        {
            _statObject = statObject;
            _maxSource = maxSource;
            _initialModifiers = initialModifiers;
            _initialRatio = initialRatio;
            _saveRatio = saveRatio;
            _clampMin = clampMin;
            _minClamp = minClamp;
            _clampMax = clampMax;
            _maxClamp = maxClamp;
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

            if (_clampMin && _clampMax)
            {
                sd.SetModifiableClamps(_minClamp, _maxClamp);
            }
            else
            {
                if (_clampMin)
                    sd.MinModifiableValue = _minClamp;
                else if (_clampMax)
                    sd.MaxModifiableValue = _maxClamp;
            }

            return sd;
        }
    }
}
