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

        /// <returns>final delta</returns>
        public T ChangeSafe(T delta);
        public T SetSafe(T value);
    }
}