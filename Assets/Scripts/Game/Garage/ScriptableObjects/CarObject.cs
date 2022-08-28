using DevourDev.Unity.ScriptableObjects;
using Externals.Utils;
using Externals.Utils.StatsSystem;
using UnityEngine;

namespace Game.Garage
{

    [CreateAssetMenu(menuName = "Game/Garage/Cars/Car Object")]
    public class CarObject : GameDatabaseElement
    {
        [SerializeField] private MetaInfo _metaInfo;
        [SerializeField] private CarSourceStats _sourceStats;
        [SerializeField] private GameObject _carPreviewPrefab;
        [SerializeField] private UpgradeObject[] _upgrades;

        public MetaInfo MetaInfo => _metaInfo;
    }

    /// <summary>
    /// характеристики машины БЕЗ апгрейдов
    /// (даже базовых).
    /// initial/max - НАЧАЛЬНОЕ максимальное
    /// значение динамического стата и МАКСИМАЛЬНО
    /// ВОЗМОЖНОЕ максимальное значение дин. стата.
    /// </summary>
    [System.Serializable]
    public struct CarSourceStats
    {
        [Tooltip("initial/max")]
        public Vector2 Health;
        [Tooltip("initial/max")]
        public Vector2 Acceleration;
        [Tooltip("initial/max")]
        public Vector2 Speed;

        [Tooltip("Сила тачилы. Чем меньше" +
            " проигрыш силы тачилы силе препятствия," +
            " тем меньше негативных влияний она получает," +
            " вплоть до полного отсутствия эффекта.")]
        public float Power;
    }
}
