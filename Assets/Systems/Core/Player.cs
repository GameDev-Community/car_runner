using UnityEngine;

namespace Game.Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private StatsHolder _statsHolder;
        [SerializeField] private Car.CarController _carController;


        public StatsHolder StatsHolder => _statsHolder;
        public Car.CarController CarController => _carController;
    }
}
