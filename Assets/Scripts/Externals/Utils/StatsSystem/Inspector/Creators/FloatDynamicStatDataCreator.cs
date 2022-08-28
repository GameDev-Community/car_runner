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
        [SerializeField] private bool _clampMin;
        [SerializeField] private float _minClamp;
        [SerializeField] private bool _clampMax;
        [SerializeField] private float _maxClamp;

        public FloatDynamicStatDataCreator(StatObject statObject, float maxSource,
            StatModifierCreator[] initialModifiers, float initialRatio, bool saveRatio,
            bool clampMin, float minClamp, bool clampMax, float maxClamp)
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
            var sd = new FloatDynamicStatData(_statObject, _maxSource, null, _initialRatio, _saveRatio);
            var ims = _initialModifiers;

            if (ims != null)
            {
                int imsC = ims.Length;
                if (imsC > 0)
                {
                    var sms = sd.StatModifiers;
                    for (int i = -1; ++i < imsC;)
                    {
                        var mc = ims[i];
                        sms.AddModifier(mc.Create(), mc.Amount);
                    }
                    sms.FinishAddingModifiers();
                }
            }


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
