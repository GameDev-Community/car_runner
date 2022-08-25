using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DevourDev.Unity.Utils
{
    public static class EditorHelpers
    {
#if UNITY_EDITOR
        public static List<Type> GetInheritedClasses(Type type, Assembly[] assemblies, bool includingSourceIfNotAbstract = false)
        {
            List<Type> types = new();

            if (assemblies == null)
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }

            if (includingSourceIfNotAbstract && !type.IsAbstract)
                types.Add(type);

            foreach (var assembly in assemblies)
            {
                var s = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(type));
                types.AddRange(s);
            }

            return types;
        }

        public static List<T> FindAssets<T>() where T : UnityEngine.Object
        {
            List<T> found = new();

            string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T)}");

            for (int i = 0; i < guids.Length; i++)
            {
                string g = guids[i];
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(g);
                var element = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                found.Add(element);
            }

            return found;
        }

        public static List<T> FindAssetsIncludingSubclasses<T>(Assembly[] assemblies) where T : UnityEngine.Object
        {
            List<T> found = new();
            List<string> allGuids = new();

            var inheriteds = GetInheritedClasses(typeof(T), assemblies, true);

            foreach (var inh in inheriteds)
            {
                allGuids.AddRange(UnityEditor.AssetDatabase.FindAssets($"t:{inh}"));
            }

            for (int i = 0; i < allGuids.Count; i++)
            {
                string g = allGuids[i];
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(g);
                var element = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                found.Add(element);
            }

            return found;
        }
#endif
    }
}
