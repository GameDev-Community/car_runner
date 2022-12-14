using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Async
{
    //с какого-то гита, вальюабилити под вопросом.
    public static class UnityTaskUtil
    {
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        //private static void Install()
        //{
        //    UnitySynchronizationContext = SynchronizationContext.Current;
        //    UnityThreadId = Thread.CurrentThread.ManagedThreadId;
        //    UnityTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        //    UnityTaskFactory = new TaskFactory<UnityEngine.Object>(UnityTaskScheduler);
        //}


        static UnityTaskUtil()
        {

            UnitySynchronizationContext = SynchronizationContext.Current;
            UnityThreadId = Thread.CurrentThread.ManagedThreadId;
            UnityTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            UnityTaskFactory = new TaskFactory<UnityEngine.Object>(UnityTaskScheduler);

        }



        public static bool CurrentThreadIsUnityThread => UnityThreadId == Thread.CurrentThread.ManagedThreadId;

        public static TaskScheduler UnityTaskScheduler { get; private set; }

        public static TaskFactory<UnityEngine.Object> UnityTaskFactory { get; private set; }

        public static int UnityThreadId { get; private set; }

        public static SynchronizationContext UnitySynchronizationContext { get; private set; }





        /// <summary>
        /// Invokes an action on the Unity main thread.
        /// Will invoke immediately if already on the main thread.
        /// </summary>
        /// <param name="action">Action to be invoked</param>
        private static void RunOnUnityScheduler(Action action)
        {
            if (SynchronizationContext.Current == UnitySynchronizationContext)
            {
                action();
            }
            else
            {
                UnitySynchronizationContext.Post(_ => action(), null);
            }
        }

        /// <summary>
        /// Runs the given function on the Unity main thread
        /// Will invoke immediately if already on the main thread
        /// </summary>
        /// <param name="func">Method to be invoked</param>
        /// <typeparam name="T">Return type of the method</typeparam>
        /// <returns>Task with result type T</returns>
        public static async Task<T> RunOnUnityThreadAsync<T>(Func<T> func)
        {
            if (CurrentThreadIsUnityThread)
            {
                return func.Invoke();
            }
            var taskFactory = new TaskFactory<T>(UnityTaskScheduler);
            var task = taskFactory.StartNew(func);
            await task.ConfigureAwait(false);
            return task.Result;
        }

        /// <summary>
        /// Runs the given action on the Unity main thread
        /// Will invoke immediately if already on the main thread
        /// </summary>
        /// <param name="action">Action to be invoked</param>
        /// <returns>Task</returns>
        public static async Task RunOnUnityThreadAsync(Action action)
        {
            if (CurrentThreadIsUnityThread)
            {
                action.Invoke();
                return;
            }
            var taskFactory = new TaskFactory(UnityTaskScheduler);
            var task = taskFactory.StartNew(action);
            await task.ConfigureAwait(false);
        }

        /// <summary>
        /// Runs the given action the Unity thread
        /// Will invoke immediatley if already on the Unity main thread
        /// </summary>
        /// <param name="action">Action to be invoked</param>
        public static void RunOnUnityThread(Action action)
        {
            if (CurrentThreadIsUnityThread)
            {
                action();
            }
            else
            {
                UnitySynchronizationContext.Post(_ => action(), null);
            }
        }

        /// <summary>
        /// Runs the given async delegate on the Unity main thread
        /// </summary>
        /// <param name="funcAsync">async delegate</param>
        /// <typeparam name="T">return value of delegate</typeparam>
        /// <returns>value of type T</returns>
        public static Task<T> RunOnUnityThreadAsync<T>(Func<Task<T>> funcAsync)
        {
            return CurrentThreadIsUnityThread ? funcAsync() : Task.Factory.StartNew(funcAsync, CancellationToken.None, TaskCreationOptions.None, UnityTaskScheduler).Unwrap();
        }

        /// <summary>
        /// Runs the given async delegate on the Unity main thread
        /// </summary>
        /// <param name="funcAsync">async delegate</param>
        /// <returns>Task</returns>
        public static Task RunOnUnityThreadAsync(Func<Task> funcAsync)
        {
            return CurrentThreadIsUnityThread ? funcAsync() : Task.Factory.StartNew(funcAsync, CancellationToken.None, TaskCreationOptions.None, UnityTaskScheduler).Unwrap();
        }

        /// <summary>
        /// Instantiate a prefab asynchronously
        /// Always creates a task on the Unity task scheduler even if already on the main thread.
        /// </summary>
        /// <param name="prefab">Prefab to be instantiated</param>
        /// <param name="parent">Transform the prefab should be parented to</param>
        /// <typeparam name="T">Type of the prefab</typeparam>
        /// <returns>Task that contains the instantiated prefab as the result</returns>
        public static async Task<T> InstantiateAsync<T>(T prefab, Transform parent) where T : UnityEngine.Object
        {
            var task = UnityTaskFactory.StartNew(() =>
            {
                var instance = UnityEngine.Object.Instantiate(prefab, parent);
                instance.name = prefab.name;
                return instance;
            });
            await task.ConfigureAwait(false);
            return task.Result as T;
        }

        /// <summary>
        /// Instantiate a prefab asynchronously
        /// Always creates a task on the Unity task scheduler even if already on the main thread.
        /// </summary>
        /// <param name="prefab">Prefab to be instantiated</param>
        /// <typeparam name="T">Type of the prefab</typeparam>
        /// <returns>Task that contains the instantiated prefab as the result</returns>
        public static async Task<T> InstantiateAsync<T>(T prefab) where T : UnityEngine.Object
        {
            var task = UnityTaskFactory.StartNew(() =>
            {
                var instance = UnityEngine.Object.Instantiate(prefab);
                instance.name = prefab.name;
                return instance;
            });
            await task.ConfigureAwait(false);
            return task.Result as T;
        }

    }
}