using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Externals.Utils.StatsSystem
{
#if UNITY_EDITOR
    [CustomEditor(typeof(StatDataManipulation))]
    public class CustomActionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var statObjProp = serializedObject.FindProperty("_statObject");
            EditorGUILayout.PropertyField(statObjProp);

            int actionID = -1;
            var statObject = (StatObject)statObjProp.objectReferenceValue;

            if (statObject == null)
                goto End;

            var sdInfo = statObject.GetStatDataInfo();
            bool isInteger = sdInfo.NumericsType == StatObject.NumericsType.Integer;
            bool isClamped = sdInfo.Clamped;
            bool isModifiable = sdInfo.Modifiable;

            if (isClamped)
            {
                var clampedValueManipulationTypeProp = serializedObject.FindProperty("_clampedValueManipulationType");
                EditorGUILayout.PropertyField(clampedValueManipulationTypeProp);

                var clampedValueManipulationType = (StatDataManipulation.ClampedValueManipulationType)clampedValueManipulationTypeProp.intValue;

                if (clampedValueManipulationType == StatDataManipulation.ClampedValueManipulationType.Bounds)
                {
                    if (isModifiable)
                    {
                        //draw modifiers constructor;
                        var modifierCreatorsArrProp = serializedObject.FindProperty("_statModifierCreators");
                        EditorGUILayout.PropertyField(modifierCreatorsArrProp);
                        actionID = 0;
                    }
                    else
                    {
                        if (isInteger)
                        {
                            //draw ClampedIntBoundsManipulator
                            var clampedIntBoundsManipulatorProp = serializedObject.FindProperty("_clampedIntBoundsManipulator");
                            EditorGUILayout.PropertyField(clampedIntBoundsManipulatorProp);
                            actionID = 6;
                        }
                        else
                        {
                            //draw ClampedFloatBoundsManipulator
                            var clampedFloatBoundsManipulatorProp = serializedObject.FindProperty("_clampedFloatBoundsManipulator");
                            EditorGUILayout.PropertyField(clampedFloatBoundsManipulatorProp);
                            actionID = 5;
                        }

                    }
                }
                else
                {
                    //change value

                    if (isInteger)
                    {
                        var intClampedAmountManipulatorProp = serializedObject.FindProperty("_intClampedAmountManipulators");
                        EditorGUILayout.PropertyField(intClampedAmountManipulatorProp);
                        actionID = 4;
                    }
                    else
                    {
                        var floatClampedAmountManipulatorProp = serializedObject.FindProperty("_floatClampedAmountManipulators");
                        EditorGUILayout.PropertyField(floatClampedAmountManipulatorProp);
                        actionID = 3;
                    }
                }
            }
            else
            {
                //non-clamped

                if (isInteger)
                {
                    var intAmountManipulatorProp = serializedObject.FindProperty("_intAmountManipulator");
                    EditorGUILayout.PropertyField(intAmountManipulatorProp);
                    actionID = 2;
                }
                else
                {
                    var floatAmountManipulatorProp = serializedObject.FindProperty("_floatAmountManipulator");
                    EditorGUILayout.PropertyField(floatAmountManipulatorProp);
                    actionID = 1;
                }
            }

End:
            var idField = typeof(StatDataManipulation).GetField("_actionID", BindingFlags.Instance | BindingFlags.NonPublic);
            idField.SetValue(target, actionID);
            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif


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
