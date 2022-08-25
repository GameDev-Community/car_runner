using DevourDev.Unity.ScriptableObjects;
using UnityEngine;

namespace Game.Garage
{
    [CreateAssetMenu(menuName = "Game/Garage/Cars/Upgrades/Database")]
    public class UpgradesDatabase : GameDatabase<UpgradeObject>
    {
#if UNITY_EDITOR
        private void OnValidate()
        {
            ManageElementsOnValidate();
        }
#endif
    }
}
