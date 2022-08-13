using DevourDev.Unity.Utils.SimpleStats.Modifiers;

namespace DevourDev.Unity.Utils.SimpleStats
{
    public interface IStatData
    {
        /// <summary>
        /// sender, dirty delta, safe delta
        /// </summary>
        public event System.Action<IStatData, float, float> OnValueChanged;


        public StatObject StatObject { get; }
        public float Value { get; }


        public void ChangeValue(float delta);
        public void AddModifier(StatModifier m, int amount, bool recalculate);
        public void RemoveModifier(StatModifier m, int amount, bool recalculate);
        public void FinishAddingModifiers();
        public void FinishRemovingModifiers();

        public (StatModifier, int)[] GetModifiers();
    }

}