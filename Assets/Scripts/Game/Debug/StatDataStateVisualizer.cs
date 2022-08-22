using Externals.Utils.StatsSystem;
using Externals.Utils.StatsSystem.Modifiers;
using UnityEngine;

namespace Game.Debug
{
    public class StatDataStateVisualizer : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _statNameText;
        [SerializeField] private TMPro.TextMeshProUGUI _statStateText;


        public void Init(IStatData isd)
        {
            _statNameText.text = isd.StatObject.MetaInfo.Name;

            switch (isd)
            {
                case ClampedFloatStatData clampedFloatStatData:
                    clampedFloatStatData.OnBoundsChanged += ClampedFloatStatData_OnBoundsChanged;
                    clampedFloatStatData.OnClampedValueChanged += ClampedFloatStatData_OnClampedValueChanged;

                    ClampedFloatStatData_OnBoundsChanged(clampedFloatStatData, Vector3.zero);
                    ClampedFloatStatData_OnClampedValueChanged(clampedFloatStatData, 0, 0);
                    break;
                case FloatStatData floatStatData:
                    floatStatData.OnFloatValueChanged += FloatStatData_OnFloatValueChanged;
                    FloatStatData_OnFloatValueChanged(floatStatData, 0);
                    break;
                case ClampedIntStatData clampedIntStatData:
                    clampedIntStatData.OnBoundsChanged += ClampedIntStatData_OnBoundsChanged;
                    clampedIntStatData.OnClampedValueChanged += ClampedIntStatData_OnClampedValueChanged;
                    ClampedIntStatData_OnBoundsChanged(clampedIntStatData, Vector3Int.zero);
                    ClampedIntStatData_OnClampedValueChanged(clampedIntStatData, 0, 0);
                    break;
                case IntStatData intStatData:
                    intStatData.OnIntValueChanged += IntStatData_OnIntValueChanged;
                    IntStatData_OnIntValueChanged(intStatData, 0);
                    break;
                case FloatModifiableStatData floatModifiableStatData:
                    floatModifiableStatData.OnFloatValueChanged += FloatModifiableStatData_OnFloatValueChanged;
                    FloatModifiableStatData_OnFloatValueChanged(floatModifiableStatData, 0);
                    break;
                default:
                    throw new System.NotImplementedException($"unexpected statdata: {isd.GetType()}");
            }
        }

        private void FloatModifiableStatData_OnFloatValueChanged(IFloatValueCallback arg1, float arg2)
        {
            if (arg1 is IModifiableStatData mdfbl)
            {
                _statStateText.text = $"value: {arg1.Value}, modifiers: {mdfbl.StatModifiers.ModifyValue(1)}";
            }
            else
            {
                _statStateText.text = $"value: {arg1.Value}";
            }
        }

        private void IntStatData_OnIntValueChanged(IIntValueCallback arg1, int arg2)
        {
            _statStateText.text = $"value: {arg1.Value}";
        }

        private void ClampedIntStatData_OnClampedValueChanged(ClampedInt arg1, int arg2, int arg3)
        {
            _statStateText.text = $"min: {arg1.Min}, max: {arg1.Max}, cur: {arg1.Value}";
        }

        private void ClampedFloatStatData_OnClampedValueChanged(ClampedFloat arg1, float arg2, float arg3)
        {
            _statStateText.text = $"min: {arg1.Min}, max: {arg1.Max}, cur: {arg1.Value}";
        }

        private void ClampedIntStatData_OnBoundsChanged(ClampedInt arg1, Vector3Int arg2)
        {
            _statStateText.text = $"min: {arg1.Min}, max: {arg1.Max}, cur: {arg1.Value}";
        }

        private void FloatStatData_OnFloatValueChanged(IFloatValueCallback arg1, float arg2)
        {
            _statStateText.text = $"value: {arg1.Value}";
        }

        private void ClampedFloatStatData_OnBoundsChanged(ClampedFloat arg1, Vector3 arg2)
        {
            _statStateText.text = $"min: {arg1.Min}, max: {arg1.Max}, cur: {arg1.Value}";
        }
    }
}
