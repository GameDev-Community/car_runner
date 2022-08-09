using Game.Core;
using System;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Game.StatsBehaviours
{
    public class NitroBehaviour : FloatStatBehaviour
    {
        [SerializeField] private Stats.StatObject _accelerationStat;
        [SerializeField] private UnityEvent _onNitroStarted;
        [SerializeField] private UnityEvent _onNitroEnded;


        [SerializeField] private StatModifier _speedModifier;
        //prototype
        [SerializeField] private float _nitroCostPerSecond = 10f;

        private bool _nitroActive;


        protected override void Start()
        {
            base.Start();
            StatData.OnMinValueReached += HandleMinNitro;
        }

        private void HandleMinNitro(ClampedFloat se, float dd, float sd)
        {
            StopNitro();
        }

        private void Update()
        {
            if (_nitroActive)
                StatData.Change(-_nitroCostPerSecond * Time.deltaTime);
        }


        /// <summary>
        /// accessor for buttons i.e.
        /// </summary>
        public void StartNitro()
        {
            if (_nitroActive)
            {
                Debug.LogError("attempt to StartNitro when " +
                    "already nitring");
                return;
            }

            _nitroActive = true;
            Racer.AddStatModifier(_accelerationStat, _speedModifier);
            _onNitroStarted?.Invoke();
        }

        /// <summary>
        /// accessor for buttons i.e.
        /// </summary>
        public void StopNitro()
        {
            if (!_nitroActive)
            {
                Debug.LogError("attempt to StopNitro when " +
                    "not nitring");
                return;
            }

            _nitroActive = false;
            Racer.RemoveStatModifier(_accelerationStat, _speedModifier);
            _onNitroEnded?.Invoke();
        }


        public void SwitchNitroState()
        {
            if (_nitroActive)
                StopNitro();
            else
                StartNitro();
        }
    }
}
