namespace Utils
{
    [System.Serializable]
    public struct DynamicValue
    {
        public MeasuringMode MeasuringMode;
        public int IntValue;
        public float FloatValue;


        public DynamicValue(int intValue)
        {
            MeasuringMode = MeasuringMode.Integer;
            IntValue = intValue;
            FloatValue = 0;
        }


        public DynamicValue(float floatValue)
        {
            MeasuringMode = MeasuringMode.Float;
            IntValue = 0;
            FloatValue = floatValue;
        }


        public static implicit operator int(DynamicValue x)
        {
            return x.IntValue;
        }

        public static implicit operator float(DynamicValue x)
        {
            return x.FloatValue;
        }

        public static implicit operator DynamicValue(int x)
        {
            return new(x);
        }

        public static implicit operator DynamicValue(float x)
        {
            return new(x);
        }
    }
}
