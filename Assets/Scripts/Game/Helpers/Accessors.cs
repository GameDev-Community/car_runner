using Externals.Utils.StatsSystem;
using Game.Core;
using Game.Garage;
using UnityEngine;

namespace Game.Helpers
{
    public class Accessors : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private CarsDatabase _carsDatabase;
        [SerializeField] private UpgradesDatabase _upgradesDatabase;

        private static Accessors _inst;


        public static Player Player => _inst._player;
        public static StatsCollection PlayerStats => Player.StatsHolder.StatsCollection;
        public static CarsDatabase CarsDatabase => _inst._carsDatabase;
        public static UpgradesDatabase UpgradesDatabase => _inst._upgradesDatabase;


        private void Awake()
        {
            _inst = this;
        }

    }
}