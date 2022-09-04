#if UNITY_EDITOR
using DevourDev.Base.Reflections;
using DevourDev.Unity.Utils.Editor.Window;
using Externals.Utils.StatsSystem;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Game.Garage
{
    public class StatDataInitializerWindow : ExtendedEditorWindow
    {
        /// <summary>
        /// 0: FloatDynamicStatData
        /// 1: ClampedFloatStatData
        /// 2: FloatModifiableStatData
        /// 3: FloatStatData 
        /// 4: IntDynamicStatData
        /// 5: ClampedIntStatData 
        /// 6: IntModifiableStatData
        /// 7: IntStatData
        /// </summary>
        [SerializeField] private StatDataRuntimeCreator _creator;
        private System.Action<StatDataRuntimeCreator> _onCompleteCallback;


        private void OnGUI()
        {
            SerializedObject.Update();
            var soProp = PR("_statObject");
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
                        actionID = 4;
                        FindAndDrawPR("_intDynamicStatDataCreator");
                    }
                    else
                    {
                        actionID = 5;
                        FindAndDrawPR("_clampedIntStatDataCreator");
                    }
                }
                else
                {
                    if (isModifiable)
                    {
                        actionID = 0;
                        FindAndDrawPR("_floatDynamicStatDataCreator");
                    }
                    else
                    {
                        actionID = 1;
                        FindAndDrawPR("_clampedFloatStatDataCreator");
                    }
                }
            }
            else
            {
                if (isInteger)
                {
                    if (isModifiable)
                    {
                        actionID = 6;
                        FindAndDrawPR("_intModifiableStatDataCreator");
                    }
                    else
                    {
                        actionID = 7;
                        FindAndDrawPR("_intStatDataCreator");
                    }
                }
                else
                {
                    if (isModifiable)
                    {
                        actionID = 2;
                        FindAndDrawPR("_floatModifiableStatDataCreator");
                    }
                    else
                    {
                        actionID = 3;
                        FindAndDrawPR("_floatStatDataCreator");
                    }
                }
            }

End:

            _creator.SetField("_actionID", actionID);


            bool add = soProp.objectReferenceValue != null && GUILayout.Button("Add");
            bool cancel = GUILayout.Button("Cancel");

            if (add || cancel)
            {

                Apply();

                if (add)
                    _onCompleteCallback.Invoke(_creator);
                else
                    _onCompleteCallback.Invoke(null);

                Close();
                return;
            }
            else
            {
                Apply();
            }

            //UnityEditor.SerializedProperty P(string fieldName) => SerializedObject.FindProperty(fieldName);
            UnityEditor.SerializedProperty PR(string fieldName) => SerializedObject.FindProperty(nameof(_creator)).FindPropertyRelative(fieldName);

            void DrawP(UnityEditor.SerializedProperty p) => EditorGUILayout.PropertyField(p);

            void FindAndDrawPR(string fieldName)
            {
                SerializedProperty sp = PR(fieldName);
                DrawP(sp);

                Jopa(sp);
            }

            //void FindAndDrawP(string fieldName)
            //{
            //    SerializedProperty sp = P(fieldName);
            //    DrawP(sp);

            //    Jopa(sp);
            //}

            System.Reflection.FieldInfo FI(System.Type t, string fieldName)
                => t.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);


            void Jopa(SerializedProperty sp)
            {
                object statDataCreatorObject = actionID switch
                {
                    0 => _creator.FloatDynamicStatDataCreator,
                    1 => _creator.ClampedFloatStatDataCreator,
                    2 => _creator.FloatModifiableStatDataCreator,
                    3 => _creator.FloatStatDataCreator,
                    4 => _creator.IntDynamicStatDataCreator,
                    5 => _creator.ClampedIntStatDataCreator,
                    6 => _creator.IntModifiableStatDataCreator,
                    7 => _creator.IntStatDataCreator,
                    _ => null,
                };

                if (statDataCreatorObject == null)
                    return;

                statDataCreatorObject.SetField("_statObject", statObject);

                if (isModifiable)
                {
                    var initialModifiersName = "_initialModifiers";
                    var x = sp.FindPropertyRelative(initialModifiersName);


                    if (x == null)
                    {
                        UnityEngine.Debug.LogError("initial modifiers not found but isModifiable");
                        return;
                    }

                    StatModifierCreator[] arr = statDataCreatorObject.GetFieldValue<StatModifierCreator[]>(initialModifiersName);

                    if (arr == null)
                        return;

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


        public static void Open(System.Action<StatDataRuntimeCreator> onCompleteCallback)
        {
            var w = GetWindow<StatDataInitializerWindow>("Stat Data Creator");
            w.SerializedObject = new SerializedObject(w);
            w._onCompleteCallback = onCompleteCallback;
            w._creator = new StatDataRuntimeCreator();
        }
    }

}
#endif
