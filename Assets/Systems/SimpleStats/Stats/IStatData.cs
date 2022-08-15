namespace DevourDev.Unity.Utils.SimpleStats
{
    public interface IStatData
    {
        /// <summary>
        /// sender, dirty delta, safe delta
        /// </summary>
        public event System.Action<IModifiableStatData, float, float> OnValueChanged;


        public StatObject StatObject { get; }
        public float Value { get; }


        public void ChangeValue(float delta);
    }

}