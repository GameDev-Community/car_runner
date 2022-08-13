using UnityEngine;
namespace DevourDev.Unity.Utils.SimpleStats.Modifiers
{
    [System.Serializable]
    public class StatModifierCreator
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private ModifyingMode _mode;
        [SerializeField] private float _value;


        public StatObject StatObject => _statObject;
        public ModifyingMode Mode => _mode;
        public float Value => _value;


        public StatModifier Create()
        {
            return new(_mode, _value);
        }
    }
}

