using UnityEngine;

namespace Externals.Utils.StatsSystem
{
    [System.Serializable]
    public class ClampedFloatBoundsManipulator : IStatChanger
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private bool _changeMin;
        [SerializeField] private float _min;
        [SerializeField] private bool _changeMax;
        [SerializeField] private float _max;
        [SerializeField] private bool _changeCur;
        [SerializeField] private float _cur;

        public void Apply(StatsCollection statsCollection, bool inverse = false)
        {
            if (!statsCollection.TryGetStatData(_statObject, out var sdRaw))
            {
                Debug.LogError($"key not found: {_statObject}");
                return;
            }

            if (sdRaw is ClampedFloatStatData sd)
            {
                //ignore inverse flag

                float min = _changeMin ? _min : sd.Min;
                float max = _changeMax ? _max : sd.Max;

                if (_changeCur)
                    sd.SetBounds(min, max, _cur);
                else
                    sd.SetBounds(min, max);
            }
            else
            {
                Debug.LogError($"unable to cast {sdRaw.GetType()} as" +
                    $" {nameof(ClampedFloatStatData)} ({_statObject})");
            }
        }
    }
    [System.Serializable]
    public class ClampedAmountManipulator<T> : IStatChanger
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private ClampedAmountManipulation _manipulation;
        [SerializeField] private T _value;


        public void Apply(StatsCollection statsCollection, bool inverse = false)
        {
            if (!statsCollection.TryGetStatData(_statObject, out var sdRaw))
            {
                Debug.LogError($"key not found: {_statObject}");
                return;
            }

            if (sdRaw is IClampedAmountManipulatable<T> sd)
            {
                if (inverse)
                {
                    switch (_manipulation)
                    {
                        case ClampedAmountManipulation.AddSafe:
                            sd.RemoveSafe(_value);
                            break;
                        case ClampedAmountManipulation.RemoveSafe:
                            sd.AddSafe(_value);
                            break;
                        case ClampedAmountManipulation.ChangeSafe:
                            sd.ChangeSafe(_value, inverse);
                            break;
                        case ClampedAmountManipulation.SetSafe:
                            sd.SetSafe(_value);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (_manipulation)
                    {
                        case ClampedAmountManipulation.AddSafe:
                            sd.AddSafe(_value);
                            break;
                        case ClampedAmountManipulation.RemoveSafe:
                            sd.RemoveSafe(_value);
                            break;
                        case ClampedAmountManipulation.ChangeSafe:
                            sd.ChangeSafe(_value);
                            break;
                        case ClampedAmountManipulation.SetSafe:
                            sd.SetSafe(_value);
                            break;
                        default:
                            break;
                    }
                }

            }
            else
            {
                Debug.LogError($"unable to cast {sdRaw.GetType()} as {nameof(IClampedAmountManipulatable<T>)} where T is {typeof(T)} ({_statObject})");
            }
        }
    }
}
