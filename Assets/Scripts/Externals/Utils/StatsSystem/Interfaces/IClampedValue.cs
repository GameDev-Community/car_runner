namespace Externals.Utils.StatsSystem
{
    public interface IClampedValue<TValue>
    {
        public TValue Min { get; }
        public TValue Max { get; }
        public TValue Value { get; }
    }
}