using UnityEngine;
using Externals.Utils.StatsSystem.Modifiers;

namespace Externals.Utils.StatsSystem
{
    [System.Serializable]
    public class StatModifierCreator
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private Modifiers.ModifyingMode _mode;
        [SerializeField] private float _value;


        public StatModifierCreator(StatObject statObject, ModifyingMode mode, float value)
        {
            _statObject = statObject;
            _mode = mode;
            _value = value;
        }


        public StatObject StatObject => _statObject;
        public ModifyingMode Mode => _mode;
        public float Value => _value;


        public StatModifier Create()
        {
            return new(_mode, _value);
        }
    }
}
