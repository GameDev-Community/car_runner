using UnityEngine;

namespace Garage
{
    //ready to move to Utils/Helpers
    public enum TransformAxis
    {
        Right,
        Up,
        Forward
    }

    //ready to move to Utils/Helpers
    public class TransformRotator : MonoBehaviour
    {
        [SerializeField] private TransformAxis _axis;
        [SerializeField] private float _degreesPerSecond = 0.3f / 0.02f;


        private void Update()
        {
            transform.Rotate(transform.up, _degreesPerSecond * Time.deltaTime);
        }
    }
}