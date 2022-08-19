using System;

namespace Externals.Utils.StatsSystem.Modifiers
{
    public sealed class FloatModifiableStatData : IModifiableStatData, IFloatValueCallback
    {
        public event Action<IFloatValueCallback, float> OnFloatValueChanged;

        private readonly StatObject _statObject;
        private readonly StatModifiersCollection _modifiersCollection;

        private float _source;
        private float _value;

        public FloatModifiableStatData(StatObject statObject, float sourceValue)
        {
            _statObject = statObject;
            _source = sourceValue;
            _modifiersCollection = new();
            _modifiersCollection.OnModified += HandleModified;
        }


        public StatObject StatObject => _statObject;
        public float Value => _value;
        public float SourceValue => _source;
        public StatModifiersCollection StatModifiers => _modifiersCollection;


        private void HandleModified(StatModifiersCollection statsCollection)
        {
            var tmp = _value;
            _value = statsCollection.ModifyValue(_source);
            OnFloatValueChanged?.Invoke(this, _value - tmp);
        }
    }
}
