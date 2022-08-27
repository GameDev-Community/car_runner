using System.Numerics;

namespace Externals.Utils.StatsSystem
{
    public interface IClampedBoundsManipulatable<TValue> : IClampedValue<TValue>
    {

        public void SetBounds(TValue newMin, TValue newMax, TValue newCur);
        public void SetBounds(TValue newMin, TValue newMax);
    }
}