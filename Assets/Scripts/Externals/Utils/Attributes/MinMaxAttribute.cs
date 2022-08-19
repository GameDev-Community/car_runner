using UnityEngine;

namespace Utils.Attributes
{
    public class MinMaxAttribute : PropertyAttribute
    {
        private readonly float _min;
        private readonly float _max;


        public MinMaxAttribute(float min, float max)
        {
            _min = min;
            _max = max;
        }


        public float Min => _min;
        public float Max => _max;
    }    
}