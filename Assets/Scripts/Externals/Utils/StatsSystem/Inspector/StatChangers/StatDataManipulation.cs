using UnityEngine;

namespace Externals.Utils.StatsSystem
{
    public class StatDataManipulation : MonoBehaviour, IStatChanger
    {
        public enum ClampedValueManipulationType
        {
            Bounds,
            Value
        }

        [SerializeField] private StatObject _statObject;

        [SerializeField] private ClampedValueManipulationType _clampedValueManipulationType;


        //for modifiables
        [SerializeField] private StatModifierCreator[] _statModifierCreators; //ID 0
        //for iamount manipulatables
        [SerializeField] private AmountManipulator<float> _floatAmountManipulator; //ID 1
        [SerializeField] private AmountManipulator<int> _intAmountManipulator; //ID 2

        //for iclamped amount manipulatables
        [SerializeField] private ClampedAmountManipulator<float> _floatClampedAmountManipulators; //ID 3
        [SerializeField] private ClampedAmountManipulator<int> _intClampedAmountManipulators; //ID 4

        //for clamped non-modifiable floats to manipulate bounds
        [SerializeField] private ClampedBoundsManipulator<float> _clampedFloatBoundsManipulator; //ID 5
        [SerializeField] private ClampedBoundsManipulator<int> _clampedIntBoundsManipulator; //ID 6


        [SerializeField, HideInInspector] private int _actionID;


        public void Apply(StatsCollection sc, bool inv = false)
        {
            switch (_actionID)
            {
                case 0:
                    foreach (var item in _statModifierCreators)
                    {
                        item.Apply(sc, inv);
                        break;
                    }
                    break;
                case 1:
                    _floatAmountManipulator.Apply(sc, inv);
                    break;
                case 2:
                    _intAmountManipulator.Apply(sc, inv);
                    break;
                case 3:
                    _floatClampedAmountManipulators.Apply(sc, inv);
                    break;
                case 4:
                    _intClampedAmountManipulators.Apply(sc, inv);
                    break;
                case 5:
                    _clampedFloatBoundsManipulator.Apply(sc, inv);
                    break;
                case 6:
                    _clampedIntBoundsManipulator.Apply(sc, inv);
                    break;
                default:
#if UNITY_EDITOR
                    Debug.LogError("unexpected id: " + _actionID.ToString());
#endif
                    break;
            }
        }
    }
}
