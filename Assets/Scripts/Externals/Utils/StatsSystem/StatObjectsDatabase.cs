using DevourDev.Unity.ScriptableObjects;
using UnityEngine;

namespace Externals.Utils.StatsSystem
{
    [CreateAssetMenu(menuName = "Stats System/Database")]
    public class StatObjectsDatabase : GameDatabase<StatObject>
    {
        private void OnValidate()
        {
            ManageElementsOnValidate();
        }
    }

}
