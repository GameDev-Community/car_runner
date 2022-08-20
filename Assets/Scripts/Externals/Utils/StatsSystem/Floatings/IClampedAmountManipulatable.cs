namespace Externals.Utils.StatsSystem
{
    internal interface IClampedAmountManipulatable<T>
    {
        public bool CanAddExact(T delta);

        public bool CanRemoveExact(T delta);

        public bool CanChangeExact(T delta);


        /// <returns>final delta</returns>
        public T AddSafe(T delta);

        /// <returns>final delta</returns>
        public T RemoveSafe(T delta);

        ///<param name="inversed">from x to -x</param>
        /// <returns>final delta</returns>
        public T ChangeSafe(T delta, bool inversed = false);
        public T SetSafe(T value);
    }
}