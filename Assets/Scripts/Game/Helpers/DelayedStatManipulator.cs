using Externals.Utils.StatsSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils.Attributes;

namespace Game.Interactables
{
    public class DelayedStatManipulator : MonoBehaviour
    {
        [Header("Clampeds")]
        [SerializeField, NonReorderable] ClampedAmountManipulator<float>[] _changeClampedAmountManipalatableFloats;
        [SerializeField, NonReorderable] ClampedAmountManipulator<int>[] _changeClampedAmountManipalatableIntegers;
        [Space]
        [Header("Non-Clampeds")]
        [SerializeField, NonReorderable] private AmountManipulator<float>[] _changeAmountManipulatableFloats;
        [SerializeField, NonReorderable] private AmountManipulator<int>[] _changeAmountManipulatableIntegers;
        [Space]
        [SerializeField, RequireInterface(typeof(IStatsHolder))] private UnityEngine.Object _statsHolder_raw;
        [SerializeField] private bool _apply;
        [SerializeField] private float _delay;
        [SerializeField] private UnityEvent _callbackEvent;

        private List<IStatChanger> _changers;
        private StatsCollection _statsCollection;
        private bool _inited;
        private System.Action _callback;
        private float _left;


        private void Start()
        {
            if (_inited)
                return;

            _statsCollection = ((IStatsHolder)_statsHolder_raw).StatsCollection;

            if (_changeClampedAmountManipalatableFloats != null && _changeClampedAmountManipalatableFloats.Length > 0)
            {
                foreach (var x in _changeClampedAmountManipalatableFloats)
                {
                    _changers.Add(x);
                }
            }

            if (_changeClampedAmountManipalatableIntegers != null && _changeClampedAmountManipalatableIntegers.Length > 0)
            {
                foreach (var x in _changeClampedAmountManipalatableIntegers)
                {
                    _changers.Add(x);
                }
            }

            if (_changeAmountManipulatableFloats != null && _changeAmountManipulatableFloats.Length > 0)
            {
                foreach (var x in _changeAmountManipulatableFloats)
                {
                    _changers.Add(x);
                }
            }

            if (_changeAmountManipulatableIntegers != null && _changeAmountManipulatableIntegers.Length > 0)
            {
                foreach (var x in _changeAmountManipulatableIntegers)
                {
                    _changers.Add(x);
                }
            }

            InitInternal();
        }

        public void Init(List<IStatChanger> changers, bool apply, float delay, StatsCollection statsColl, System.Action callback = null)
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