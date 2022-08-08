using Game.Core;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Game.StatsBehaviours
{
    public class RocketsBehaviour : StatBehaviour
    {
        [SerializeField] private RocketMissile[] _rocketsPrefabs;
        [SerializeField] private Transform _rocketsSpawnPoint;
        //?
        [SerializeField] private UnityEvent _onRocketLaunched; //?

        [SerializeField] TMPro.TextMeshProUGUI _rocketsTex;


        private void Start()
        {
            if (Racer.TryGetStatData(StatObject, out var icv)
                && icv is ClampedInt cint)
            {
                cint.OnValueChanged += Cint_OnValueChanged;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
                LaunchRocket();
        }


        public void LaunchRocket()
        {
            var l = _rocketsPrefabs.Length;

            if (l == 0)
            {
                Debug.Log("no rockets prefab");
                return;
            }

            var ri = UnityEngine.Random.Range(0, l);
            var r = Instantiate(_rocketsPrefabs[ri], _rocketsSpawnPoint.position,
                _rocketsSpawnPoint.rotation, null);

            var tr = Racer.transform;
            r.Init(tr.position + tr.forward * 10);
        }

        private void Cint_OnValueChanged(ClampedInt se, int dd, int sd)
        {
            _rocketsTex.text = $"{StatObject.StatName}: {se.Value}";
        }
    }
}
