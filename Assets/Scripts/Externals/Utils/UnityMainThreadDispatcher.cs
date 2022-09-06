using System;
using System.Collections.Generic;
using UnityEngine;


namespace DevourDev.Unity
{
    public class UnityMainThreadDispatcher : MonoBehaviour
    {
        public static UnityMainThreadDispatcher Instance { get; private set; }
        private bool _singletonSucceed;

        private readonly object _lockObject = new();

        private Queue<Action> _queuedActions;
        private Dictionary<object, Action> _rewritableActions;
        private List<Action> _tmpActions;

//#if UNITY_EDITOR
//        [SerializeField] private bool _awake;
//        [SerializeField] private bool _update;


//        private void OnValidate()
//        {
//            if (_awake)
//            {
//                _awake = false;
//                Awake();
//            }

//            if (_update)
//            {
//                _update = false;
//                Update();
//            }
//        }
//#endif


        private void Awake()
        {
            InitSingleton();

            if (!_singletonSucceed)
                return;

            _queuedActions = new Queue<Action>();
            _rewritableActions = new Dictionary<object, Action>();
            _tmpActions = new();
        }

        private void Start()
        {
            if (!_singletonSucceed)
                return;


        }

        private void Update()
        {
            ExecuteQueuedActions();
            ExecuteRewritableActions();

        }

        private void ExecuteRewritableActions()
        {
            lock (_lockObject)
            {
                if (_rewritableActions.Count == 0)
                    return;

                _tmpActions.Clear();

                foreach (var ra in _rewritableActions)
                {
                    _tmpActions.Add(ra.Value);
                }

                _rewritableActions.Clear();

                foreach (var act in _tmpActions)
                {
                    act?.Invoke();
                }
            }
        }

        private void ExecuteQueuedActions()
        {
            lock (_lockObject)
            {
                if (_queuedActions.Count == 0)
                    return;

                _tmpActions.Clear();

                while (_queuedActions.TryDequeue(out var a))
                {
                    _tmpActions.Add(a);
                }


                foreach (var act in _tmpActions)
                {
                    act?.Invoke();
                }
            }


        }


        public static void Log(string message)
        {
            InvokeOnMainThread(() => Debug.Log(message));
        }

        public static void LogWarning(string message)
        {
            InvokeOnMainThread(() => Debug.LogWarning(message));
        }

        public static void LogError(string message)
        {
            InvokeOnMainThread(() => Debug.LogError(message));
        }

        public static void InvokeOnMainThread(Action act)
        {
            lock (Instance._lockObject)
            {
                Instance._queuedActions.Enqueue(act);
            }
        }
        public static void InvokeOnMainThread(Action act, object rewritableActionKey)
        {
            lock (Instance._lockObject)
            {
                if (!Instance._rewritableActions.TryAdd(rewritableActionKey, act))
                {
                    Instance._rewritableActions[rewritableActionKey] = act;
                }
            }
        }

        private void InitSingleton()
        {
            if (Instance != null)
            {
                if (Instance != this)
                {
                    Destroy(gameObject);
                    _singletonSucceed = false;
                }

                _singletonSucceed = true;
            }

            Instance = this;
            _singletonSucceed = true;
            //DontDestroyOnLoad(gameObject);
        }
    }
}
