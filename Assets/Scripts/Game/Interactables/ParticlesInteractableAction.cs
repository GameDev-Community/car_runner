using Game.Core;
using UnityEngine;

namespace Game.Interactables
{
    public class ParticlesInteractableAction : InteractableActionBase
    {
        [SerializeField] private ParticleSystemAndParticles[] _particleSystemCreators;


        public override void Act(Player player)
        {
            if (_particleSystemCreators != null)
            {
                foreach (var item in _particleSystemCreators)
                {
                    item.InstantiateVfx(transform.position, true);
                }
            }
        }
    }
}