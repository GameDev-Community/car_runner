using Externals.Utils.Extentions;
using Externals.Utils.SaveManager;
using Game.Helpers;
using System.Collections.Generic;
using System.IO;

namespace Game.Garage
{
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
