using Game.Core;
using UnityEngine;

namespace Game.Interactables
{
    public class DelayedModifierChanger : MonoBehaviour
    {
        [SerializeField] private Racer _racer;
        [SerializeField] private bool _add;
        [SerializeField] private SpeedModifier[] _modifiers;
        [SerializeField] private float _time;

        private bool _started;
        private float _left;


        private void Start()
        {
            if (_started)
                return;

            Init(_racer, _add, _time, _modifiers);
        }


        public void Init(Racer racer, bool add, float time, params SpeedModifier[] modifiers)
        {
            _started = true;
            _racer = racer;
            _add = add;
            _modifiers = modifiers;
            _left = _time = time;
        }


        private void Update()
        {
            if ((_left -= Time.deltaTime) <= 0)
            {
                foreach (var m in _modifiers)
                {
                    if (_add)
                        _racer.AddSpeedModifier(m);
                    else
                        _racer.RemoveSpeedModifier(m);
                }

                Destroy(gameObject);
            }
        }
    }


}
