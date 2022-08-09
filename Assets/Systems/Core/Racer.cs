using Game.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Game.Core
{
    public class Racer : MonoBehaviour
    {
        private class Optimizer
        {
            public List<StatModifier> Modifiers;
            public Vector2 Sum;


            public Optimizer(List<StatModifier> modifiers, Vector2 sum)
            {
                this.Modifiers = modifiers;
                this.Sum = sum;
            }
        }


        // prototype
        [SerializeField] private Collider _c;

        private Dictionary<StatObject, IClampedValue> _stats;
        private Dictionary<StatObject, Optimizer> _modifiers;
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
            var c = Physics.OverlapBoxNonAlloc(_c.bounds.center, _c.bounds.extents,
                _buffer, Quaternion.identity, Accessors.InteractablesLM);

            if (c == 0)
                return;

            for (int i = 0; i < c; i++)
            {
                Collider col = _buffer[i];
                if (col.TryGetComponent<Interactables.InteractableItem>(out var ii))
                    ii.Interact(this);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(_c.bounds.center, _c.bounds.size);
        }

        public bool TryGetStatData(StatObject sobj, out IClampedValue v)
           => _stats.TryGetValue(sobj, out v);


        public void AddStatModifier(StatObject statObj, StatModifier m)
        {
            if (!_modifiers.TryGetValue(statObj, out var v))
            {
                v = new(new List<StatModifier>(), new Vector2(0, 1));
                _modifiers.Add(statObj, v);
            }

            v.Modifiers.Add(m);
            RecalculateModifiers(statObj);
        }

        public void RemoveStatModifier(StatObject statObj, StatModifier m)
        {
            if (_modifiers.TryGetValue(statObj, out var v))
            {
                if (v.Modifiers.Remove(m))
                    RecalculateModifiers(statObj);
            }
        }


        private void Init()
        {
            _stats = InitialStatsBalanceController.GetInitialStats_Prototype();

            //subs

            //

            _modifiers = new();
            //RecalculateModifiers();
        }

        private void RecalculateModifiers(StatObject statObj)
        {
            float flat = 0;
            float mult = 1;

            var v = _modifiers[statObj];
            var modifiers = v.Modifiers;

            foreach (var m in modifiers)
            {
                if (m.Flat)
                    flat += m.Value;
                else
                    mult += m.Value;
            }

            v.Sum = new Vector2(flat, mult);
        }


        /// <summary>
        /// speed of velocity or any other shift
        /// </summary>
        /// <returns></returns>
        public float ProcessStatValue(StatObject staObject, float dirty)
        {
            if (!_modifiers.TryGetValue(staObject, out var v))
                return dirty;

            var sum = v.Sum;
            return (dirty + sum.x) * sum.y;
        }

    }
}
