using DevourDev.Base;
using UnityEngine;

namespace DevourDev.Unity.ScriptableObjects
{
    public abstract class GameDatabaseElement : ScriptableObject
    {
    
        [SerializeField] private int _databaseElementId;

        
        public int DatabaseElementID { get => _databaseElementId; set => _databaseElementId = value; }
    }
}