using UnityEngine;

namespace Game.Core
{
    [System.Serializable]
    public class SpeedModifier
    {
        //todo: turn Speed into a Stat and reimplement SpeedModifier
        //as StatModifier

        [SerializeField] private float _value;
        [SerializeField] private bool _flat;


        public SpeedModifier(float v, bool flat)
        {
            _value = v;
            _flat = flat;
        }


        public float Value => _value;
        public bool Flat => _flat;
    }
}
