using DevourDev.Unity.ScriptableObjects;
using Externals.Utils;
using Externals.Utils.Runtime;
using Externals.Utils.StatsSystem;
using Game.Core.Car;
using Game.Helpers;
using UnityEngine;

namespace Game.Garage
{
    [CreateAssetMenu(menuName = "Game/Garage/Cars/Car Object")]
    public class CarObject : GameDatabaseElement
    {
        [SerializeField] private MetaInfo _metaInfo;
        [SerializeField] private StatDataRuntimeCreator[] _sourceStats;
        [SerializeField] private CustomizableCar _carCustomizable;
        [SerializeField] private CustomizableCar _readyToGoCarPrefab;
        [SerializeField] private UpgradeObject[] _upgrades;
        [SerializeField] private int _cost;


        public MetaInfo MetaInfo => _metaInfo;


        /// <summary>
        /// строит визуал машины в стоковой
        /// комплектации (без апгрейдов)
        /// </summary>
        /// <returns></returns>
        public GameObject CreateVisualsStock()
        {
            var custCar = GetPreviewModelInstance();

            foreach (var u in _upgrades)
            {
                if (u.TryGetUpgrateTier(0, out var tier))
                {
                    tier.ApplyVisuals(custCar);
                }
            }


            return custCar.gameObject;
        }

        /// <summary>
        /// строит визуал машины в жопе
        /// (с фулл последними апгрейдами)
        /// </summary>
        /// <returns></returns>
        public GameObject CreateVisualsTop()
        {
            var custCar = GetPreviewModelInstance();

            foreach (var u in _upgrades)
            {
                if (u.TryGetUpgrateTier(-1, out var tier))
                {
                    tier.ApplyVisuals(custCar);
                }
            }


            return custCar.gameObject;
        }

        public GameObject CreateVisualsActive()
        {
            var gdata = Accessors.Player.GarageData;

            if (!gdata.AcquiredCars.Contains(this))
            {
                UnityEngine.Debug.LogError(@"— Господин поручик — вы, говорят,
                большой специалист в этих делах. Скажите, вон та дама в рот берет?" + "\n" +
                @"— Которая ? — переспрашивает Ржевский." + "\n" +
                @"— Вон та, в желтом платье." + "\n" +
                @"— Ну, я со спины сказать не могу, — пожимает плечами поручик.
                В этот момент дама поворачивается, поручик Ржевский пристально смотрит ей в лицо и говорит:" + "\n" +
                @"— Эта — берет." + "\n" +
                @"Корнет подходит к даме, они о чем - то беседуют и удаляются.Через некоторое время корнет возвращается, довольный:" + "\n" +
                @"— Действительно, берет! Но как вы определили, поручик ? !" + "\n" +
                @"— Послушайте, корнет, — веско произносит поручик Ржевский, — Рот есть — значит берёт!");
                return CreateVisualsStock();
            }

            var custCar = GetPreviewModelInstance();

            foreach (var u in _upgrades)
            {
                if (u.TryGetUpgrateTier(-1, out var tier))
                {
                    tier.ApplyVisuals(custCar);
                }
            }


            return custCar.gameObject;
        }


        private CustomizableCar GetPreviewModelInstance()
        {
            var inst = Instantiate(_carCustomizable);

            return inst;
        }

        private CustomizableCar GetReadyToGoCarInstance()
        {
            var inst = Instantiate(_readyToGoCarPrefab);
            return inst;
        }


        public GameObject CreateCar()
        {
            var gdata = Accessors.Player.GarageData;

            if (!gdata.AcquiredCars.Contains(this))
            {
                UnityEngine.Debug.Log("attempt to create not acquired car");

                if (UnityEngine.Random.value < 0.5f)
                    throw new System.InvalidOperationException();

                return CreateVisualsTop();
            }

            var custCar = GetReadyToGoCarInstance();
            var sc = Accessors.PlayerStats;

            foreach (var u in _upgrades)
            {
                if (gdata.AcquiredUpgrades.TryGetValue(u, out var v) && v.LastUpgradeTier >= 0)
                {
                    if (u.TryGetUpgrateTier(v.LastUpgradeTier, out var tier))
                    {
                        tier.ApplyVisuals(custCar);
                        tier.ApplyStats(sc);
                    }
#if UNITY_EDITOR
                    else
                    {
                        DevourRuntimeHelpers.ThrowMessageModal("бляяяяяя", true);
                    }
#endif
                }

            }


            return custCar.gameObject;
        }
    }

}
