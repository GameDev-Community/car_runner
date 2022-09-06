using Externals.Utils.Extentions;
using Externals.Utils.Runtime;
using Externals.Utils.SaveManager;
using Game.Helpers;
using System.Collections.Generic;
using System.IO;

namespace Game.Garage
{
    public class UpgradeData : ISavable
    {
        private UpgradeObject _upgradeObject;
        private int _lastUpgradeTier;


        public UpgradeData()
        {
        }

        public UpgradeData(UpgradeObject upgradeObject, int lastUpgradeTier)
        {
            _upgradeObject = upgradeObject;
            _lastUpgradeTier = lastUpgradeTier;
        }


        public UpgradeObject UpgradeObject => _upgradeObject;
        public int LastUpgradeTier { get => _lastUpgradeTier; set => _lastUpgradeTier = value; }


        public void Load(BinaryReader br)
        {
            _upgradeObject = br.ReadGameDatabaseElement(Accessors.UpgradesDatabase);
            _lastUpgradeTier = br.ReadInt32();

        }

        public void Save(BinaryWriter bw)
        {
            bw.WriteGameDatabaseElement(_upgradeObject);
            bw.Write(_lastUpgradeTier);
        }
    }

    public class GarageData : ISavable
    {
        private readonly HashSet<CarObject> _unlockedCars;
        private readonly HashSet<CarObject> _acquiredCars;
        private readonly Dictionary<UpgradeObject, UpgradeData> _aquiredUpgrades;


        public GarageData()
        {
            _unlockedCars = new();
            _acquiredCars = new();
            _aquiredUpgrades = new();
        }


        public HashSet<CarObject> UnlockedCars => _unlockedCars;
        public HashSet<CarObject> AcquiredCars => _acquiredCars;
        public Dictionary<UpgradeObject, UpgradeData> AcquiredUpgrades => _aquiredUpgrades;


        public void Save(BinaryWriter bw)
        {
            bw.WriteGameDatabaseElements(_unlockedCars);
            bw.WriteGameDatabaseElements(_acquiredCars);
            bw.WriteSavablesT(_aquiredUpgrades.Values);
        }

        public void Load(BinaryReader br)
        {
            var cdb = Accessors.CarsDatabase;
            br.ReadGameDatabaseElementsNonAllocToCollection<CarObject>(cdb, _unlockedCars);
            br.ReadGameDatabaseElementsNonAllocToCollection<CarObject>(cdb, _acquiredCars);
            br.ReadSavablesNonAllocToRentedBuffer<UpgradeData>(out var pool, out var arr, out var c);

            _aquiredUpgrades.EnsureCapacity(c);
            var au = _aquiredUpgrades;

            for (int i = -1; ++i < c;)
            {
                var item = arr[i];
                au.Add(item.UpgradeObject, item);
            }
        }

    }
}
