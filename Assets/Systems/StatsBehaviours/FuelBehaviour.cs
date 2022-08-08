using UnityEngine;
using Utils;

namespace Game.StatsBehaviours
{
    public class FuelBehaviour : FloatStatBehaviour
    {
        [Tooltip("t: speed, v: from 0 to 1")]
        [SerializeField] private AnimationCurve _speedToFuelLossCurve;
        [SerializeField] private float _fuelFuelLossFlatModifier;
        [SerializeField] private float _fuelFuelLossMultModifier = 1.4f;
        //prototype
        [SerializeField] private float _fuelCostPerSecond = 10f;



        protected override void Start()
        {
            base.Start();
            StatData.OnMinValueReached += HandleOutOfFuel;
        }

        private void HandleOutOfFuel(ClampedFloat se, float dd, float sd)
        {
            enabled = false;
        }

        private void Update()
        {
            StatData.Change(-_fuelCostPerSecond * Time.deltaTime);
        }

    }
}
