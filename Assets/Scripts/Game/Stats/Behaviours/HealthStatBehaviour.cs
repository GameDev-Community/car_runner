using Externals.Utils.StatsSystem;
using Externals.Utils.StatsSystem.Modifiers;
using Game.Helpers;
using UnityEngine;

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

        private void Update()
        {
            StatsHolder.StatsCollection.TryGetStatDataT<FloatDynamicStatData>(_healthStatCreator.StatObject, out var hsd);
            hsd.Remove(Time.deltaTime);
        }
    }
}
