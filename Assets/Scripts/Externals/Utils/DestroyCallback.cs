using UnityEngine;

namespace Utils
{
    public class DestroyCallback : MonoBehaviour
    {
        private System.Action _callback;


        public void Init(System.Action callback)
        {
            if (_callback != null)
            {
                AddAction(callback);
                return;
            }

            _callback = callback;
        }


        public void AddAction(System.Action action)
        {
            if (_callback == null)
            {
                Init(action);
                return;
            }

            _callback += action;
        }


        private void OnDestroy()
        {
            _callback?.Invoke();
        }
    }

}