using System;
using UnityEngine;

namespace Externals.Utils.StatsSystem.Ui
{
    public abstract class StatVisualizerBase<T> : MonoBehaviour where T : IStatData
    {
        [SerializeField] private TMPro.TextMeshProUGUI _statNameText;

        private T _statData;


        protected T StatData => _statData;


        public void Init(T data)
        {
            _statData = data;
            _statNameText.text = data.StatObject.MetaInfo.Name;
            HandleInitialization();
        }


        protected abstract void HandleInitialization();
    }
}
