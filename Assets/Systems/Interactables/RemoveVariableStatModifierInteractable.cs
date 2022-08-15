using DevourDev.Unity.Utils.SimpleStats.Modifiers;
using Game.Core;
using UnityEngine;

namespace Game.Interactables
{
    [System.Obsolete("WIP", true)]
    [CreateAssetMenu(menuName = "Game/Interactables/Stats Manipulating/Remove Variable Stat modifiers")]
    public class RemoveVariableStatModifierInteractable : InteractableBase
    {
        [System.Serializable]
        public class StatModifierPredicateCreator
        {

        }


        [SerializeField] private StatModifierCreator[] _modifiers;


        protected override void HandleInteraction(Player player)
        {
            var stats = player.StatsHolder.Stats;

            foreach (var m in _modifiers)
            {
                stats.RemoveModifier(m);
            }

            stats.FinishRemovingModifiers();
        }
    }

}
