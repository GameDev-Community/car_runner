namespace Externals.Utils.StatsSystem.Modifiers
{
    public interface IModifiableStatData : IStatData
    {
        public StatModifiersCollection StatModifiers { get; }
        //члены ниже переехали в StatModifiersCollection

        //public bool ContainsModifier(StatModifier m);
        //public bool TryGetModifierAmount(StatModifier m, out int amount);
        ///// <summary>
        ///// Requires calling FinishAdding
        ///// to apply changes
        ///// </summary>
        //public void AddModifier(StatModifier m, int amount);
        ///// <summary>
        ///// Changes applying instantly, no
        ///// need to call FinishRemoving
        ///// after calling TryRemove
        ///// </summary>
        //public bool TryRemoveModifier(StatModifier m, int amount);
        ///// <summary>
        ///// Requires calling FinishRemoving 
        ///// to apply changes
        ///// </summary>
        //public void RemoveModifier(StatModifier m, int amount);
        //public void FinishAddingModifiers();
        //public void FinishRemovingModifiers();

        ///// <summary>
        ///// COLLECTION COPY
        ///// </summary>
        ///// <returns></returns>
        //public (StatModifier, int)[] GetModifiers();

        ///// <summary>
        ///// COLLECTION REFERENCE
        ///// </summary>
        ///// <returns></returns>
        //public CountingDictionary<StatModifier> GetModifiersDictionary();
    }
}
