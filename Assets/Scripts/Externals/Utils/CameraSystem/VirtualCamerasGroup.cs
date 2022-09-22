using Cinemachine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Externals.Utils
{
    public class VirtualCamerasGroup : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _defaultVC;
        [SerializeField] private List<VirtualCameraTarget> _targets;

        private VirtualCameraTarget _currentTarget;
        private Cinemachine.CinemachineBrain _brain;


        public float TranslationTime => _brain.m_DefaultBlend.BlendTime;

        internal int TranslationTimeMillis => (int)(TranslationTime * 1000);


        private void Awake()
        {
            foreach (var t in _targets)
            {
                t.InitInternal(this);
            }

            _brain = Camera.main.GetComponent<CinemachineBrain>();
        }

        internal void FocusOn(VirtualCameraTarget ct)
        {
            DisableActive();

            _currentTarget = ct;
            ct.VC.gameObject.SetActive(true);
        }

        internal async Task FocusOnAsync(VirtualCameraTarget ct)
        {
            FocusOn(ct);
            await Task.Delay(TranslationTimeMillis);
        }

        public void FocusOnDefault()
        {
            if (_currentTarget == null)
                return;

            DisableActive();

            _currentTarget = null;
            _defaultVC.gameObject.SetActive(true);
        }

        public async Task FocusOnDefaultAsync()
        {
            FocusOnDefault();
            await Task.Delay(TranslationTimeMillis);
        }


        private void DisableActive()
        {
            if (_currentTarget == null)
                _defaultVC.gameObject.SetActive(false);
            else
                _currentTarget.VC.gameObject.SetActive(false);
        }
    }
}
