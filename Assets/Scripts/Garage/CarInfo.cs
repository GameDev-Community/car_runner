using Externals.Utils.StatsSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Garage
{
    [CreateAssetMenu(menuName = "Cars/CarInfo")]
    internal sealed class CarInfo : ScriptableObject
    {
        public string Name;
        public float Speed;
        public float Health;
        public float Acceleration;
        public GameObject CarPrefab;
        public CarUpgradeElement[] CarUpgradeElements;
        public StatModifierCreator[] StartModifiers; // заменить на более примитивную структуру, если нужно
    }
}