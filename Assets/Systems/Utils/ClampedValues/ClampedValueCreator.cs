namespace Utils
{
    [System.Serializable]
    public struct ClampedValueCreator
    {
        public MeasuringMode MeasuringMode;
        public float MinF, MaxF, InitialF;
        public int MinI, MaxI, InitialI;


        public IClampedValue Create()
        {
            return MeasuringMode switch
            {
                MeasuringMode.Float => new ClampedFloat(MinF, MaxF, InitialF),
                MeasuringMode.Integer => new ClampedInt(MinI, MaxI, InitialI),
                _ => throw new System.Exception("unexpected enum value: " + MeasuringMode),
            };
        }
    }
}
