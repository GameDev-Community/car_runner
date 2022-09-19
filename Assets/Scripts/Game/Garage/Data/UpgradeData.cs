using Externals.Utils.Extentions;
using Externals.Utils.SaveManager;
using Game.Helpers;
using System.IO;

namespace Game.Garage
{
    /// <summary>
    /// Upgrades serializing helper
    /// </summary>
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
}
