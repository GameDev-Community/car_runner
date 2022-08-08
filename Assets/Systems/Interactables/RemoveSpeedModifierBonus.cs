using Game.Core;
using UnityEngine;

namespace Game.Interactables
{
    [CreateAssetMenu(menuName = "Game/Interactables/Bonuses/Remove speed modifier")]
    public class RemoveSpeedModifierBonus : InteractableBase
    {
        [SerializeField] private SpeedModifier[] _modifiers;

        protected override void HandleInteraction(Racer interactor)
        {
            foreach (var m in _modifiers)
            {
                interactor.RemoveSpeedModifier(m);
            }
        }
    }


}
