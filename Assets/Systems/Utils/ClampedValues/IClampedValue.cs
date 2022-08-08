namespace Utils
{
    public interface IClampedValue
    {
        public DynamicValue DynamicValue { get; }


        public DynamicValue TryChange(DynamicValue delta, out bool reachedMin, out bool reachedMax);
        public void Change(DynamicValue delta);
    }
}
