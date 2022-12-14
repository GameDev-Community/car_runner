using DevourDev.Base.Reflections;
using Externals.Utils.Extentions;
using Externals.Utils.SaveManager;
using Game.Helpers;
using System.Collections.Generic;
using System.IO;

namespace Game.Garage
{

    /// <summary>
    /// всё, что касается машин
    /// </summary>
    public class GarageData : ISavable
    {
        private readonly HashSet<CarObject> _unlockedCars;
        private readonly HashSet<CarObject> _acquiredCars;
        private readonly Dictionary<UpgradeObject, UpgradeData> _aquiredUpgrades;
        private CarObject _activeCar;


        public GarageData()
        {
            _unlockedCars = new();
            _acquiredCars = new();
            _aquiredUpgrades = new();
            _activeCar = null;
        }


        public HashSet<CarObject> UnlockedCars => _unlockedCars;
        public HashSet<CarObject> AcquiredCars => _acquiredCars;
        public Dictionary<UpgradeObject, UpgradeData> AcquiredUpgrades => _aquiredUpgrades;

        public CarObject ActiveCar { get => _activeCar; set => _activeCar = value; }


        public void Save(BinaryWriter bw)
        {
            bw.WriteGameDatabaseElements(_unlockedCars);
            bw.WriteGameDatabaseElements(_acquiredCars);
            bw.WriteSavablesT(_aquiredUpgrades.Values);
            bw.WriteGameDatabaseElement(_activeCar);
        }

        public void Load(BinaryReader br)
        {
            var cdb = Accessors.CarsDatabase;
            br.ReadGameDatabaseElementsNonAllocToCollection(cdb, _unlockedCars);
            br.ReadGameDatabaseElementsNonAllocToCollection(cdb, _acquiredCars);
            br.ReadSavablesNonAllocToRentedBuffer<UpgradeData>(out var pool, out var arr, out var c);

            _aquiredUpgrades.EnsureCapacity(c);
            var au = _aquiredUpgrades;

            for (int i = -1; ++i < c;)
            {
                var item = arr[i];
                au.Add(item.UpgradeObject, item);
            }

            _activeCar = br.ReadGameDatabaseElement(cdb);
        }

    }
}
