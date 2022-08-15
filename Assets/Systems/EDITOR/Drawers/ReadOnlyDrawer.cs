using UnityEditor;
using UnityEngine;
using Utils.Attributes;

namespace EDITOR.Drawers
{
    //срисовано отсюда: https://www.patrykgalach.com/2020/01/27/assigning-interface-in-unity-inspector/
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
#if UNITY_EDITOR
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var previousGUIState = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = previousGUIState;
        }
#endif


    }
}