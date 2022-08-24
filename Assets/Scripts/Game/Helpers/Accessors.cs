using Externals.Utils.StatsSystem;
using Game.Core;
using UnityEngine;

namespace Game.Helpers
{
    public class Accessors : MonoBehaviour
    {

        [SerializeField] private Player _player;

        private static Accessors _inst;


        public static Player Player => _inst._player;
        public static StatsCollection PlayerStats => Player.StatsHolder.StatsCollection;


        private void Awake()
        {
            _inst = this;
        }

    }
}