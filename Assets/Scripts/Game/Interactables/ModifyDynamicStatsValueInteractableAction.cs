using Externals.Utils.StatsSystem;
using Game.Core;
using UnityEngine;

namespace Game.Interactables
{
    public class ModifyDynamicStatsValueInteractableAction : InteractableActionBase
    {
        [SerializeField] DynamicStatValueChanger<float>[] _changeDynamicFloats;
        [SerializeField] DynamicStatValueChanger<int>[] _changeDynamicIntegers;


        public override void Act(Player player)
        {
            if (_changeDynamicFloats != null || _changeDynamicFloats.Length > 0)
            {
                foreach (var cdf in _changeDynamicFloats)
                {
                    cdf.Apply(player.StatsHolder.StatsCollection);
                }
            }

            if (_changeDynamicIntegers != null || _changeDynamicIntegers.Length > 0)
            {
                foreach (var cdi in _changeDynamicIntegers)
                {
                    cdi.Apply(player.StatsHolder.StatsCollection);
                }
            }
        }
    }
}