using System.Reflection;
using UnityEditor;

namespace Externals.Utils.StatsSystem
{
#if UNITY_EDITOR
    [CustomEditor(typeof(StatDataInitializer))]
    public class StatDataInitializerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var soProp = P("_statObject");
            DrawP(soProp);

            int actionID = -1;
            var statObject = (StatObject)soProp.objectReferenceValue;

            if (statObject == null)
                goto End;

            var sdInfo = statObject.GetStatDataInfo();
            bool isInteger = sdInfo.NumericsType == StatObject.NumericsType.Integer;
            bool isClamped = sdInfo.Clamped;
            bool isModifiable = sdInfo.Modifiable;

            if (isClamped)
            {
                if (isInteger)
                {
                    if (isModifiable)
                    {
                        FindAndDrawP("_intDynamicStatDataCreator");
                        actionID = 4;
                    }
                    else
                    {
                        FindAndDrawP("_clampedIntStatDataCreator");
                        actionID = 5;
                    }
                }
                else
                {
                    if (isModifiable)
                    {
                        FindAndDrawP("_floatDynamicStatDataCreator");
                        actionID = 0;
                    }
                    else
                    {
                        FindAndDrawP("_clampedFloatStatDataCreator");
                        actionID = 1;
                    }
                }
            }
            else
            {
                if (isInteger)
                {
                    if (isModifiable)
                    {
                        FindAndDrawP("_intModifiableStatDataCreator");
                        actionID = 6;
                    }
                    else
                    {
                        FindAndDrawP("_intStatDataCreator");
                        actionID = 7;
                    }
                }
                else
                {
                    if (isModifiable)
                    {
                        FindAndDrawP("_floatModifiableStatDataCreator");
                        actionID = 2;
                    }
                    else
                    {
                        FindAndDrawP("_floatStatDataCreator");
                        actionID = 3;
                    }
                }
            }

End:
            var idField = FIHere("_actionID");
            idField.SetValue(target, actionID);
            EditorUtility.SetDirty(target);

            serializedObject.ApplyModifiedProperties();


            UnityEditor.SerializedProperty P(string fieldName) => serializedObject.FindProperty(fieldName);

            void DrawP(UnityEditor.SerializedProperty p) => EditorGUILayout.PropertyField(p);

            void FindAndDrawP(string fieldName)
            {
                var sp = P(fieldName);
                DrawP(sp);
                FieldInfo spFI = FIHere(fieldName);
                FieldInfo soFI = FI(spFI.FieldType, "_statObject");
                object spVal = spFI.GetValue(target);
                soFI.SetValue(spVal, statObject);

                if (isModifiable)
                {
                    FieldInfo smcFI = FI(spFI.FieldType, "_initialModifiers");

                    if (smcFI == null)
                    {
                        UnityEngine.Debug.LogError("failed to reflect _initialModifiers in " + spFI.FieldType.Name);
                        return;
                    }

                    object smcVal = smcFI.GetValue(spVal);

                    if (smcVal != null && smcVal is StatModifierCreator[] arr)
                    {
                        var c = arr.Length;

                        if (c > 0)
                        {
                            var smcsoFI = FI(typeof(StatModifierCreator), "_statObject");

                            for (int i = -1; ++i < c;)
                            {
                                smcsoFI.SetValue(arr[i], statObject);
                            }
                        }
                    }
                }
            }

            System.Reflection.FieldInfo FI(System.Type t, string fieldName)
                => t.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            System.Reflection.FieldInfo FIHere(string fieldName)
                => FI(typeof(StatDataInitializer), fieldName);
        }
    }

#endif
}