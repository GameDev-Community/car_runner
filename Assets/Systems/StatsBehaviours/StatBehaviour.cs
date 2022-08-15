using DevourDev.Unity.Utils.SimpleStats;
using UnityEngine;
using UnityEngine.Events;
using Utils.Attributes;

namespace Game.StatsBehaviours
{
    public class StatBehaviour : MonoBehaviour
    {
        [SerializeField, RequireInterface(typeof(IStatsHolder))] private UnityEngine.Object _statsHolder;
        [SerializeField] private StatObject _statObject;

        [SerializeField] private UnityEvent<float> _onStatChanged_NewValue;
        [SerializeField] private UnityEvent<float> _onStatChanged_DirtyDelta;
        [SerializeField] private UnityEvent<float> _onStatChanged_SafeDelta;

        private IModifiableStatData _statData;


        protected StatObject StatObject => _statObject;
        protected IModifiableStatData StatData => _statData;


        protected virtual void Start()
        {
            var ish = _statsHolder as IStatsHolder;
            var stats = ish.Stats;
            var check = stats.TryGetStatData(_statObject, out _statData);

#if UNITY_EDITOR
            if (!check)
                throw new System.Collections.Generic.KeyNotFoundException($"{_statObject}");
#endif

            _statData.OnValueChanged += HandleValueChanged;
        }

        private void HandleValueChanged(IModifiableStatData statData, float dd, float sd)
        {
            _onStatChanged_NewValue?.Invoke(statData.Value);
        }
    }
}
