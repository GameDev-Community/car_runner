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
        [SerializeField] private bool _clampMin;
        [SerializeField] private float _minClamp;
        [SerializeField] private bool _clampMax;
        [SerializeField] private float _maxClamp;


        public FloatModifiableStatDataCreator(StatObject statObject, float modifyingSourceValue,
            StatModifierCreator[] initialModifiers, bool clampMin, float minClamp,
            bool clampMax, float maxClamp)
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
