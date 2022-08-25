using Externals.Utils.Valuables;
using UnityEngine;

namespace Externals.Utils.StatsSystem
{
    [System.Serializable]
    public class AmountManipulator<T> : IStatChanger
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private AmountManipulation _manipulation;
        [SerializeField] private T _value;
        [SerializeField] private bool _try;


        public void Apply(StatsCollection statsCollection, bool inverse = false)
        {
            if (!statsCollection.TryGetStatData(_statObject, out var sdRaw))
            {
                Debug.LogError($"key not found: {_statObject}");
                return;
            }

            if (sdRaw is IAmountManipulatable<T> sd)
            {
                if (inverse)
                {
                    switch (_manipulation)
                    {
                        case AmountManipulation.Add:
                            if (_try)
                                sd.TryRemove(_value);
                            else
                                sd.Remove(_value);
                            break;
                        case AmountManipulation.Remove:
                            if (_try)
                                sd.TryAdd(_value);
                            else
                                sd.Add(_value);
                            break;
                        case AmountManipulation.Change:
                            if (_try)
                                sd.TryChange(_value, inverse);
                            else
                                sd.Change(_value, inverse);
                            break;
                        case AmountManipulation.Set:
                            if (_try)
                                sd.TrySet(_value);
                            else
                                sd.Set(_value);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (_manipulation)
                    {
                        case AmountManipulation.Add:
                            if (_try)
                                sd.TryAdd(_value);
                            else
                                sd.Add(_value);
                            break;
                        case AmountManipulation.Remove:
                            if (_try)
                                sd.TryRemove(_value);
                            else
                                sd.Remove(_value);
                            break;
                        case AmountManipulation.Change:
                            if (_try)
                                sd.TryChange(_value);
                            else
                                sd.Change(_value);
                            break;
                        case AmountManipulation.Set:
                            if (_try)
                                sd.TrySet(_value);
                            else
                                sd.Set(_value);
                            break;
                        default:
                            break;
                    }
                }
             
            }
            else
            {
                Debug.LogError($"unable to cast {sdRaw.GetType()} as {nameof(IAmountManipulatable<T>)} ({_statObject})");
            }
        }
    }
}
