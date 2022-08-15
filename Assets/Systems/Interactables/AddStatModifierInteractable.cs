using DevourDev.Unity.Utils.SimpleStats.Modifiers;
using Game.Core;
using UnityEngine;

namespace Game.Interactables
{
    [CreateAssetMenu(menuName = "Game/Interactables/Stats Manipulating/Add Stat Modifier")]
    public class AddStatModifierInteractable : InteractableBase
    {
        [System.Serializable]
        private struct Creator
        {
            public StatModifierCreator InternalCreator;
            public bool Temp;
            public float Duration;
        }


        [SerializeField, NonReorderable] private Creator[] _creators;


        protected override void HandleInteraction(Player player)
        {
            var stats = player.StatsHolder.Stats;

            foreach (var c in _creators)
            {
                stats.AddModifier(c.InternalCreator);

                if (c.Temp)
                {
                    var x = Instantiate(Accessors.DelayedModifierChangerPrefab);
                    x.Init(player, false, c.Duration, c.InternalCreator);
                }
            }

            stats.FinishAddingModifiers();
        }
    }
}
