using Game.Core;
using UnityEngine;

namespace Game.Interactables
{
    [CreateAssetMenu(menuName = "Game/Interactables/Bonuses/Add speed modifier")]
    public class AddSpeedModifierBonus : InteractableBase
    {
        [System.Serializable]
        private struct Creator
        {
            public SpeedModifier Modifier;
            public bool Temp;
            public float Duration;
        }


        [SerializeField, NonReorderable] private Creator[] _creators;


        protected override void HandleInteraction(Racer interactor)
        {
            foreach (var c in _creators)
            {
                interactor.AddSpeedModifier(c.Modifier);

                if (c.Temp)
                {
                    var x = Instantiate(Accessors.DelayedModifierChangerPrefab, interactor.transform);
                    x.Init(interactor, true, c.Duration, c.Modifier);
                }
            }
        }
    }


}
