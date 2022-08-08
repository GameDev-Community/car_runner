using Game.Core;
using Game.Stats;
using UnityEngine;
using Utils;

namespace Game.Interactables
{
    [CreateAssetMenu(menuName = "Game/Interactables/Bonuses/Change Stat Data")]
    public class StatBonus : InteractableBase
    {
        [SerializeField] private StatObject _stat;
        [SerializeField] private DynamicValue _delta;


        protected override void HandleInteraction(Racer interactor)
        {
            if (interactor.TryGetStatData(_stat, out var v))
            {
                v.Change(_delta);
            }
        }
    }

}
