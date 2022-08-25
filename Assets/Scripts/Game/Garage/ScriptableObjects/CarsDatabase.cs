using DevourDev.Unity.ScriptableObjects;
using UnityEngine;

namespace Game.Garage
{
    [CreateAssetMenu(menuName = "Game/Garage/Cars/Database")]
    public class CarsDatabase : GameDatabase<CarObject>
    {
#if UNITY_EDITOR
        private void OnValidate()
        {
            ManageElementsOnValidate();
        }
#endif
    }
}
