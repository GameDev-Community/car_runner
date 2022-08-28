using DevourDev.Unity.ScriptableObjects;
using Externals.Utils;
using Externals.Utils.Extentions;
using Externals.Utils.SaveManager;
using Externals.Utils.StatsSystem;
using Game.Helpers;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game.Garage
{
    [CreateAssetMenu(menuName = "Game/Garage/Cars/Upgrades/Upgrade Object")]
    public class UpgradeObject : GameDatabaseElement
    {
        [System.Serializable]
        public struct UpgrageTier
        {
            public MetaInfo MetaInfo;

            public StatModifierCreator[] Improves;
            [Tooltip("этот апгрейд не перезаписывает," +
                " а добавляет бонусы к предыдущим бонусам," +
                " кроме бонусов под индексами _excludeTiers")]
            public bool IncludePreviousTiers;
            [Tooltip("индексоси хуй of UpgradeTier (should be" +
                " < this UpgradeTier index) чтобы исключить" +
                " из шахты говна (см пример)")]
            public int[] ExcludingTiers;
        }


        [SerializeField] private MetaInfo _metaInfo;
        [Space]
        [SerializeField] private UpgrageTier[] _tiers;


        public MetaInfo MetaInfo => _metaInfo;


        public UpgrageTier GetUpgrageTier(int tier)
        {
            return _tiers[tier];
        }


        public bool TryGetUpgrateTier(int tier, out UpgrageTier upgrageTier)
        {
            if (tier < 0 || tier >= _tiers.Length)
            {
                upgrageTier = default;
                return false;
            }

            upgrageTier = GetUpgrageTier(tier);
            return true;
        }


        /// <summary>
        /// !!! применять только на тмп коллекции !!!
        /// </summary>
        public static void ApplyUpgrages(UpgrageTier tier, StatsCollection sc)
        {
            if (tier.IncludePreviousTiers && (tier.ExcludingTiers == null || tier.ExcludingTiers.Length == 0))
            {
                foreach (var item in tier.Improves)
                {
                    item.Apply(sc);
                }
            }
            else
            {

            }
        }
    }


    public class GarageData : ISavable<GarageData>
    {
        private readonly List<CarObject> _unlockedCars;
        private readonly List<CarObject> _acquiredCars;


        private GarageData(IEnumerable<CarObject> unlockedCars, IEnumerable<CarObject> acquiredCars)
        {
            _unlockedCars = new(unlockedCars);
            _acquiredCars = new(acquiredCars);
        }


        public GarageData()
        {
            _unlockedCars = new();
            _acquiredCars = new();
        }


        public List<CarObject> UnlockedCars => _unlockedCars;
        public List<CarObject> AcquiredCars => _acquiredCars;


        public void Save(BinaryWriter bw)
        {
            bw.WriteGameDatabaseElements(_unlockedCars);
            bw.WriteGameDatabaseElements(_acquiredCars);
        }

        public GarageData Load(BinaryReader br)
        {
            var cdb = Accessors.CarsDatabase;
            return new GarageData(br.ReadGameDatabaseElements<CarObject>(cdb),
                br.ReadGameDatabaseElements<CarObject>(cdb));
        }
    }
}
