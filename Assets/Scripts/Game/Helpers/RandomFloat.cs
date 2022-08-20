using UnityEngine;
using Utils;

namespace Game.Interactables
{
    [System.Serializable]
    public class RandomFloat
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;
        [Tooltip("optional")]
        [SerializeField] private AnimationCurve _curve;


        public float GetValue()
        {
            return RandomHelpers.RandomFloat(_min, _max, _curve);
        }
    }
}