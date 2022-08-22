using Externals.Utils.StatsSystem;
using Externals.Utils.StatsSystem.Modifiers;
using Game.Helpers;
using Game.Interactables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Stats
{
    public class HealthStatBehaviour : StatsBehaviour
    {
        [SerializeField] private FloatDynamicStatDataCreator _healthStatCreator;


        protected override void Awake()
        {
            base.Awake();
            var healthSD = _healthStatCreator.Create();
            StatsHolder.StatsCollection.AddStat(healthSD.StatObject, healthSD);
            healthSD.OnClampedValueReachedMin += HandleHealthEnded;
        }

        private void HandleHealthEnded(ClampedFloat arg1, float arg2, float arg3)
        {
            Accessors.Player.Kill();
        }
    }

    public class DamagableItem : MonoBehaviour
    {
        [System.Serializable]
        public class Stage
        {
            [SerializeField] private float _healthThreshold;

            [SerializeField] private UnityEvent _onEnterState;
            [SerializeField] private UnityEvent _onExitState;

            [SerializeField] private ParticleSystemAndParticles _vfx;


            public float HealthTreshold;


            public void Enter()
            {

                _onEnterState?.Invoke();
            }

            public void Exit()
            {

                _onExitState?.Invoke();
            }


        }


        [SerializeField] private StatObject _healthStatObject;
        [SerializeField] private Stage[] _stages;
#if UNITY_EDITOR
        [SerializeField, HideInInspector] private int _stagesTmpLength;
#endif

        private int _currentStageIndex;


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stages != null)
            {
                if (_stages.Length != _stagesTmpLength)
                {
                    _stagesTmpLength = _stages.Length;
                    Array.Sort(_stages, (x, y) => x.HealthTreshold.CompareTo(y.HealthTreshold));
                }
            }
        }
#endif

        private void Start()
        {
            _currentStageIndex = _stages == null ? -1 : _stages.Length - 1;
            var check = Accessors.PlayerStats.TryGetStatDataT<FloatDynamicStatData>(_healthStatObject, out var healthSD);

#if UNITY_EDITOR
            if (!check)
                throw new System.Exception($"stat not found: {_healthStatObject}, {Accessors.PlayerStats.TryGetStatData(_healthStatObject, out var x)}, {x}");
#endif
            healthSD.OnClampedValueChanged += HandleHealthChanged;

        }

        private void HandleHealthChanged(ClampedFloat sender, float dirtyDelta, float safeDelta)
        {
            

        }


       
    }
}
