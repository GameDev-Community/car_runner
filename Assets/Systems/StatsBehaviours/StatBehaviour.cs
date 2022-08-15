using DevourDev.Unity.Utils.SimpleStats;
using UnityEngine;
using Utils.Attributes;

namespace Game.StatsBehaviours
{
    public class StatBehaviour : MonoBehaviour
    {
        [SerializeField, RequireInterface(typeof(IStatsHolder))] private UnityEngine.Object _statsHolder;



        private void OnValidate()
        {
            if (_statsHolder != null)
            {
                Debug.Log(_statsHolder.GetType());
            }
        }
    }
}
