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
}
