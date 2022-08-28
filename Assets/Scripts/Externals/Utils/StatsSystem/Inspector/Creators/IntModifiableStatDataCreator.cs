using UnityEngine;

namespace Externals.Utils.StatsSystem.Modifiers
{
    [System.Serializable]
    public sealed class IntModifiableStatDataCreator
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private int _modifyingSourceValue;
        [Tooltip("optional")]
        [SerializeField] private StatModifierCreator[] _initialModifiers;
        [Tooltip("если включено - модификаторы не смогут вывести значение за рамки")]
        [SerializeField] private bool _clampMin;
        [SerializeField] private int _minClamp;
        [SerializeField] private bool _clampMax;
        [SerializeField] private int _maxClamp;


        public IntModifiableStatDataCreator(StatObject statObject, int modifyingSourceValue, 
            StatModifierCreator[] initialModifiers, bool clampMin, int minClamp, bool clampMax, int maxClamp)
        {
            _statObject = statObject;
            _modifyingSourceValue = modifyingSourceValue;
            _initialModifiers = initialModifiers;
            _clampMin = clampMin;
            _minClamp = minClamp;
            _clampMax = clampMax;
            _maxClamp = maxClamp;
        }


        public StatObject StatObject => _statObject;


        public IntModifiableStatData Create()
        {
            var sd = new IntModifiableStatData(_statObject, _modifyingSourceValue, null);
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
