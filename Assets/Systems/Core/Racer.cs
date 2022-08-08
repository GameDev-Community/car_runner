using Game.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Game.Core
{
    public class Racer : MonoBehaviour
    {
        // prototype
        [SerializeField] private Collider _c;

        private Dictionary<StatObject, IClampedValue> _stats;
        private List<SpeedModifier> _speedModifiers;

        private Vector2 _speedModifiersSums;
        private Collider[] _buffer;



        private void Awake()
        {
            //scripts execution order: InitialStatsBalanceController should
            //have more priority than Racer


            _buffer = new Collider[128];

            Init();
        }


        private void FixedUpdate()
        {
            CheckInteractables();

        }

        private void CheckInteractables()
        {
            var c = Physics.OverlapBoxNonAlloc(transform.position, _c.bounds.extents,
                _buffer, _c.transform.rotation, Accessors.InteractablesLM);

            if (c == 0)
                return;

            for (int i = 0; i < c; i++)
            {
                Collider col = _buffer[i];
                if (col.TryGetComponent<Interactables.InteractableItem>(out var ii))
                    ii.Interact(this);
            }
        }

        public bool TryGetStatData(StatObject sobj, out IClampedValue v)
           => _stats.TryGetValue(sobj, out v);


        public void AddSpeedModifier(SpeedModifier m)
        {
            _speedModifiers.Add(m);
            RecalculateSpeedModifiers();
        }

        public void RemoveSpeedModifier(SpeedModifier m)
        {
            if (_speedModifiers.Remove(m))
                RecalculateSpeedModifiers();
        }


        private void Init()
        {
            _stats = InitialStatsBalanceController.GetInitialStats_Prototype();

            //subs

            //

            _speedModifiers = new();
            RecalculateSpeedModifiers();
        }

        private void RecalculateSpeedModifiers()
        {
            float flat = 0;
            float mult = 1;

            foreach (var m in _speedModifiers)
            {
                if (m.Flat)
                    flat += m.Value;
                else
                    mult += m.Value;
            }

            _speedModifiersSums = new Vector2(flat, mult);
        }


        /// <summary>
        /// speed of velocity or any other shift
        /// </summary>
        /// <returns></returns>
        public float ProcessSpeed(float dirty)
        {
            var sms = _speedModifiersSums;
            return (dirty + sms.x) * sms.y;
        }

        public Vector3 ProcessVelocity(Vector3 dirty)
        {
            var sms = _speedModifiersSums;
            return (dirty + Vector3.one * sms.x) * sms.y;
        }
    }
}
