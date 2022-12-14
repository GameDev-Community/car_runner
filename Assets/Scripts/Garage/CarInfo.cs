using Externals.Utils.StatsSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Garage
{
    [System.Obsolete("moved to CarInfo struct", false)]
    [CreateAssetMenu(menuName = "Cars/CarInfo")]
    internal sealed class CarInfo : ScriptableObject
    {
        public string Name;
        public float Speed;
        public float Health;
        public float Acceleration;
        public int Price;
        public GameObject CarPrefab;
        public CarUpgradeElement[] CarUpgradeElements;
        public StatModifierCreator[] StartModifiers; // ???????? ?? ????? ??????????? ?????????, ???? ????? //ne nuzhno
    }
}