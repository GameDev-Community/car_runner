using UnityEngine;

namespace Externals.Utils.StatsSystem
{
    public class StatsPanelUi : MonoBehaviour
    {
        //x floating stat
        //x integer stat
        //x floating clamped stat
        //x integer clamped stat
        //x floating modifiable 
    }

    public abstract class StatVisualizerBase<T> : MonoBehaviour where T : IStatData
    {
        public abstract void Init(T data);
        public abstract void Visualize(T data);
    }

    //public class FloatDynamicStatVisualizer : StatVisualizerBase<FloatDynamicStatVisualizer>
    //{
    //    [SerializeField]
    //    private ImageConversion
    //    [SerializeField] private TMPro.TextMeshProUGUI _statValueText;


    //    public override void Init(FloatDynamicStatVisualizer data)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public override void Visualize(FloatDynamicStatVisualizer data)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}
}
