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
        [SerializeField] private StatObject _coinsStatObject;

        private static Accessors _inst;
        private static SimpleWallet _wallet;

        public static Player Player => _inst._player;
        public static StatsCollection PlayerStats => Player.StatsHolder.StatsCollection;
        public static CarsDatabase CarsDatabase => _inst._carsDatabase;
        public static UpgradesDatabase UpgradesDatabase => _inst._upgradesDatabase;
        public static SimpleWallet Wallet => _wallet;


        private void Awake()
        {
            _inst = this;
            PlayerStats.TryGetStatDataT<ClampedIntStatData>(_coinsStatObject, out var sd);
            _wallet = new SimpleWallet(sd);
        }

    }



    public class SimpleWallet
    {
        /// <summary>
        /// new balance, delta
        /// </summary>
        public event System.Action<int, int> OnBalanceChanged;

        private readonly ClampedIntStatData _sd;


        public SimpleWallet(ClampedIntStatData sd)
        {
            _sd = sd;
            sd.OnValueChanged += Sd_OnValueChanged;
        }


        public int Balance => _sd.Value;


        private void Sd_OnValueChanged(IValueCallback<int> arg1, int arg2)
        {
            OnBalanceChanged?.Invoke(arg1.Value, arg2);
        }


        public void Earn(int v)
        {
            _sd.AddSafe(v);
        }

        public bool TrySpend(int v)
        {
            if (!CanSpend(v))
                return false;

            return true;
        }

        public bool CanSpend(int v)
        {
            return _sd.CanRemoveExact(v);
        }
    }
}