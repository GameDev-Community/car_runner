#if UNITY_EDITOR
using DevourDev.Unity.Utils.Editor;
using DevourDev.Unity.Utils.Editor.Window;
using Externals.Utils;
using Externals.Utils.StatsSystem;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Garage
{
    public class CarCreatorWindow : ExtendedEditorWindow
    {
        [SerializeField] private string _carName;
        [SerializeField] private string _description;
        [SerializeField] private Texture2D _lowresTex;
        [SerializeField] private Texture2D _hiresTex;
        [SerializeField] private List<StatDataRuntimeCreator> _initialStats;

        private CarCreator _creator;


        [MenuItem("Window/Car Creator")]
        public static void Open()
        {
            var w = GetWindow<CarCreatorWindow>("Car creator");
            var creator = new CarCreator();
            w._creator = creator;
            w.SerializedObject = new SerializedObject(w);
            w._initialStats = new();
        }

        private void OnDestroy()
        {
            _creator = null;
        }


        private void OnGUI()
        {
            SerializedObject.FDP(nameof(_carName));
            SerializedObject.FDP(nameof(_description));

            SerializedObject.FDP(nameof(_lowresTex));
            SerializedObject.FDP(nameof(_hiresTex));

            //todo: добавить вертикальный список:
            // (стат: название StatObject, тип StatData) (кнопка: изменить) (кнопка: удалить)
            // (стат: название StatObject, тип StatData) (кнопка: изменить) (кнопка: удалить)
            // (стат: название StatObject, тип StatData) (кнопка: изменить) (кнопка: удалить)
            // (стат: название StatObject, тип StatData) (кнопка: изменить) (кнопка: удалить)

            //todo2: добавить изменение стата выше (а не только в _initialModifiers)

            for (int i = 0; i < _initialStats.Count; i++)
            {
                StatDataRuntimeCreator sd = _initialStats[i];
                GUILayout.BeginHorizontal();

                bool flag = false;

                GUILayout.Label(sd.StatObject.MetaInfo.Name);

                if (GUILayout.Button("change"))
                {
                    flag = true;
                }

                if (GUILayout.Button("remove"))
                {
                    flag = true;

                    _initialStats.RemoveAt(i);
                }

                GUILayout.EndHorizontal();

                if (flag)
                    goto End;
            }

            if (GUILayout.Button("Create Stat"))
            {
                StatDataInitializerWindow.Open(AddStatData);
            }

            if (GUILayout.Button("Create Upgrage"))
            {
                StatDataInitializerWindow.Open(AddStatData);
            }


            if (GUILayout.Button("Create Car Object"))
            {
                var pathToAsset = EditorUtility.SaveFilePanelInProject("Save Car Object", _carName + ".asset", "asset", "Chose new asset location");

                if (pathToAsset.Length > 0)
                {
                    Apply();
                    _creator.MetaInfo = new MetaInfo(_carName, _description, _lowresTex, _hiresTex);
                    _creator.SourceStats = _initialStats.ToArray();
                    var x = _creator.Create(pathToAsset);
                    return;
                }
            }

End:

            Apply();
        }


        private void AddStatData(StatDataRuntimeCreator statDataInitializer)
        {
            if (statDataInitializer == null)
                return;

            _initialStats.Add(statDataInitializer);
            Repaint();
        }
    }
}
#endif