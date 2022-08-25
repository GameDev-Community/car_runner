namespace Externals.Utils.StatsSystem
{
    public interface IClampedAmountManipulatable<TValue>
    {
        public bool CanAddExact(TValue delta);

        public bool CanRemoveExact(TValue delta);

        public bool CanChangeExact(TValue delta);


        /// <returns>final delta</returns>
        public TValue AddSafe(TValue delta);

        /// <returns>final delta</returns>
        public TValue RemoveSafe(TValue delta);

        ///<param name="inversed">from x to -x</param>
        /// <returns>final delta</returns>
        public TValue ChangeSafe(TValue delta, bool inversed = false);
        public TValue SetSafe(TValue value);
    }
}