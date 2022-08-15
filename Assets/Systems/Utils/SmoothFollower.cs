using UnityEngine;

namespace Utils
{
    [System.Obsolete("WIP", true)]
    public class SmoothFollower : MonoBehaviour
    {
        [SerializeField] private Transform _followTarget;
        [SerializeField] private bool _lookAt;
        [SerializeField, Range(0, 5f)] private float _smoothingTime = 0.4f;
        [SerializeField, InspectorName("Position Offset")] private Vector3 _positionOffset_initor;
        [SerializeField, InspectorName("Position Offset")] private Vector3 _rotationOffset_initor;
        [SerializeField] private bool _considerStartPositionsDelta;

        private Transform _tr;
        private Vector3 _posOffset;
        private Quaternion _rotOffset;

        private float _noUpdateTime;
        private Vector3 _targetTmpP;
        //private Quaternion _targetTmpR;


        private void Start()
        {
           
        }


        private void LateUpdate()
        {
        }
    }
}