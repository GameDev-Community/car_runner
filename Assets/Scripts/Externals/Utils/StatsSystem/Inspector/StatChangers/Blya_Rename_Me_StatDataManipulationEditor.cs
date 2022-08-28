using System.Reflection;
using UnityEditor;

namespace Externals.Utils.StatsSystem
{
#if false
//По территории воинской части мчится прапорщик с тележкой, полной дерьма. Его останавливает капитан:
//- Стой! Что спиздил?
//- Ничего.
//- Не верю, - капитан запускает руки в дерьмо и шарит там. Ничего не
//найдя, отпускает прапорщика. Прапорщик несется дальше. Навстречу ему майор:
//- Стой! Что спиздил?
//- Ничего.
//- Не верю, - майор запускает руки в дерьмо и шарит там, но также ничего не находит. Прапорщик мчится дальше. Навстречу ему полковник:
//- Стой! Что спиздил?
//- Ничего.
//- Я те дам ничего, - полковник запускает руки в дерьмо и шарит там,
//но тоже ничего не находит. Наконец прапорщик выбегает за ворота части,оглядывается, вываливает из тачки дерьмо и злобно ворчит:
//-Что спиздил - что спиздил... Тележку спиздил!
#if UNITY_EDITOR
    [CustomEditor(typeof(Blya_Rename_Me_StatDataManipulation))]
    public class Blya_Rename_Me_StatDataManipulationEditor : Editor
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
                if (isModifiable)
                {
                    //draw modifiers constructor;
                    DrawStatModifiers();
                }
                else
                {
                    if (isInteger)
                    {
                        //draw ClampedIntBoundsManipulator
                        var clampedIntBoundsManipulatorProp = serializedObject.FindProperty("_clampedIntBoundsManipulator");
                        EditorGUILayout.PropertyField(clampedIntBoundsManipulatorProp);
                        actionID = 6;

                        var boundsManipulatorField = typeof(StatDataManipulation).GetField("_clampedIntBoundsManipulator", BindingFlags.Instance | BindingFlags.NonPublic);
                        var boundsManipulatorStatObjectField = typeof(ClampedBoundsManipulator<int>).GetField("_statObject", BindingFlags.Instance | BindingFlags.NonPublic);
                        boundsManipulatorStatObjectField.SetValue(boundsManipulatorField.GetValue(target), statObject);
                    }
                    else
                    {
                        //draw ClampedFloatBoundsManipulator
                        var clampedFloatBoundsManipulatorProp = serializedObject.FindProperty("_clampedFloatBoundsManipulator");
                        EditorGUILayout.PropertyField(clampedFloatBoundsManipulatorProp);
                        actionID = 5;

                        var boundsManipulatorField = typeof(StatDataManipulation).GetField("_clampedFloatBoundsManipulator", BindingFlags.Instance | BindingFlags.NonPublic);
                        var boundsManipulatorStatObjectField = typeof(ClampedBoundsManipulator<float>).GetField("_statObject", BindingFlags.Instance | BindingFlags.NonPublic);
                        boundsManipulatorStatObjectField.SetValue(boundsManipulatorField.GetValue(target), statObject);
                    }
                }
            }
            else
            {
                //non-clamped

                if (isModifiable)
                {
                    DrawStatModifiers();
                }
                else
                {
                    if (isInteger)
                    {
                        var intAmountManipulatorProp = serializedObject.FindProperty("_intAmountManipulator");
                        EditorGUILayout.PropertyField(intAmountManipulatorProp);
                        actionID = 2;

                        var amountManipulatorField = typeof(StatDataManipulation).GetField("_intAmountManipulator", BindingFlags.Instance | BindingFlags.NonPublic);
                        var amountManipulatorStatObjectField = typeof(AmountManipulator<int>).GetField("_statObject", BindingFlags.Instance | BindingFlags.NonPublic);
                        amountManipulatorStatObjectField.SetValue(amountManipulatorField.GetValue(target), statObject);
                    }
                    else
                    {
                        var floatAmountManipulatorProp = serializedObject.FindProperty("_floatAmountManipulator");
                        EditorGUILayout.PropertyField(floatAmountManipulatorProp);
                        actionID = 1;

                        var amountManipulatorField = typeof(StatDataManipulation).GetField("_floatAmountManipulator", BindingFlags.Instance | BindingFlags.NonPublic);
                        var amountManipulatorStatObjectField = typeof(AmountManipulator<float>).GetField("_statObject", BindingFlags.Instance | BindingFlags.NonPublic);
                        amountManipulatorStatObjectField.SetValue(amountManipulatorField.GetValue(target), statObject);
                    }
                }


            }

End:
            var idField = typeof(StatDataManipulation).GetField("_actionID", BindingFlags.Instance | BindingFlags.NonPublic);
            idField.SetValue(target, actionID);
            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();


            void DrawStatModifiers()
            {
                var modifierCreatorsArrProp = serializedObject.FindProperty("_statModifierCreators");
                EditorGUILayout.PropertyField(modifierCreatorsArrProp);
                actionID = 0;

                var smcreatorsField = typeof(StatDataManipulation).GetField("_statModifierCreators", BindingFlags.Instance | BindingFlags.NonPublic);
                var smCreatorsArrRaw = smcreatorsField.GetValue(target);

                if (smCreatorsArrRaw != null)
                {
                    var arr = (StatModifierCreator[])smCreatorsArrRaw;

                    if (arr.Length > 0)
                    {
                        var smSoField = typeof(StatModifierCreator).GetField("_statObject", BindingFlags.Instance | BindingFlags.NonPublic);

                        for (int i = 0; i < arr.Length; i++)
                        {
                            StatModifierCreator x = arr[i];

                            if (x != null)
                            {
                                smSoField.SetValue(x, statObject);
                            }
                        }
                    }
                }
            }
        }
    }
#endif
#endif
}
