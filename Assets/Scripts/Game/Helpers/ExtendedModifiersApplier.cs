using Externals.Utils.StatsSystem;
using System;
using UnityEngine;

namespace Game.Interactables
{

    [System.Serializable]
    public class ExtendedModifiersApplier
    {
        [System.Serializable]
        private class ModifiersGroup
        {
            [SerializeField, NonReorderable] private StatModifierCreator[] _modifiersCreators;

            [SerializeField] private bool _doNotApply;
            [SerializeField] private bool _delayedApplying;
            [SerializeField] private float _applyingDelay;

            [SerializeField] private bool _doNotDisapply;
            [SerializeField] private bool _delayedDisapplying;
            [Tooltip("не зависит от _applyingDelay, то есть, если" +
                " _disapplyingDelay будет равен _applyingDelay, то либо ничего не изменится," +
                "либо добавятся перманентные модификаторы (т.к. модификаторы не убираются \"в долг\")")]
            [SerializeField] private float _disapplyingDelay;


            public void Apply(StatsCollection statsCollection)
            {
                if (!_doNotApply)
                {
                    if (_delayedApplying)
                    {
                        var go = new GameObject(String.Empty, typeof(DelayedModifierApplier));
                        var dma = go.GetComponent<DelayedModifierApplier>();
                        dma.Init(_modifiersCreators, true, _applyingDelay, statsCollection, null);
                    }
                    else
                    {
                        ModifiersHelpers.ApplyModifiers(_modifiersCreators, statsCollection);
                    }
                }

                if (!_doNotDisapply)
                {
                    if (_delayedDisapplying)
                    {
                        var go = new GameObject(String.Empty, typeof(DelayedModifierApplier));
                        var dma = go.GetComponent<DelayedModifierApplier>();
                        dma.Init(_modifiersCreators, false, _disapplyingDelay, statsCollection, null);
                    }
                    else
                    {
                        ModifiersHelpers.ApplyModifiers(_modifiersCreators, statsCollection);
                    }
                }

            }
        }


        [SerializeField, NonReorderable] private ModifiersGroup[] _grops;


        public void Apply(StatsCollection statsCollection)
        {
            foreach (var g in _grops)
            {
                g.Apply(statsCollection);
            }
        }
    }
}