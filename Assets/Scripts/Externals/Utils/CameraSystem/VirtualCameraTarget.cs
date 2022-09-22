using Cinemachine;
using System.Threading.Tasks;
using UnityEngine;

namespace Externals.Utils
{
    public class VirtualCameraTarget : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _vc;

        private VirtualCamerasGroup _group;


        internal CinemachineVirtualCamera VC => _vc;


        public void Focus()
        {
            _group.FocusOn(this);
        }


        public async Task FocusAsync()
        {
            await _group.FocusOnAsync(this);
        }

        public void Unfocus()
        {
            _group.FocusOnDefault();
        }

        public async Task UnfocusAsync()
        {
            await _group.FocusOnDefaultAsync();
        }

        
        internal void InitInternal(VirtualCamerasGroup group)
        {
            _group = group;
        }


    }
}
