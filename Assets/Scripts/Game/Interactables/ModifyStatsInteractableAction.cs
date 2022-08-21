using Game.Core;
using UnityEngine;

namespace Game.Interactables
{
    public class ModifyStatsInteractableAction : InteractableActionBase
    {
        [SerializeField] private ExtendedModifiersApplier _extendedModifiersApplier;


        public override void Act(Player player)
        {
            _extendedModifiersApplier.Apply(player.StatsHolder.StatsCollection);
        }
    }
}