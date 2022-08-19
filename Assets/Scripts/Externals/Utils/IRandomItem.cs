namespace Utils
{
    public interface IRandomItem<T>
    {
        public T Item { get; }
        public int Chance { get; }
    }

}