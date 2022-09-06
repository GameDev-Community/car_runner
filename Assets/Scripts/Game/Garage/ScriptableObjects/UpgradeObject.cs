using DevourDev.Unity.ScriptableObjects;
using Externals.Utils;
using Externals.Utils.Runtime;
using Externals.Utils.StatsSystem;
using Game.Core.Car;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Utils.Attributes;

namespace Game.Garage
{
    // вернул сущность скриптабельного объекта, т.к. один апгрейд вполне может применяться
    // к разным сущностям (машинам)
    // Апгрейд - это АПГРЕЙД, следующая ступень улучшения, так что
    // интегрировать его в предметы (купил икноку - получаешь меньше урона,
    // применимо ко всем машинам) - архитектурно не верно. Предметы должны
    // давать бонусы, не связанные с чем-то другим (например, предыдущими
    // улучшениями, как апгрейды)
    [CreateAssetMenu(menuName = "Garage/Upgrades/Upgrade Object")]
    public class UpgradeObject : GameDatabaseElement
    {
        [System.Serializable]
        public class UpgrageTier
        {
            [SerializeField] private MetaInfo _metaInfo;
            [SerializeField] private Utils.Items.DefualtItemBehaviour _visual;
            [SerializeField, NonReorderable] private StatModifierCreator[] _improves;

            [Tooltip("этот апгрейд не перезаписывает," +
                " а добавляет бонусы к предыдущим бонусам," +
                " кроме бонусов под индексами _excludeTiers")]
            [SerializeField] private bool _includePreviousTiers;
            [Tooltip("индексоси хуй of UpgradeTier (should be" +
                " < this UpgradeTier index) чтобы исключить" +
                " из шахты говна (см пример)")]
            [SerializeField] private int[] _excludingTiers;

            [SerializeField, ReadOnly] private int _index;
            [SerializeField, HideInInspector] private StatModifierCreator[] _allImproves;

            public MetaInfo MetaInfo => _metaInfo;
            public StatModifierCreator[] Improves => _improves;
            public bool IncludePreviousTiers => _includePreviousTiers;
            public int[] ExcludingTiers => _excludingTiers;


            public void ApplyStats(StatsCollection sc)
            {
                if (_allImproves != null && _allImproves.Length > 0)
                {
                    foreach (var item in _allImproves)
                    {
                        item.Apply(sc, false);
                    }

                    return;
                }

                if (_improves != null && _improves.Length > 0)
                {
                    foreach (var item in _improves)
                    {
                        item.Apply(sc, false);
                    }
                }


            }

            public void ApplyVisuals(CustomizableCar customizableCar)
            {
                if(customizableCar == null)
                {
                    DevourRuntimeHelpers.ThrowMessageModal("Null Reference Exception: customizableCar", true);
                }

                if(_visual == null)
                {
                    DevourRuntimeHelpers.ThrowMessageModal("Null Reference Exception: " + customizableCar.name, false);
                    return;

                }
                customizableCar.ChangeItem(_visual);
            }
        }



        [SerializeField] private MetaInfo _metaInfo;
        [Space]
        [SerializeField, NonReorderable] private UpgrageTier[] _tiers;

#if UNITY_EDITOR
        [SerializeField] private bool _computeUpgrades;
#endif


        public MetaInfo MetaInfo => _metaInfo;


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_computeUpgrades)
            {
                _computeUpgrades = false;
                ComputeUpgrades();
            }
        }


        //invoking with Reflection or OnValidate
        private void ComputeUpgrades()
        {
            var tiers = _tiers;
            var c = tiers.Length;

            if (c == 0)
                return;

            var indexFI = typeof(UpgrageTier).GetField("_index", BindingFlags.Instance | BindingFlags.NonPublic);
            var allImprovesFI = typeof(UpgrageTier).GetField("_allImproves", BindingFlags.Instance | BindingFlags.NonPublic);

            for (int i = -1; ++i < c;)
            {
                var x = tiers[i];
                indexFI.SetValue(x, i);

                if (!x.IncludePreviousTiers)
                    continue;

                List<StatModifierCreator> allImprs = new();

                if (x.Improves != null && x.Improves.Length > 0)
                    allImprs.AddRange(x.Improves);

                if (x.ExcludingTiers == null || x.ExcludingTiers.Length == 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        var pt = tiers[j];

                        var ptAllImprs = (StatModifierCreator[])allImprovesFI.GetValue(pt);
                        var ptImprs = pt.IncludePreviousTiers ? ptAllImprs : pt.Improves;

                        allImprs.AddRange(ptImprs);
                    }
                }
                else
                {
                    for (int j = 0; j < i; j++)
                    {
                        var pt = tiers[j];

                        if (x.ExcludingTiers.Contains((int)indexFI.GetValue(pt)))
                            continue;

                        var ptAllImprs = (StatModifierCreator[])allImprovesFI.GetValue(pt);
                        var ptImprs = pt.IncludePreviousTiers ? ptAllImprs : pt.Improves;

                        allImprs.AddRange(ptImprs);
                    }
                }


                allImprovesFI.SetValue(x, allImprs.ToArray());
            }

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        public UpgrageTier GetUpgrageTier(int tier)
        {
            return _tiers[tier];
        }

        /// <param name="tier">add minus to reverse (-1 == ^1)</param>
        /// <param name="upgrageTier"></param>
        /// <returns></returns>
        public bool TryGetUpgrateTier(int tier, out UpgrageTier upgrageTier)
        {
            if (tier < 0)
            {
                tier = -tier;

                if (_tiers.Length < tier)
                {
                    upgrageTier = null;
                    return false;
                }

                upgrageTier = _tiers[^tier];
                return true;
            }

            if (tier >= _tiers.Length)
            {
                upgrageTier = default;
                return false;
            }

            upgrageTier = _tiers[tier];
            return true;
        }


    }
}
