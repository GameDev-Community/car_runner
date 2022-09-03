using UnityEditor;
using UnityEngine;

namespace DevourDev.Unity.Utils.Editor.Window
{
    public class ExtendedEditorWindow : EditorWindow
    {
        private SerializedObject _serializedObject;
        private SerializedProperty _currentProperty;
        private string _selectedPropertyPath;
        private SerializedProperty _selectedProperty;


        protected SerializedObject SerializedObject { get => _serializedObject; set => _serializedObject = value; }
        protected SerializedProperty CurrentProperty { get => _currentProperty; set => _currentProperty = value; }
        protected SerializedProperty SelectedProperty { get => _selectedProperty; set => _selectedProperty = value; }


        protected void DrawProperties(SerializedProperty prop, bool drawChildren)
        {
            string lastPropPath = string.Empty;

            foreach (SerializedProperty p in prop)
            {
                if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
                {
                    EditorGUILayout.BeginHorizontal();
                    p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);

                    if (p.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        DrawProperties(p, drawChildren);
                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath))
                        continue;

                    lastPropPath = p.propertyPath;
                    EditorGUILayout.PropertyField(p, drawChildren);
                }
            }
        }

        protected void DrawSidebar(SerializedProperty prop)
        {
            foreach (SerializedProperty p in prop)
            {
                if (GUILayout.Button(p.displayName))
                {
                    _selectedPropertyPath = p.propertyPath;
                }
            }

            if (!string.IsNullOrEmpty(_selectedPropertyPath))
            {
                _selectedProperty = _serializedObject.FindProperty(_selectedPropertyPath);
            }
        }

        protected void DrawField(string propName, bool relative)
        {
            if (relative && _currentProperty != null)
            {
                EditorGUILayout.PropertyField(CurrentProperty.FindPropertyRelative(propName), true);
            }
            else if (_serializedObject != null)
            {
                EditorGUILayout.PropertyField(_serializedObject.FindProperty(propName), true);
            }
        }

        protected void DrawSelectedPropertiesPanel(params string[] propsNames)
        {
            CurrentProperty = SelectedProperty;
            EditorGUILayout.BeginHorizontal("box");

            foreach (var pn in propsNames)
                DrawField(pn, true);

            EditorGUILayout.EndHorizontal();
        }

        protected void Apply()
        {
            _serializedObject.ApplyModifiedProperties();
        }

        protected UnityEditor.SerializedProperty P(string fieldName) => SerializedObject.FindProperty(fieldName);

        protected void FindAndDrawP(string fieldName)
        {
            SerializedProperty sp = P(fieldName);
            DrawP(sp);
        }

        protected void DrawP(UnityEditor.SerializedProperty p) => EditorGUILayout.PropertyField(p);
    }
}
