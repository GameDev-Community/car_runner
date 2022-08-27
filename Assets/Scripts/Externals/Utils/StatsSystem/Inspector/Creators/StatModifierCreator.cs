using UnityEngine;
using Externals.Utils.StatsSystem.Modifiers;

namespace Externals.Utils.StatsSystem
{
    [System.Serializable]
    public class StatModifierCreator : IStatChanger
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private Modifiers.ModifyingMode _mode;
        [SerializeField] private float _value;
        [SerializeField] private int _amount = 1;


        public StatModifierCreator(StatObject statObject, ModifyingMode mode, float value)
        {
            _statObject = statObject;
            _mode = mode;
            _value = value;
        }


        public StatObject StatObject => _statObject;
        public ModifyingMode Mode => _mode;
        public float Value => _value;
        public int Amount => _amount;


        public StatModifier Create()
        {
            return new(_mode, _value);
        }


        public void Apply(StatsCollection statsCollection, bool inverse = false)
        {
            var x = statsCollection.TryGetStatDataT<IModifiableStatData>(_statObject, out var msd);

            if (!x)
            {
#if UNITY_EDITOR
                Debug.LogError($"{statsCollection}, {_statObject}");
                if (statsCollection.TryGetStatData(_statObject, out var debugSD))
                {
                    Debug.LogError(debugSD.GetType());
                }
                else
                {
                    Debug.LogError("Stat not found");
                }
#endif

                return;
            }

            var mds = msd.StatModifiers;

            if (inverse)
            {
                mds.RemoveModifierImmediate(Create(), _amount);
            }
            else
            {
                mds.AddModifierImmediate(Create(), _amount);
            }
        }
    }
}
