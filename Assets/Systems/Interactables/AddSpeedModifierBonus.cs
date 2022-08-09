using Game.Core;
using Game.Stats;
using UnityEngine;

namespace Game.Interactables
{
    [CreateAssetMenu(menuName = "Game/Interactables/Bonuses/Add speed modifier")]
    public class AddSpeedModifierBonus : InteractableBase
    {
        [System.Serializable]
        private struct Creator
        {
            public StatObject StatObject;
            public StatModifier Modifier;
            public bool Temp;
            public float Duration;


            public static explicit operator StatModifierCreator(Creator x)
            {
                return new StatModifierCreator(x.StatObject, x.Modifier);
            }
        }


        [SerializeField, NonReorderable] private Creator[] _creators;


        protected override void HandleInteraction(Racer interactor)
        {
            foreach (var c in _creators)
            {
                interactor.AddStatModifier(c.StatObject, c.Modifier);

                if (c.Temp)
                {
                    var x = Instantiate(Accessors.DelayedModifierChangerPrefab, interactor.transform);
                    x.Init(interactor, false, c.Duration, (StatModifierCreator)c);
                }
            }
        }
    }


}
