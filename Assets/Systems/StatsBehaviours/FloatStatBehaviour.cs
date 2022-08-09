using Game.Core;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Game.StatsBehaviours
{
    /// <summary>
    /// decorator and the same time handler BloodTrail
    /// </summary>
    public class FloatStatBehaviour : StatBehaviour
    {
        [Space]
        [SerializeField] private UnityEvent<float> _onAmountChanged_NewValue;
        [SerializeField] private UnityEvent<float> _onAmountChanged_NewValue_Normalized;
        [SerializeField] private UnityEvent<float> _onAmountChanged_SafeDelta;
        [Space]
        [SerializeField] private UnityEvent _onMinReached;
        [Space]
        [SerializeField] private UnityEvent _onMaxReached;



        private ClampedFloat _statData;


       
        protected ClampedFloat StatData => _statData;


        protected virtual void Start()
        {
            if (!Racer.TryGetStatData(StatObject, out var icv))
                throw new System.Collections.Generic.KeyNotFoundException(nameof(StatObject));

            //will throw cast exception if something changed for some reason
            var cf = (ClampedFloat)icv;

            cf.OnValueChanged += HandleCfValueChanged;
            cf.OnMinValueReached += HandleCfOnMinValueReached;
            cf.OnMaxValueReached += HandleCfOnMaxValueReached;
            _statData = cf;

            HandleCfValueChanged(cf, 0, 0);
        }


        private void HandleCfValueChanged(ClampedFloat se, float dd, float sd)
        {
            _onAmountChanged_NewValue?.Invoke(se.Value);
            _onAmountChanged_NewValue_Normalized?.Invoke(Mathf.InverseLerp(se.Min, se.Max, se.Value));
            _onAmountChanged_SafeDelta?.Invoke(sd);
        }

        private void HandleCfOnMinValueReached(ClampedFloat se, float dd, float sd)
        {
            _onMinReached?.Invoke();
        }

        private void HandleCfOnMaxValueReached(ClampedFloat se, float dd, float sd)
        {
            _onMaxReached?.Invoke();
        }
    }
}
