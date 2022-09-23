using Externals.Utils.Valuables;
using UnityEditor.Build;

namespace Externals.Utils.StatsSystem
{
    public interface IClampedAmountManipulatable<TValue> : IAmountManipulatable<TValue>
    {
        public bool CanAddExact(TValue delta, out TValue result);

        public bool CanRemoveExact(TValue delta, out TValue result);

        public bool CanChangeExact(TValue delta, out TValue result, bool inverse);

        public bool CanSetExact(TValue value, out TValue result);


        /// <returns>final delta</returns>
        public TValue AddSafe(TValue delta);

        /// <returns>final delta</returns>
        public TValue RemoveSafe(TValue delta);

        ///<param name="inversed">from x to -x</param>
        /// <returns>final delta</returns>
        public TValue ChangeSafe(TValue delta, bool inversed = false);
        /// <returns>final value</returns>
        public TValue SetSafe(TValue value);


        bool IAmountManipulatable<TValue>.CanChange(TValue delta, out TValue result, bool inverse)
        {
            return CanChangeExact(delta, out result, inverse);
        }

        bool IAmountManipulatable<TValue>.CanAdd(TValue delta, out TValue result)
        {
            return CanAddExact(delta, out result);
        }

        bool IAmountManipulatable<TValue>.CanRemove(TValue delta, out TValue result)
        {
            return CanRemoveExact(delta, out result);
        }

        bool IAmountManipulatable<TValue>.CanSet(TValue value, out TValue result)
        {
            return CanSetExact(value, out result);
        }

        void IAmountManipulatable<TValue>.Change(TValue delta, bool inverse)
        {
            _ = ChangeSafe(delta, inverse);
        }

        void IAmountManipulatable<TValue>.Add(TValue delta)
        {
            _ = AddSafe(delta);
        }

        void IAmountManipulatable<TValue>.Remove(TValue delta)
        {
            _ = RemoveSafe(delta);
        }

        void IAmountManipulatable<TValue>.Set(TValue value)
        {
            _ = SetSafe(value);
        }

        bool IAmountManipulatable<TValue>.TryChange(TValue delta, bool inverse)
        {
            if (CanChangeExact(delta, out var result, inverse))
            {
                _ = SetSafe(result);
                return true;
            }

            return false;
        }

        bool IAmountManipulatable<TValue>.TryAdd(TValue delta)
        {
            if (CanAddExact(delta, out var result))
            {
                _ = SetSafe(result);
                return true;
            }

            return false;
        }

        bool IAmountManipulatable<TValue>.TryRemove(TValue delta)
        {
            if (CanRemoveExact(delta, out var result))
            {
                _ = SetSafe(result);
                return true;
            }

            return false;
        }

        bool IAmountManipulatable<TValue>.TrySet(TValue value)
        {
            if (CanSetExact(value, out var result))
            {
                _ = SetSafe(result);
                return true;
            }

            return false;
        }
    }
}