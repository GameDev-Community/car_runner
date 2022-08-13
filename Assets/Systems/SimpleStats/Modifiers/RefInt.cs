namespace DevourDev.Unity.Utils.SimpleStats.Modifiers
{
    public sealed class RefInt
    {
        public int Value;


        public RefInt(int value)
        {
            Value = value;
        }


        public void Add(int v)
            => Value += v;


        public void Set(int v)
            => Value = v;


        public static implicit operator int(RefInt v)
        {
            return v.Value;
        }
    }
}

