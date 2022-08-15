using DevourDev.Unity.Utils.SimpleStats.Modifiers;
using Game.Core;
using UnityEngine;

namespace Game.Interactables
{
    /// <summary>
    /// Убирает лишь модификаторы, удовлетворяющие
    /// Equals. Если ранее добавленный модификатор
    /// создавался со случайным разбросом, то следует
    /// либо добавлять модификатор вместе с Отложенным
    /// Удалителем, либо использовать Remove Variable 
    /// Stat Modifier
    /// </summary>
    [CreateAssetMenu(menuName = "Game/Interactables/Stats Manipulating/Remove Exact Stat modifiers")]
    public class RemoveExactStatModifierInteractable : InteractableBase
    {
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
