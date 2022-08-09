using Game.Core;
using UnityEngine;

namespace Game.Interactables
{

    [CreateAssetMenu(menuName = "Game/Interactables/Bonuses/Remove Stat modifier")]
    public class RemoveStatModifierBonus : InteractableBase
    {
       

        [SerializeField] private StatModifierCreator[] _modifiers;

        protected override void HandleInteraction(Racer interactor)
        {
            foreach (var m in _modifiers)
            {
                interactor.RemoveStatModifier(m.StatObject, m.Modifier);
            }
        }
    }


}
