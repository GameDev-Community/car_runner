using Externals.Utils.StatsSystem;
using Externals.Utils.StatsSystem.Modifiers;
using Game.Helpers;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Stats
{
    public class DamagableItem : StatsBehaviour
    {
        [System.Serializable]
        public class Stage
        {
            [Tooltip("If health value <= _healthTreshold stage switches to lower stage")]
            [SerializeField, Min(0f)] private float _healthThreshold;

            [SerializeField] private UnityEvent _onEnterFromLowerState;
            [SerializeField] private UnityEvent _onEnterFromHigherState;

            [SerializeField] private ParticleSystemAndParticles _vfx;


            public float HealthTreshold => _healthThreshold;
            public UnityEvent OnEnterFromLowerState => _onEnterFromLowerState;
            public UnityEvent OnEnterFromHigherState => _onEnterFromHigherState;


            public void EnterFromLower()
            {
                _onEnterFromLowerState?.Invoke();
            }

            public void EnterFromHigher()
            {
                _onEnterFromHigherState?.Invoke();
            }


        }


        [SerializeField] private StatObject _healthStatObject;
        [SerializeField] private Stage[] _stages;
        [SerializeField] private bool _normalizedHealthValues;
#if UNITY_EDITOR
        [SerializeField] private bool _sortStages;
        [SerializeField, HideInInspector] private int _stagesTmpLength;
#endif
        private int _currentStageIndex;


        public Stage CurrentStage => _stages[_currentStageIndex];


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stages != null)
            {
                if (_stages.Length != _stagesTmpLength || _sortStages)
                {
                    _sortStages = false;
                    _stagesTmpLength = _stages.Length;
                    Array.Sort(_stages, (x, y) => x.HealthTreshold.CompareTo(y.HealthTreshold));
                    UnityEditor.EditorUtility.SetDirty(this);
                }
            }
        }
#endif

        private void Start()
        {
#if UNITY_EDITOR
            for (int i = 0; i < _stages.Length; i++)
            {
                Stage item = _stages[i];
                int number = i;
                item.OnEnterFromLowerState.AddListener(() => UnityEngine.Debug.Log($"from lower to stage #{number}"));
                item.OnEnterFromHigherState.AddListener(() => UnityEngine.Debug.Log($"from higher to stage #{number}"));
            }
#endif

            var check = StatsHolder.StatsCollection.TryGetStatDataT<FloatDynamicStatData>(_healthStatObject, out var healthSD);
#if UNITY_EDITOR
            if (!check)
                throw new System.Exception($"stat not found: {_healthStatObject}, {Accessors.PlayerStats.TryGetStatData(_healthStatObject, out var x)}, {x}");
#endif
            healthSD.OnClampedValueChanged += HandleHealthChanged;

            float v;

            if (_normalizedHealthValues)
            {
                v = Mathf.InverseLerp(healthSD.Min, healthSD.Max, healthSD.Value);
            }
            else
            {
                v = healthSD.Value;
            }

            var stages = _stages;
            var c = stages.Length;

            for (int i = c - 1; i >= 0; i--)
            {
                var sv = stages[i].HealthTreshold;

                if (sv < v)
                {
                    _currentStageIndex = i;
                    break;
                }
            }

            UnityEngine.Debug.Log(_currentStageIndex);

        }

        private void HandleHealthChanged(ClampedFloat sender, float dirtyDelta, float safeDelta)
        {
            float v;

            if (_normalizedHealthValues)
            {
                v = Mathf.InverseLerp(sender.Min, sender.Max, sender.Value);
            }
            else
            {
                v = sender.Value;
            }

            if (safeDelta < 0)
            {
                CheckForStageDown(v);
            }
            else if (safeDelta > 0)
            {
                CheckForStageUp(v);
            }

        }

        private void CheckForStageDown(float v)
        {
            while (_currentStageIndex > 0 && v < CurrentStage.HealthTreshold)
            {
                ProcessStageDown();
            }
        }

        private void CheckForStageUp(float v)
        {
            while (_currentStageIndex < _stages.Length - 1 && v >= CurrentStage.HealthTreshold)
            {
                ProcessStageUp();
            }
        }


        private void ProcessStageDown()
        {
            //if (_currentStageIndex == 0)
            //    return;

            --_currentStageIndex;

#if UNITY_EDITOR
            if (_currentStageIndex < 0)
                throw new IndexOutOfRangeException($"_currentStageIndex: {_currentStageIndex}");
#endif

            CurrentStage.EnterFromHigher();
        }

        private void ProcessStageUp()
        {
            //if (_currentStageIndex == _stages.Length - 1)
            //    return;

            ++_currentStageIndex;

#if UNITY_EDITOR
            if (_currentStageIndex == _stages.Length)
                throw new IndexOutOfRangeException($"_currentStageIndex: {_currentStageIndex}");
#endif

            CurrentStage.EnterFromLower();
        }


    }
}
