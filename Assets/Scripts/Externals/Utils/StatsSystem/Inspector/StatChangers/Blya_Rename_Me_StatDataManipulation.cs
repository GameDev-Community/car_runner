using UnityEngine;

namespace Externals.Utils.StatsSystem
{
#if false
//это говно должно было быть хуйней для апгрейдов, но его рот ебал нахуй кастом инспектора ебаного,
который только в компонентах юнити работает, ебать его в корень жопы нахуй сука хуй!
    [System.Serializable]
    public class Blya_Rename_Me_StatDataManipulation : IStatChanger
    {
        [SerializeField, HideInInspector] private int _actionID;

        [SerializeField] private StatObject _statObject;

        //for modifiables
        [SerializeField] private StatModifierCreator[] _statModifierCreators; //ID 0
        //for iamount manipulatables
        [SerializeField] private AmountManipulator<float> _floatAmountManipulator; //ID 1
        [SerializeField] private AmountManipulator<int> _intAmountManipulator; //ID 2


        //for clamped non-modifiable floats to manipulate bounds
        [SerializeField] private ClampedBoundsManipulator<float> _clampedFloatBoundsManipulator; //ID 5
        [SerializeField] private ClampedBoundsManipulator<int> _clampedIntBoundsManipulator; //ID 6


        //3 и 4 удалены, т.к. этот класс выделялся для апгрейдов, которые подразумевают:
        //1) изменение ГРАНИЦ ограниченых статов;
        //2) изменение значений неограниченых статов;
        // (и в 1 и во 2 случаях, манипуляции с модификаторами включительны)

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
#endif
}
