using Externals.Utils.StatsSystem;
using Game.Core;
using UnityEngine;

namespace Game.Interactables
{
    public class ChangeStatAmountInteractableAction : InteractableActionBase
    {
        [Header("Clampeds")]
        [SerializeField, NonReorderable] ClampedAmountManipulator<float>[] _changeClampedAmountManipalatableFloats;
        [SerializeField, NonReorderable] ClampedAmountManipulator<int>[] _changeClampedAmountManipalatableIntegers;
        [Space]
        [Header("Non-Clampeds")]
        [SerializeField, NonReorderable] private AmountManipulator<float>[] _changeAmountManipulatableFloats;
        [SerializeField, NonReorderable] private AmountManipulator<int>[] _changeAmountManipulatableIntegers;


        public override void Act(Player player)
        {
            if (_changeClampedAmountManipalatableFloats != null && _changeClampedAmountManipalatableFloats.Length > 0)
            {
                foreach (var x in _changeClampedAmountManipalatableFloats)
                {
                    x.Apply(player.StatsHolder.StatsCollection);
                }
            }

            if (_changeClampedAmountManipalatableIntegers != null && _changeClampedAmountManipalatableIntegers.Length > 0)
            {
                foreach (var x in _changeClampedAmountManipalatableIntegers)
                {
                    x.Apply(player.StatsHolder.StatsCollection);
                }
            }

            if (_changeAmountManipulatableFloats != null && _changeAmountManipulatableFloats.Length > 0)
            {
                foreach (var x in _changeAmountManipulatableFloats)
                {
                    x.Apply(player.StatsHolder.StatsCollection);
                }
            }

            if (_changeAmountManipulatableIntegers != null && _changeAmountManipulatableIntegers.Length > 0)
            {
                foreach (var x in _changeAmountManipulatableIntegers)
                {
                    x.Apply(player.StatsHolder.StatsCollection);
                }
            }
        }
    }
}