using UnityEngine;

namespace Externals.Utils.StatsSystem
{
    [System.Serializable]
    public class ClampedBoundsManipulator<TValue> : IStatChanger
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private bool _changeMin;
        [SerializeField] private TValue _min;
        [SerializeField] private bool _changeMax;
        [SerializeField] private TValue _max;
        [SerializeField] private bool _changeCur;
        [SerializeField] private TValue _cur;

        public void Apply(StatsCollection statsCollection, bool inverse = false)
        {
            if (!statsCollection.TryGetStatData(_statObject, out var sdRaw))
            {
                Debug.LogError($"key not found: {_statObject}");
                return;
            }

            if (sdRaw is IClampedBoundsManipulatable<TValue> sd)
            {
                //ignore inverse flag

                var min = _changeMin ? _min : sd.Min;
                var max = _changeMax ? _max : sd.Max;

                if (_changeCur)
                    sd.SetBounds(min, max, _cur);
                else
                    sd.SetBounds(min, max);
            }
            else
            {
                Debug.LogError($"unable to cast {sdRaw.GetType()} as" +
                     $" {nameof(IClampedBoundsManipulatable<TValue>)} where T is {typeof(TValue)} ({_statObject})");
            }
        }
    }
}
