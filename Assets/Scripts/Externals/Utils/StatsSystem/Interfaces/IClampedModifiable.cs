namespace Externals.Utils.StatsSystem
{
    public interface IClampedModifiable<TValue>
        where TValue : struct
    {
        public TValue? MinModifiableValue { get; set; }
        public TValue? MaxModifiableValue { get; set; }


        public void SetModifiableClamps(TValue? min, TValue? max);
    }

}