using Game.Core;
using Game.Stats;
using UnityEngine;

namespace Game.StatsBehaviours
{
    public abstract class StatBehaviour : MonoBehaviour
    {
        [SerializeField] private Racer _racer;
        [SerializeField] private StatObject _statObject;

        protected Racer Racer => _racer;
        protected StatObject StatObject => _statObject;
    }
}
