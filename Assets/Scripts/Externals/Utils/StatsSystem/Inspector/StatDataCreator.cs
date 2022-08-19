using UnityEngine;
using Externals.Utils.StatsSystem.Modifiers;

namespace Externals.Utils.StatsSystem
{
    [System.Serializable]
    public class StatDataCreator
    {
        public enum StatValueType
        {
            Float,
            Integer
        }

        [SerializeField] private StatObject _statObject;
        [SerializeField] private StatValueType _valueType;
        [SerializeField] private float _initial;
        [Space]
        [SerializeField] private bool _clamped;
        [SerializeField] private float _min;
        [SerializeField] private float _max;
        [SerializeField] private bool _initialIsRatio;
        [SerializeField] private bool _saveRatio;
        [SerializeField] private bool _modifiable;
        [SerializeField] private StatModifierCreator[] _modifiers;
        [SerializeField] private float _maxSource;


        public StatObject StatObject => _statObject;


        public IStatData Create()
        {
            IStatData data;

            switch (_valueType)
            {
                case StatValueType.Float:
                    if (_clamped)
                    {
                        if (_modifiable)
                        {
                            StatModifier[] modifiers = null;
                            if (_modifiers != null)
                            {
                                modifiers = new StatModifier[_modifiers.Length];

                                for (int i = 0; i < _modifiers.Length; i++)
                                {
                                    modifiers[i] = _modifiers[i].Create();
                                }
                            }
                            data = new FloatDynamicStatData(_statObject, _maxSource, modifiers, _initial, _saveRatio);
                        }
                        else
                        {
                            data = new ClampedFloatStatData(_statObject, _min, _max, _initial, _initialIsRatio, _saveRatio);
                        }
                    }
                    else
                    {
                        data = new FloatStatData(_statObject, _initial);
                    }
                    break;
                case StatValueType.Integer:
                    if (_clamped)
                    {
                        if (_modifiable)
                        {
                            StatModifier[] modifiers = null;
                            if (_modifiers != null)
                            {
                                modifiers = new StatModifier[_modifiers.Length];

                                for (int i = 0; i < _modifiers.Length; i++)
                                {
                                    modifiers[i] = _modifiers[i].Create();
                                }
                            }
                            data = new ClampedIntModifiableStatData(_statObject, _maxSource, modifiers, _initial, _saveRatio);
                        }
                        else
                        {
                            data = new ClampedIntStatData(_statObject, (int)_min, (int)_max, (int)_initial, _initialIsRatio, _saveRatio);
                        }
                    }
                    else
                    {
                        data = new IntStatData(_statObject, (int)_initial);
                    }
                    break;
                default:
                    throw new System.NotSupportedException("unexpected enum value: " + _valueType.ToString());
            }

            return data;
        }

    }
}
