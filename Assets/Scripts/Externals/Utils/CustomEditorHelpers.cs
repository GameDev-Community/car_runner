#if UNITY_EDITOR
using UnityEditor;

namespace DevourDev.Unity.Utils.Editor
{
    public static class CustomEditorHelpers
    {
        public static SerializedProperty FP(this SerializedObject source, string propName)
        {
            return source.FindProperty(propName);
        }

        public static void FDP(this SerializedObject source, string propName, bool includeChildren = true)
        {
            var p = source.FP(propName);
            EditorGUILayout.PropertyField(p, includeChildren);
        }
    }
}
#endif