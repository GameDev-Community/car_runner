using UnityEngine;

namespace Game.Core
{
    [System.Serializable]
    public class StatModifier
    {
        [SerializeField] private float _value;
        [SerializeField] private bool _flat;


        public StatModifier(float v, bool flat)
        {
            _value = v;
            _flat = flat;
        }


        public float Value => _value;
        public bool Flat => _flat;
    }
}
