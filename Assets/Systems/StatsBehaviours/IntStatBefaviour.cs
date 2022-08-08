using Game.Core;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Game.StatsBehaviours
{
    public class IntStatBefaviour : StatBehaviour
    {
        [SerializeField] private UnityEvent<int> _onValueChanged_NewValue;
        [SerializeField] private UnityEvent<string> _onValueChanged_NewValue_String;
        [SerializeField] private UnityEvent<float> _onValueChanged_NewValue_Normalized;
        [SerializeField] private UnityEvent<int> _onValueChanged_SafeDelta;
        [Space]
        [SerializeField] private UnityEvent _onMinReached;
        [Space]
        [SerializeField] private UnityEvent _onMaxReached;
        [Space]
        private ClampedInt _statData;


        protected ClampedInt StatData => _statData;


        protected virtual void Start()
        {
            if (!Racer.TryGetStatData(StatObject, out var icv))
                throw new System.Collections.Generic.KeyNotFoundException(nameof(StatObject));

            //will throw cast exception if something changed for some reason
            var cint = (ClampedInt)icv;

            cint.OnValueChanged += HandleCintValueChanged;
            cint.OnMinValueReached += HandleCintOnMinValueReached;
            cint.OnMaxValueReached += HandleCintOnMaxValueReached;
            _statData = cint;
            HandleCintValueChanged(cint, 0, 0);
        }

        private void HandleCintValueChanged(ClampedInt se, int dd, int sd)
        {
            _onValueChanged_NewValue?.Invoke(se.Value);
            _onValueChanged_NewValue_String?.Invoke(se.Value.ToString());
            _onValueChanged_NewValue_Normalized?.Invoke(Mathf.InverseLerp(se.Min, se.Max, se.Value));
            _onValueChanged_SafeDelta?.Invoke(sd);
        }

        private void HandleCintOnMinValueReached(ClampedInt se, int dd, int sd)
        {
            _onMinReached?.Invoke();
        }

        private void HandleCintOnMaxValueReached(ClampedInt se, int dd, int sd)
        {
            _onMaxReached?.Invoke();
        }
    }
}
