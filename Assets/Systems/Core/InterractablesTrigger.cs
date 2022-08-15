using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class InterractablesTrigger : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Collider _collider;

        private Collider[] _buffer;
        private float _checkRadius;
        private Transform _tr;


        private void Awake()
        {
            _buffer = new Collider[128];
            var extends = _collider.bounds.extents;
            _checkRadius = Mathf.Max(extends.x, extends.y, extends.z);
            _tr = transform;
        }


        private void FixedUpdate()
        {
            var p = _tr.position;
            int c = Physics.OverlapSphereNonAlloc(p, _checkRadius, _buffer, _layerMask);

            if (c == 0)
                return;

            var b = _buffer;
            var r = _tr.rotation;
            for (int i = -1; ++i < c;)
            {
                var otherCol = b[i];
                var otherTr = otherCol.transform;
                var otherP = otherTr.position;
                var otherRot = otherTr.rotation;

                if (Physics.ComputePenetration(_collider, p, r, otherCol, otherP, otherRot, out var dir, out var dist))
                {
                    if (otherCol.TryGetComponent(out Interactables.InteractableItem interactable))
                    {
                        interactable.Interact(_player);
                    }
                }
            }
        }


    }
}
