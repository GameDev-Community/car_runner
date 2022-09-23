namespace Externals.Utils.Valuables
{
    public interface IAmountManipulatable<TValue> : IValuable<TValue>
    {
        public bool CanChange(TValue delta, out TValue result, bool inverse);
        public bool CanAdd(TValue delta, out TValue result);
        public bool CanRemove(TValue delta, out TValue result);
        public bool CanSet(TValue value, out TValue result);

        public void Change(TValue delta, bool inverse = false);
        public void Add(TValue delta);
        public void Remove(TValue delta);
        public void Set(TValue value);

        public bool TryChange(TValue delta, bool inverse);
        public bool TryAdd(TValue delta);
        public bool TryRemove(TValue delta);
        public bool TrySet(TValue value);
    }
}
