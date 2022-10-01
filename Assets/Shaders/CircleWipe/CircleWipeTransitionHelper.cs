using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Testing.Shaders
{
    public sealed class CircleWipeTransitionHelper : MonoBehaviour
    {
        private const float _circleWipeDefaultTime = 2.3f;


        public static async Task TranslateToSceneAsync(int sceneID)
        {
            await TranslateToSceneAsync(sceneID, _circleWipeDefaultTime, _circleWipeDefaultTime / 2f);
        }


        public static async Task TranslateToSceneAsync(int sceneID, float fadingOutTime, float fadingInTime)
        {
            var sceneLoading = LoadSceneAsync(sceneID, LoadSceneMode.Single, false);
            var circleWiping = CircleWipe.CloseWithDefaultCurveAsync(fadingOutTime);
            await Task.WhenAll(sceneLoading, circleWiping);
            await FinishUnityAsyncOperationAsync(sceneLoading.Result);
            await CircleWipe.OpenWithDefaultCurveAsync(fadingInTime);
        }

        //public static async Task TranslateToSceneAsync_Old(int sceneID, float fadingOutTime, float fadingInTime)
        //{
        //    var sceneUnloading = UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex, false);
        //    var sceneLoading = LoadSceneAsync(sceneID, LoadSceneMode.Additive, false);
        //    var circleWiping = CircleWipe.TranslateRadiusDefaultCurveAsync(0, fadingOutTime);
        //    await Task.WhenAll(sceneUnloading, sceneLoading, circleWiping);
        //    await FinishUnityAsyncOperationAsync(sceneUnloading.Result);
        //    await FinishUnityAsyncOperationAsync(sceneLoading.Result);
        //    await CircleWipe.TranslateRadiusDefaultCurveAsync(1, fadingInTime);
        //}

        public static async Task<UnityEngine.AsyncOperation> LoadSceneAsync(int sceneID, LoadSceneMode loadSceneMode, bool activateSceneASAP)
        {
            var sceneLoading = SceneManager.LoadSceneAsync(sceneID, loadSceneMode);
            sceneLoading.allowSceneActivation = activateSceneASAP;
            for (; sceneLoading.progress < 0.899f; await Task.Yield());

            return sceneLoading;
        }

        public static async Task<UnityEngine.AsyncOperation> UnloadSceneAsync(int sceneID, bool unloadSceneASAP)
        {
            var sceneUnloading = SceneManager.UnloadSceneAsync(sceneID);
            sceneUnloading.allowSceneActivation = unloadSceneASAP;
            for (; sceneUnloading.progress < 0.899f; await Task.Yield());

            return sceneUnloading;
        }

        public static async Task FinishUnityAsyncOperationAsync(UnityEngine.AsyncOperation asop)
        {
            asop.allowSceneActivation = true;
            for (; !asop.isDone; await Task.Yield()) ;
        }

        //finishUnloadingAsync
    }
}