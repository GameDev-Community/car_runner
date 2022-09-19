#if UNITY_EDITOR
using DevourDev.Base.Reflections;
using DevourDev.Unity.Utils.Editor.Window;
using Externals.Utils;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Garage
{
    public class UpgradeTierCreatorWindow : ExtendedEditorWindow
    {
        //[SerializeField] private MetaInfo _metaInfo;
        [SerializeField] private UpgradeObject.UpgrageTier _tier;
        private System.Action<UpgradeObject.UpgrageTier> _onCompleteCallback;
        //private SerializedProperty _metaInfo_sp;
        private SerializedProperty _tier_sp;


        public static void Open(System.Action<UpgradeObject.UpgrageTier> onCompleteCallback)
        {
            var w = GetWindow<UpgradeTierCreatorWindow>("Upgrade Creator");
            w.SerializedObject = new SerializedObject(w);
            w._onCompleteCallback = onCompleteCallback;
            //w._metaInfo = new("New Tier", "", null, null);
            w._tier = new UpgradeObject.UpgrageTier();
            //w._metaInfo_sp = w.SerializedObject.FindProperty(nameof(_metaInfo));
            w._tier_sp = w.SerializedObject.FindProperty(nameof(_tier));

        }


        private void OnGUI()
        {
            SerializedObject.Update();

            //DrawP(_metaInfo_sp);
            DrawP(_tier_sp);



            bool add = GUILayout.Button("Add");
            bool cancel = GUILayout.Button("Cancel");

            if (add || cancel)
            {

                Apply();

                if (add)
                    _onCompleteCallback.Invoke(_tier);
                else
                    _onCompleteCallback.Invoke(null);

                Close();
                return;
            }
            else
            {
                Apply();
            }

        }
    }

    public class UpgradeCreatorWindow : ExtendedEditorWindow
    {
        [SerializeField] private MetaInfo _metaInfo;
        private System.Action<UpgradeObject> _onCompleteCallback;

        private List<UpgradeObject.UpgrageTier> _tiers;
        private SerializedProperty _metaInfo_sp;

        public static void Open(System.Action<UpgradeObject> onCompleteCallback)
        {
            var w = GetWindow<UpgradeCreatorWindow>("Upgrade Creator");
            w.SerializedObject = new SerializedObject(w);
            w._onCompleteCallback = onCompleteCallback;
            w._metaInfo = new("New Upgrade", "", null, null);
            w._tiers = new();
            w._metaInfo_sp = w.SerializedObject.FindProperty(nameof(_metaInfo));
        }

        private void OnGUI()
        {
            SerializedObject.Update();

            DrawP(_metaInfo_sp);

            var arr = _tiers;
            int tiersC = arr.Count;

            for (int i = -1; ++i < tiersC;)
            {
                var t = arr[i];
                bool endFlag = false;

                GUILayout.BeginHorizontal();


                var mi = t.MetaInfo;

                if (mi != null)
                {
                    var n = mi.Name;

                    if (n != null)
                    {
                        GUILayout.Label(n);
                        goto Buttons;
                    }
                }

                GUILayout.Label($"tier #{i}");

Buttons:
                if (GUILayout.Button($"change (not impl)"))
                {
                    UnityEngine.Debug.Log("not implemented");
                    endFlag = true;
                }

                if (!endFlag && GUILayout.Button("remove"))
                {
                    endFlag = true;
                    arr.RemoveAt(i);
                }

                GUILayout.EndHorizontal();

                if (endFlag)
                    goto End;
            }


            if (GUILayout.Button("Add Upgrade Tier"))
            {
                AddUpgradeTier();

                goto End;
            }



            bool add = GUILayout.Button("OK");
            bool cancel = GUILayout.Button("Cancel");

            if (add || cancel)
            {

                Apply();

                if (add)
                    _onCompleteCallback.Invoke(CreateUpgradeData());
                else
                    _onCompleteCallback.Invoke(null);

                Close();
                return;
            }

End:

            Apply();
        }

        private UpgradeObject CreateUpgradeData()
        {
            var upgData = ScriptableObject.CreateInstance<UpgradeObject>();
            upgData.SetField("_metaInfo", _metaInfo);
            upgData.SetField("_tiers", _tiers.ToArray());
            upgData.InvokeMethod("ComputeUpgrades", null);
            return upgData;
        }

        private void AddUpgradeTier()
        {
            UpgradeTierCreatorWindow.Open(AddTier);
        }

        private void AddTier(UpgradeObject.UpgrageTier tier)
        {
            if (tier == null)
                return;

            _tiers.Add(tier);
            Repaint();
        }
    }

}
#endif
