namespace DevourDev.Unity.Utils.SimpleStats.Modifiers
{
    public class ModifiableStatData : IStatData
    {
        public event System.Action<IStatData, float, float> OnValueChanged;

        private readonly StatObject _statObject;
        private readonly StatModifiersCollection _modifiersCollection;
        private float _sourceValue;
        private float _modifiedValue;


        public ModifiableStatData(StatObject statObject)
        {
            _statObject = statObject;
            _modifiersCollection = new();
            _modifiersCollection.OnModified += HandleModifying;
        }


        public StatObject StatObject => _statObject;

        public float Source => _sourceValue;
        public float Value => _modifiedValue;


        public void ChangeValue(float delta)
        {
            var raw = _sourceValue + delta;
            _sourceValue = ProcessSourceValue(raw);
            HandleModifying(_modifiersCollection);
        }


        private void HandleModifying(StatModifiersCollection sender)
        {
            UpdateModifiedValue();
        }

        private void UpdateModifiedValue()
        {
            float tmp = _modifiedValue;
            var raw = _modifiersCollection.ModifyValue(_sourceValue);
            float dirtyDelta = raw - tmp;
            var safe = ProcessModifiedValue(raw);
            float safeDelta = _modifiedValue - tmp;
            SetModifiedValue(safe, dirtyDelta, safeDelta);
        }


        protected virtual float ProcessSourceValue(float raw)
        {
            return raw < 0 ? 0 : raw;
        }

        protected virtual float ProcessModifiedValue(float raw)
        {
            return raw < 0 ? 0 : raw;
        }


        protected void SetSourceValue(float value)
        {
            _sourceValue = ProcessSourceValue(value);
            UpdateModifiedValue();
        }

        protected void SetModifiedValue(float value, float dirtyDelta, float safeDelta)
        {
            _modifiedValue = value;
            OnValueChanged?.Invoke(this, dirtyDelta, safeDelta);
        }


        public bool ContainsModifier(StatModifier m)
        {
            return _modifiersCollection.ContainsModifier(m);
        }

        public bool TryGetModifierAmount(StatModifier m, out int amount)
        {
            return _modifiersCollection.TryGetAmount(m, out amount);
        }

        public void AddModifier(StatModifier m, int amount)
        {
            _modifiersCollection.AddModifier(m, amount);
        }

        public void RemoveModifier(StatModifier m, int amount)
        {
            _modifiersCollection.RemoveModifier(m, amount);
        }

        public void FinishAddingModifiers()
        {
            _modifiersCollection.FinishAddingModifiers();
        }

        public void FinishRemovingModifiers()
        {
            _modifiersCollection.FinishRemovingModifiers();
        }


        public (StatModifier, int)[] GetModifiers()
        {
            return _modifiersCollection.GetModifiers();
        }

        public CountingDictionary<StatModifier> GetModifiersDictionary()
        {
            return _modifiersCollection.GetModifiersDictionary();
        }

        public bool TryRemoveModifier(StatModifier m, int amount)
        {
            return _modifiersCollection.TryRemoveModifier(m, amount);
        }
    }
}

