using Externals.Utils.StatsSystem;
using UnityEngine;
using UnityEngine.Events;
using Utils.Attributes;

namespace Game.Interactables
{
    public class DelayedClampedFloatValueManipulator : MonoBehaviour
    {
        [SerializeField, NonReorderable] private DynamicStatValueChanger<float>[] _changers;
        [SerializeField, RequireInterface(typeof(IStatsHolder))] private UnityEngine.Object _statsHolder_raw;
        [SerializeField] private bool _apply;
        [SerializeField] private float _delay;
        [SerializeField] private UnityEvent _callbackEvent;

        private StatsCollection _statsCollection;
        private bool _inited;
        private System.Action _callback;
        private float _left;


        private void Start()
        {
            if (_inited)
                return;

            _statsCollection = ((IStatsHolder)_statsHolder_raw).StatsCollection;
            InitInternal();
        }

        public void Init(DynamicStatValueChanger<float>[] changers, bool apply, float delay, StatsCollection statsColl, System.Action callback = null)
        {
            if (_inited)
                return;


            _changers = changers;
            _apply = apply;
            _delay = delay;
            _statsCollection = statsColl;
            _callback = callback;

            InitInternal();
        }

        private void InitInternal()
        {
            _left = _delay;
            _inited = true;
        }


        private void Update()
        {
            if (_inited)
                return;

            if ((_left -= Time.deltaTime) <= 0)
            {
                if (_apply)
                    Apply();
                else
                    Disapply();

                _callback?.Invoke();
                _callbackEvent?.Invoke();

                Destroy(gameObject);
            }
        }

        private void Apply()
        {
            foreach (var item in _changers)
            {
                item.Apply(_statsCollection, !_apply);
            }
        }

        private void Disapply()
        {
            foreach (var item in _changers)
            {
                item.Apply(_statsCollection, !_apply);
            }
        }
    }
}