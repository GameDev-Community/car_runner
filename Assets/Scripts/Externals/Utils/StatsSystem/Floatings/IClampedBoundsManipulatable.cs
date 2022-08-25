namespace Externals.Utils.StatsSystem
{
    public interface IClampedBoundsManipulatable<TValue> : IClampedValue<TValue>
    {

        public void SetBounds(TValue newMin, TValue newMax, TValue newCur);
        public void SetBounds(TValue newMin, TValue newMax);
    }

    public interface IClampedValue<TValue>
    {
        public TValue Min { get; }
        public TValue Max { get; }
        public TValue Value { get; }
    }
}