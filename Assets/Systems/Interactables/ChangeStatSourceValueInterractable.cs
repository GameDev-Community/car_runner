using DevourDev.Unity.Utils.SimpleStats;
using Game.Core;
using UnityEngine;

namespace Game.Interactables
{
    [CreateAssetMenu(menuName = "Game/Interactables/Stats Manipulating/Change Stat Data")]
    public class ChangeStatSourceValueInterractable : InteractableBase
    {
        [SerializeField] private StatObject _stat;
        [SerializeField] private float _delta;


        protected override void HandleInteraction(Player player)
        {
            var stats = player.StatsHolder.Stats;

            if (stats.TryGetStatData(_stat, out var v))
            {
                v.ChangeValue(_delta);
            }
        }
    }


}
