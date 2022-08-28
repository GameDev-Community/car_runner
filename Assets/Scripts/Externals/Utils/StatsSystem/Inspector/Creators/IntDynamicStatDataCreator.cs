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
        [Tooltip("если включено - модификаторы не смогут вывести МАКСИМАЛЬНОЕ значение за рамки")]
        [SerializeField] private bool _clampMin;
        [SerializeField] private int _minClamp;
        [SerializeField] private bool _clampMax;
        [SerializeField] private int _maxClamp;


        public IntDynamicStatDataCreator(StatObject statObject, int maxSource,
            StatModifierCreator[] initialModifiers, float initialRatio,
            bool clampMin, int minClamp, bool clampMax, int maxClamp)
        {
            _statObject = statObject;
            _maxSource = maxSource;
            _initialModifiers = initialModifiers;
            _initialRatio = initialRatio;
            _clampMin = clampMin;
            _minClamp = minClamp;
            _clampMax = clampMax;
            _maxClamp = maxClamp;
        }


        public StatObject StatObject => _statObject;


        public IntDynamicStatData Create()
        {
            var sd = new IntDynamicStatData(_statObject, _maxSource, null, _initialRatio);
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
