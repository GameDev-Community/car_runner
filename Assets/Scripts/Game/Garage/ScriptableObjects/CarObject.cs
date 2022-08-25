using DevourDev.Unity.ScriptableObjects;
using Externals.Utils;
using Externals.Utils.StatsSystem;
using UnityEngine;

namespace Game.Garage
{

    [CreateAssetMenu(menuName = "Game/Garage/Cars/Upgrades/Upgrade Object")]
    public class UpgradeObject : GameDatabaseElement
    {
        [System.Serializable]
        public struct UpgrageTier
        {
            public MetaInfo MetaInfo;

            //public CustomActionTest[] _improves;
        }

        [SerializeField] private MetaInfo _metaInfo;
        [Space]
        [SerializeField] private UpgrageTier[] _tiers;


        public MetaInfo MetaInfo => _metaInfo;


        public UpgrageTier GetUpgrageTier(int tier)
        {
            return _tiers[tier];
        }


        public bool TryGetUpgrateTier(int tier, out UpgrageTier upgrageTier)
        {
            if (tier < 0 || tier >= _tiers.Length)
            {
                upgrageTier = default;
                return false;
            }

            upgrageTier = GetUpgrageTier(tier);
            return true;
        }
    }

    [CreateAssetMenu(menuName = "Game/Garage/Cars/Car Object")]
    public class CarObject : GameDatabaseElement
    {
        [SerializeField] private MetaInfo _metaInfo;
        [SerializeField] private CarSourceStats _sourceStats;
        [SerializeField] UpgradeObject[] _upgrades;

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
