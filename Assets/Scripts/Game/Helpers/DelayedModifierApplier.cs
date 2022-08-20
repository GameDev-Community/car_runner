using Externals.Utils.StatsSystem;
using UnityEngine;
using UnityEngine.Events;
using Utils.Attributes;

namespace Game.Interactables
{
    /// <summary>
    /// rename me
    /// </summary>
    public class DelayedModifierApplier : MonoBehaviour
    {
        [SerializeField, NonReorderable] private StatModifierCreator[] _modifiersCreator;
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

        public void Init(StatModifierCreator[] modifiersCreators, bool apply, float delay, StatsCollection statsColl, System.Action callback = null)
        {
            if (_inited)
                return;


            _modifiersCreator = modifiersCreators;
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
            ModifiersHelpers.ApplyModifiers(_modifiersCreator, _statsCollection);
        }

        private void Disapply()
        {
            ModifiersHelpers.DisapplyModifiers(_modifiersCreator, _statsCollection);
        }


    }
}