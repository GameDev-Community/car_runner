namespace Externals.Utils.StatsSystem.Modifiers
{
    public class StatModifier
    {
        private readonly ModifyingMode _mode;
        private readonly long _value;


        public StatModifier(ModifyingMode mode, float value)
        {
            _mode = mode;
            _value = (long)(value * ModifiersOptimizer.Coefficient);
        }


        public ModifyingMode Mode => _mode;
        public float Value => (float)_value / ModifiersOptimizer.Coefficient;


        public void Modify(ref float flat, ref float mult, int amount)
        {
            var totalV = _value * amount;
            switch (_mode)
            {
                case ModifyingMode.Flat:
                    flat += totalV;
                    return;
                case ModifyingMode.Multiply:
                    mult += totalV;
                    return;
                default:
                    throw new System.NotImplementedException($"unexpected enum value:" +
                        $" {nameof(ModifyingMode)}.{_mode}");
            }
        }

        public void Modify(ModifiersOptimizer optimizer, int amount)
        {
            var totalV = _value * amount;

            switch (_mode)
            {
                case ModifyingMode.Flat:
                    optimizer.ChangeFlatData(totalV);
                    return;
                case ModifyingMode.Multiply:
                    optimizer.ChangeMultData(totalV);
                    return;
                default:
                    throw new System.NotImplementedException($"unexpected enum value:" +
                        $" {nameof(ModifyingMode)}.{_mode}");
            }
        }


        public void Unmodify(ref float flat, ref float mult, int amount)
        {
            var totalV = _value * amount;
            switch (_mode)
            {
                case ModifyingMode.Flat:
                    flat -= totalV;
                    return;
                case ModifyingMode.Multiply:
                    mult -= totalV;
                    return;
                default:
                    throw new System.NotImplementedException($"unexpected enum value:" +
                        $" {nameof(ModifyingMode)}.{_mode}");
            }
        }

        public void Unmodify(ModifiersOptimizer optimizer, int amount)
        {
            var totalV = _value * amount;

            switch (_mode)
            {
                case ModifyingMode.Flat:
                    optimizer.ChangeFlatData(-totalV);
                    return;
                case ModifyingMode.Multiply:
                    optimizer.ChangeMultData(-totalV);
                    return;
                default:
                    throw new System.NotImplementedException($"unexpected enum value:" +
                        $" {nameof(ModifyingMode)}.{_mode}");
            }
        }

        public override string ToString()
        {
            return $"Stat Modifier (mode: {_mode}, value: {_value:N2})";
        }

        public override bool Equals(object obj)
        {
            return obj is StatModifier other
                && other._mode == _mode
                && other._value == _value;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine((int)_mode, _value);
        }
    }
}
