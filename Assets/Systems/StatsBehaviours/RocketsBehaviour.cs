using Game.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Game.StatsBehaviours
{
    public class RocketsBehaviour : StatBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private RocketMissile[] _rocketsPrefabs;
        [SerializeField] private Transform _rocketsSpawnPoint;
        //?
        [SerializeField] private UnityEvent _onRocketLaunched; //?

        [SerializeField] TMPro.TextMeshProUGUI _rocketsTex;


        protected override void Start()
        {
            base.Start();

            HandleChangedRocketsValue(StatData, 0, 0);
            StatData.OnValueChanged += HandleChangedRocketsValue;
        }

        private void HandleChangedRocketsValue(DevourDev.Unity.Utils.SimpleStats.IModifiableStatData statData, float dirtyDelta, float safeDelta)
        {
            _rocketsTex.text = $"{StatObject.StatName}: {Mathf.RoundToInt(statData.Value)}";
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
                LaunchRocket();
        }


        public void LaunchRocket()
        {
            var stats = _player.StatsHolder.Stats;

            if (!stats.TryGetStatData(StatObject, out var sdata))
            {
                throw new System.Collections.Generic.KeyNotFoundException(nameof(StatObject));
            }

            var rv = Mathf.RoundToInt(sdata.Value);

            if (rv < 1)
                return;

            float endV = rv - 1;

            sdata.ChangeValue(endV - sdata.Value);

            var l = _rocketsPrefabs.Length;

            if (l == 0)
            {
                Debug.Log("no rockets prefab");
                return;
            }

            var ri = UnityEngine.Random.Range(0, l);
            var r = Instantiate(_rocketsPrefabs[ri], _rocketsSpawnPoint.position,
                _rocketsSpawnPoint.rotation, null);

            var tr = _player.transform;
            r.Init(tr.position + tr.forward * 10);
            r.Speed += _player.CarController.Speed;

        }

    }
}
