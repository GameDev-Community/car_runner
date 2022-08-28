using DevourDev.Base;
using UnityEngine;
using Utils.Attributes;

namespace DevourDev.Unity.ScriptableObjects
{
    public abstract class GameDatabaseElement : ScriptableObject
    {
    
        [SerializeField, ReadOnly] private int _databaseElementId;

        
        public int DatabaseElementID { get => _databaseElementId; set => _databaseElementId = value; }
    }
}