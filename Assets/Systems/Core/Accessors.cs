using Game.Interactables;
using Game.StatsUtils;
using UnityEngine;

namespace Game.Core
{
    public class Accessors : MonoBehaviour
    {
        [SerializeField] private DelayedModifierChanger _delayedModifierChangerPrefab;

        private static Accessors _inst;


        public static DelayedModifierChanger DelayedModifierChangerPrefab => _inst._delayedModifierChangerPrefab;


        private void Awake()
        {
            _inst = this;
        }
    }
}
