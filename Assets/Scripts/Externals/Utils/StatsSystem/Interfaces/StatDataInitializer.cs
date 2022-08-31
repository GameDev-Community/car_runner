using Externals.Utils.StatsSystem.Modifiers;
using UnityEngine;

namespace Externals.Utils.StatsSystem
{
    public class StatDataInitializer : MonoBehaviour
    {
        /// <summary>
        /// 0: FloatDynamicStatData
        /// 1: ClampedFloatStatData
        /// 2: FloatModifiableStatData
        /// 3: FloatStatData 
        /// 4: IntDynamicStatData v
        /// 5: ClampedIntStatData 
        /// 6: IntModifiableStatData
        /// 7: IntStatData
        /// </summary>
        [SerializeField, HideInInspector] private int _actionID;
        [SerializeField] private StatObject _statObject;

        [SerializeField] private FloatDynamicStatDataCreator _floatDynamicStatDataCreator;
        [SerializeField] private ClampedFloatStatDataCreator _clampedFloatStatDataCreator;
        [SerializeField] private FloatModifiableStatDataCreator _floatModifiableStatDataCreator;
        [SerializeField] private FloatStatDataCreator _floatStatDataCreator;
        [SerializeField] private IntDynamicStatDataCreator _intDynamicStatDataCreator;
        [SerializeField] private ClampedIntStatDataCreator _clampedIntStatDataCreator;
        [SerializeField] private IntModifiableStatDataCreator _intModifiableStatDataCreator;
        [SerializeField] private IntStatDataCreator _intStatDataCreator;


        public StatObject StatObject => _statObject;


        public IStatData Create()
        {
            return _actionID switch
            {
                0 => _floatDynamicStatDataCreator.Create(),
                1 => _clampedFloatStatDataCreator.Create(),
                2 => _floatModifiableStatDataCreator.Create(),
                3 => _floatStatDataCreator.Create(),
                4 => _intDynamicStatDataCreator.Create(),
                5 => _clampedIntStatDataCreator.Create(),
                6 => _intModifiableStatDataCreator.Create(),
                7 => _intStatDataCreator.Create(),
                _ => throw new System.NotImplementedException("unexpected id: " + _actionID.ToString()),
            };
        }
    }

    [System.Serializable]
    public class StatDataRuntimeCreator
    {
        /// <summary>
        /// 0: FloatDynamicStatData
        /// 1: ClampedFloatStatData
        /// 2: FloatModifiableStatData
        /// 3: FloatStatData 
        /// 4: IntDynamicStatData
        /// 5: ClampedIntStatData 
        /// 6: IntModifiableStatData
        /// 7: IntStatData
        /// </summary>
        [SerializeField, HideInInspector] private int _actionID;
        [SerializeField] private StatObject _statObject;
        [SerializeField] private FloatDynamicStatDataCreator _floatDynamicStatDataCreator;
        [SerializeField] private ClampedFloatStatDataCreator _clampedFloatStatDataCreator;
        [SerializeField] private FloatModifiableStatDataCreator _floatModifiableStatDataCreator;
        [SerializeField] private FloatStatDataCreator _floatStatDataCreator;
        [SerializeField] private IntDynamicStatDataCreator _intDynamicStatDataCreator;
        [SerializeField] private ClampedIntStatDataCreator _clampedIntStatDataCreator;
        [SerializeField] private IntModifiableStatDataCreator _intModifiableStatDataCreator;
        [SerializeField] private IntStatDataCreator _intStatDataCreator;

        public StatObject StatObject => _statObject;


        public FloatDynamicStatDataCreator FloatDynamicStatDataCreator => _floatDynamicStatDataCreator;
        public ClampedFloatStatDataCreator ClampedFloatStatDataCreator => _clampedFloatStatDataCreator;
        public FloatModifiableStatDataCreator FloatModifiableStatDataCreator => _floatModifiableStatDataCreator;
        public FloatStatDataCreator FloatStatDataCreator => _floatStatDataCreator;
        public IntDynamicStatDataCreator IntDynamicStatDataCreator => _intDynamicStatDataCreator;
        public ClampedIntStatDataCreator ClampedIntStatDataCreator => _clampedIntStatDataCreator;
        public IntModifiableStatDataCreator IntModifiableStatDataCreator => _intModifiableStatDataCreator;
        public IntStatDataCreator IntStatDataCreator => _intStatDataCreator;


        public IStatData Create()
        {
            return _actionID switch
            {
                0 => _floatDynamicStatDataCreator.Create(),
                1 => _clampedFloatStatDataCreator.Create(),
                2 => _floatModifiableStatDataCreator.Create(),
                3 => _floatStatDataCreator.Create(),
                4 => _intDynamicStatDataCreator.Create(),
                5 => _clampedIntStatDataCreator.Create(),
                6 => _intModifiableStatDataCreator.Create(),
                7 => _intStatDataCreator.Create(),
                _ => throw new System.NotImplementedException("unexpected id: " + _actionID.ToString()),
            };
        }
    }

}