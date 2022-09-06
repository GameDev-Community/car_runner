using Externals.Utils.Extentions;
using Externals.Utils.SaveManager;
using Game.Helpers;
using System.Collections.Generic;
using System.IO;

namespace Game.Garage
{
    public class GarageData : ISavable
    {
        private class UpgradeData : ISavable
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


        private readonly List<CarObject> _unlockedCars;
        private readonly List<CarObject> _acquiredCars;
        private readonly List<UpgradeData> _upgrades;


        public GarageData()
        {
            _unlockedCars = new();
            _acquiredCars = new();
            _upgrades = new();
        }


        public List<CarObject> UnlockedCars => _unlockedCars;
        public List<CarObject> AcquiredCars => _acquiredCars;


        public void Save(BinaryWriter bw)
        {
            bw.WriteGameDatabaseElements(_unlockedCars);
            bw.WriteGameDatabaseElements(_acquiredCars);

        }

        public void Load(BinaryReader br)
        {
            var cdb = Accessors.CarsDatabase;
            br.ReadGameDatabaseElementsNonAlloc<CarObject>(cdb, _unlockedCars);
            br.ReadGameDatabaseElementsNonAlloc<CarObject>(cdb, _acquiredCars);
            br.ReadSavablesNonAlloc<UpgradeData>(_upgrades);
        }
    }
}
