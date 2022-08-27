using System.Collections.Generic;
using UnityEngine;

namespace Externals.Utils.StatsSystem.Ui
{
    public class StatsVisualiserUi : MonoBehaviour
    {
        //[SerializeField] private ClampedFloatStatDataVisualizer _clampedFloatStatDataVisualizerPrefab;
        //[SerializeField] private ClampedIntStatDataVisualizer _clampedIntStatDataVisualizerPrefab;
        //[SerializeField] private IntStatDataVisualizer _intStatDataVisualizerPrefab;
        //[SerializeField] private FloatStatDataVisualizer _floatStatDataVisualizerPrefab;

        //private Dictionary<StatObject, GameObject> _slots;


        //public void Init(StatsCollection statsCollection)
        //{
        //    _slots = new();

        //    foreach (KeyValuePair<StatObject, IStatData> statKvp in statsCollection)
        //    {
        //        GameObject slot;

        //        var sd = statKvp.Value;
        //        var sdInfo = statKvp.Value.StatObject.GetStatDataInfo();
        //        bool isInteger = sdInfo.NumericsType == StatObject.NumericsType.Integer;
        //        bool isClamped = sdInfo.Clamped;
        //        bool isModifiable = sdInfo.Modifiable;

        //        if (isClamped)
        //        {
        //            switch (sd)
        //            {
        //                case 
        //                default:
        //                    throw new System.Exception("unexpected stat data: " + sd.GetType().FullName);
        //            }
        //        }
        //    }

        //    statsCollection.OnStatAdded += HandleStatAdded;
        //    statsCollection.OnStatRemoved += HandleStatRemoved;
        //}

        //private void HandleStatRemoved(StatsCollection arg1, IStatData arg2)
        //{
        //    throw new System.NotImplementedException();
        //}

        //private void HandleStatAdded(StatsCollection arg1, IStatData arg2)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
