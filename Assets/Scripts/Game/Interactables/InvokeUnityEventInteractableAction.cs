using Game.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactables
{
    public class InvokeUnityEventInteractableAction : InteractableActionBase
    {
        [SerializeField] private UnityEvent _event;
        [SerializeField] private UnityEvent<Player> _event_Player;


        public override void Act(Player player)
        {
            _event?.Invoke();
            _event_Player?.Invoke(player);
        }
    }
}