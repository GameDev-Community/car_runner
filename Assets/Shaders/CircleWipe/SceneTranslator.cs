using UnityEngine;

namespace Testing.Shaders
{
    public class SceneTranslator : MonoBehaviour
    {
        [SerializeField] private int _targetSceneID;
        [SerializeField] private float _fadingOutTime = 2.3f;
        [SerializeField] private float _fadingInTime = 1f;


        public async void TranslateToTargetScene()
        {
            DontDestroyOnLoad(gameObject);
            var tmpPrio = UnityEngine.Application.backgroundLoadingPriority;
            UnityEngine.Application.backgroundLoadingPriority = ThreadPriority.Low;
            Debug.Log("translating");
            await CircleWipeTransitionHelper.TranslateToSceneAsync(_targetSceneID, _fadingOutTime, _fadingInTime);
            Debug.Log("translated?");
            UnityEngine.Application.backgroundLoadingPriority = tmpPrio;
            Destroy(gameObject);
        }

        public async void TranslateToScene(int buildIndex)
        {
            await CircleWipeTransitionHelper.TranslateToSceneAsync(buildIndex, _fadingOutTime, _fadingInTime);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
                TranslateToTargetScene();
        }
    }
}