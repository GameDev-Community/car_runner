using Externals.Utils.StatsSystem;
using System.Collections;
using UnityEngine;

namespace Game.Stats
{
    public class StatsManipulatingFromInspectorTester : MonoBehaviour
    {
        [SerializeField] private StatDataManipulation[] _manipulations;


        private IEnumerator Start()
        {
            var sc = Helpers.Accessors.PlayerStats;

            foreach (var item in sc)
            {
                var sdRaw = item.Value;

                if (sdRaw is ClampedFloat clampedFloat)
                {
                    clampedFloat.OnBoundsChanged += ClampedFloat_OnBoundsChanged;
                }

                if (sdRaw is IValueCallback<int> valueSD)
                {
                    valueSD.OnValueChanged += ValueSD_OnValueChanged_Int;
                }
                else if (sdRaw is IValueCallback<float> valueSDf)
                {
                    valueSDf.OnValueChanged += ValueSD_OnValueChanged_Float;
                }

                UnityEngine.Debug.Log(sdRaw.GetType().Name);
            }

            foreach (var m in _manipulations)
            {
                m.Apply(sc, false);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void ClampedFloat_OnBoundsChanged(ClampedFloat arg1, Vector3 arg2)
        {
            UnityEngine.Debug.Log($"{arg1.GetType()}: bounds changed: {arg1.Min}, {arg1.Max}, {arg1.Value}");
        }

        private void ValueSD_OnValueChanged_Int(IValueCallback<int> arg1, int arg2)
        {
            UnityEngine.Debug.Log($"{arg1.GetType()}: value changed: {arg1.Value}");
        }
        private void ValueSD_OnValueChanged_Float(IValueCallback<float> arg1, float arg2)
        {
            UnityEngine.Debug.Log($"{arg1.GetType()}: value changed: {arg1.Value}");
        }

    }
}
