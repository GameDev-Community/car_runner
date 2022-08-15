using UnityEngine;
namespace DevourDev.Unity.Utils.SimpleStats.Modifiers
{
    [System.Serializable]
    public class StatModifierCreator
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private ModifyingMode _mode;
        [SerializeField] private float _value;
        [SerializeField, Min(1)] private int _amount = 1;


        public StatModifierCreator(StatObject statObject, ModifyingMode mode, float value, int amount = 1)
        {
            _statObject = statObject;
            _mode = mode;
            _value = value;
            _amount = amount > 0 ? amount : 1;
        }


        public StatObject StatObject => _statObject;
        public ModifyingMode Mode => _mode;
        public float Value => _value;
        public int Amount => _amount;


        public StatModifier Create()
        {
            return new(_mode, _value);
        }
    }
}

