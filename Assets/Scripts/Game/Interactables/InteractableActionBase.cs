using Game.Core;
using UnityEngine;

namespace Game.Interactables
{
    public abstract class InteractableActionBase : MonoBehaviour
    {
        public abstract void Act(Player player);
    }
}