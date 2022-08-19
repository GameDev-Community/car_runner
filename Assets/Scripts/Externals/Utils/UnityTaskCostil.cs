using UnityEngine;

namespace Utils
{
    public class UnityTaskCostil : MonoBehaviour
    {
        private static bool _isCancellationRequested;


        private void Awake()
        {
            _isCancellationRequested = false;

            if (UnityEngine.Application.isPlaying)
                UnityEngine.Application.quitting += Application_quitting;
        }

        private void Application_quitting()
        {
            _isCancellationRequested = true;
        }


        public static bool IsCancellationRequested => _isCancellationRequested;


        public static void ThrowIfCancellationRequested()
        {
#if UNITY_EDITOR
            _isCancellationRequested = !UnityEngine.Application.isPlaying;
#endif
            if (IsCancellationRequested)
                throw new System.Threading.Tasks.TaskCanceledException("Task cancelled due to Application quitting");
        }
    }
}