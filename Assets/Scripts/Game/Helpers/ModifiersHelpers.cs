using Externals.Utils.StatsSystem;
using Externals.Utils.StatsSystem.Modifiers;

namespace Game.Interactables
{
    public static class ModifiersHelpers
    {
        public static void ApplyModifiers(StatModifierCreator[] modifiersCreator, StatsCollection statsCollection)
        {
            using Utils.AdaptiveCollection<StatObject> adaptiveCollection = new(modifiersCreator.Length);

            foreach (var mc in modifiersCreator)
            {
                var sobj = mc.StatObject;
                adaptiveCollection.AddUnique(sobj);
                var check = statsCollection.TryGetStatDataT<IModifiableStatData>(sobj, out var msd);
#if UNITY_EDITOR
                if (!check)
                    throw new System.Collections.Generic.KeyNotFoundException(sobj.name);
#endif
                msd.StatModifiers.AddModifier(mc.Create(), mc.Amount);
            }

            foreach (var item in adaptiveCollection)
            {
                var check = statsCollection.TryGetStatDataT<IModifiableStatData>(item, out var msd);
#if UNITY_EDITOR
                if (!check)
                    throw new System.Collections.Generic.KeyNotFoundException(item.name);
#endif
                msd.StatModifiers.FinishAddingModifiers();
            }
        }

        public static void ApplyModifiers(StatModifierCreator[] modifiersCreator, StatsCollection statsCollection,
            out Utils.AdaptiveCollection<StatObject> adaptiveCollection)
        {
            adaptiveCollection = new(modifiersCreator.Length);

            foreach (var mc in modifiersCreator)
            {
                var sobj = mc.StatObject;
                adaptiveCollection.AddUnique(sobj);
                var check = statsCollection.TryGetStatDataT<IModifiableStatData>(sobj, out var msd);
#if UNITY_EDITOR
                if (!check)
                    throw new System.Collections.Generic.KeyNotFoundException(sobj.name);
#endif
                msd.StatModifiers.AddModifier(mc.Create(), mc.Amount);
            }

            foreach (var item in adaptiveCollection)
            {
                var check = statsCollection.TryGetStatDataT<IModifiableStatData>(item, out var msd);
#if UNITY_EDITOR
                if (!check)
                    throw new System.Collections.Generic.KeyNotFoundException(item.name);
#endif
                msd.StatModifiers.FinishAddingModifiers();
            }
        }

        public static void DisapplyModifiers(StatModifierCreator[] modifiersCreator, StatsCollection statsCollection)
        {
            using Utils.AdaptiveCollection<StatObject> adaptiveCollection = new(modifiersCreator.Length);

            foreach (var mc in modifiersCreator)
            {
                var sobj = mc.StatObject;
                adaptiveCollection.AddUnique(sobj);
                var check = statsCollection.TryGetStatDataT<IModifiableStatData>(sobj, out var msd);
#if UNITY_EDITOR
                if (!check)
                    throw new System.Collections.Generic.KeyNotFoundException(sobj.name);
#endif
                msd.StatModifiers.RemoveModifier(mc.Create(), mc.Amount);
            }

            foreach (var item in adaptiveCollection)
            {
                var check = statsCollection.TryGetStatDataT<IModifiableStatData>(item, out var msd);
#if UNITY_EDITOR
                if (!check)
                    throw new System.Collections.Generic.KeyNotFoundException(item.name);
#endif
                msd.StatModifiers.FinishRemovingModifiers();
            }
        }

        public static void DisapplyModifiers(StatModifierCreator[] modifiersCreator, StatsCollection statsCollection,
            Utils.AdaptiveCollection<StatObject> adaptiveCollection)
        {
            foreach (var mc in modifiersCreator)
            {
                var sobj = mc.StatObject;
                adaptiveCollection.AddUnique(sobj);
                var check = statsCollection.TryGetStatDataT<IModifiableStatData>(sobj, out var msd);
#if UNITY_EDITOR
                if (!check)
                    throw new System.Collections.Generic.KeyNotFoundException(sobj.name);
#endif
                msd.StatModifiers.RemoveModifier(mc.Create(), mc.Amount);
            }

            foreach (var item in adaptiveCollection)
            {
                var check = statsCollection.TryGetStatDataT<IModifiableStatData>(item, out var msd);
#if UNITY_EDITOR
                if (!check)
                    throw new System.Collections.Generic.KeyNotFoundException(item.name);
#endif
                msd.StatModifiers.FinishRemovingModifiers();
            }

            adaptiveCollection.Dispose();
        }
    }
}