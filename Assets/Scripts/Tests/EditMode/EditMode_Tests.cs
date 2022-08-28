using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Externals.Utils.StatsSystem.Modifiers;
using Externals.Utils.StatsSystem;
using Externals.Utils;
using Utils;

namespace Tests.EditMode
{
    public class EditMode_Tests
    {
        [Test]
        public void TestFloatStatData()
        {
            StatsCollection collection = new();
            StatObject tmpStatObj = ScriptableObject.CreateInstance<StatObject>();
            int modifiersCount = 1024;
            StatModifier[] modifiers = new StatModifier[modifiersCount];
            var rnd = new System.Random(228);

            for (int i = -1; ++i < modifiersCount;)
            {
                ModifyingMode mdfMode = RandomHelpers.RandomFloat(rnd, 0f, 1f) > 0.5f ? ModifyingMode.Flat : ModifyingMode.Multiply;
                float mdfVal = mdfMode == ModifyingMode.Flat ? RandomHelpers.RandomFloat(rnd, -20f, 21f) : RandomHelpers.RandomFloat(rnd, 0f, 1f) * 0.2f;
                StatModifier m = new StatModifier(mdfMode, mdfVal);
                modifiers[i] = m;
            }

            float sourceV = 100;
            FloatDynamicStatData dynamicStatData = new(tmpStatObj, sourceV, modifiers, 0.5f);
            collection.AddStat(tmpStatObj, dynamicStatData);

            {
                var check = collection.TryGetStatData(tmpStatObj, out var d);

                if (!check)
                    throw new System.Exception("!check");

                var fsd = (FloatDynamicStatData)d;
                Debug.Log($"stat added. {fsd}");
            }

            RandomHelpers.Shuffle<StatModifier>(modifiers, rnd);

            int c = modifiersCount / 2;
            System.Diagnostics.Stopwatch sw = new();
            sw.Start();
            for (int i = -1; ++i < c;)
            {
                var m = modifiers[i];

                collection.TryGetStatDataT<FloatDynamicStatData>(tmpStatObj, out var data);
                data.StatModifiers.RemoveModifier(m, 1);
            }

            collection.TryGetStatDataT<FloatDynamicStatData>(tmpStatObj, out var data2);
            data2.StatModifiers.FinishRemovingModifiers();
            sw.Stop();

            {
                var check = collection.TryGetStatData(tmpStatObj, out var d);

                if (!check)
                    throw new System.Exception("!check");

                var fsd = (FloatDynamicStatData)d;
                Debug.Log($"half modifiers removed. {fsd}; timer: {sw.Elapsed.TotalMilliseconds} ms");
            }


            sw.Start();
            for (int i = c - 1; ++i < modifiersCount;)
            {
                var m = modifiers[i];

                var check = collection.TryGetStatDataT<FloatDynamicStatData>(tmpStatObj, out var data);

                if (!check)
                    throw new System.Exception("!check");

                data.StatModifiers.RemoveModifier(m, 1);
            }
            sw.Stop();

            collection.TryGetStatDataT<FloatDynamicStatData>(tmpStatObj, out var data3);
            data3.StatModifiers.FinishRemovingModifiers();


            {
                var check = collection.TryGetStatData(tmpStatObj, out var d);

                if (!check)
                    throw new System.Exception("!check");

                var fsd = (FloatDynamicStatData)d;
                Debug.Log($"all modifiers removed. {fsd}" +
                $" Initial (source) V was: {sourceV}; timer: {sw.Elapsed.TotalMilliseconds} ms");
            }

            {
                var check = collection.TryGetStatData(tmpStatObj, out var d);

                if (!check)
                    throw new System.Exception("!check");

                var fsd = (FloatDynamicStatData)d;

                Assert.AreEqual(sourceV, fsd.Max);
            }


            if (tmpStatObj != null)
            {
                GameObject.DestroyImmediate(tmpStatObj);
            }

            Debug.Log($"{tmpStatObj}");


        }
        [Test]
        public void TestFloatDynamicStatData()
        {
            StatsCollection collection = new();
            StatObject tmpStatObj = ScriptableObject.CreateInstance<StatObject>();
            int modifiersCount = 1024;
            StatModifier[] modifiers = new StatModifier[modifiersCount];
            var rnd = new System.Random(228);

            for (int i = -1; ++i < modifiersCount;)
            {
                ModifyingMode mdfMode = RandomHelpers.RandomFloat(rnd, 0f, 1f) > 0.5f ? ModifyingMode.Flat : ModifyingMode.Multiply;
                float mdfVal = mdfMode == ModifyingMode.Flat ? RandomHelpers.RandomFloat(rnd, -20f, 21f) : RandomHelpers.RandomFloat(rnd, 0f, 1f) * 0.2f;
                StatModifier m = new StatModifier(mdfMode, mdfVal);
                modifiers[i] = m;
            }

            float sourceV = 100;
            FloatDynamicStatData dynamicStatData = new(tmpStatObj, sourceV, modifiers, 0.5f);
            collection.AddStat(tmpStatObj, dynamicStatData);

            {
                var check = collection.TryGetStatData(tmpStatObj, out var d);

                if (!check)
                    throw new System.Exception("!check");

                var fsd = (FloatDynamicStatData)d;
                Debug.Log($"stat added. {fsd}");
            }

            RandomHelpers.Shuffle<StatModifier>(modifiers, rnd);

            int c = modifiersCount / 2;
            System.Diagnostics.Stopwatch sw = new();
            sw.Start();
            for (int i = -1; ++i < c;)
            {
                var m = modifiers[i];

                collection.TryGetStatDataT<FloatDynamicStatData>(tmpStatObj, out var data);
                data.StatModifiers.RemoveModifier(m, 1);
            }

            collection.TryGetStatDataT<FloatDynamicStatData>(tmpStatObj, out var data2);
            data2.StatModifiers.FinishRemovingModifiers();
            sw.Stop();

            {
                var check = collection.TryGetStatData(tmpStatObj, out var d);

                if (!check)
                    throw new System.Exception("!check");

                var fsd = (FloatDynamicStatData)d;
                Debug.Log($"half modifiers removed. {fsd}; timer: {sw.Elapsed.TotalMilliseconds} ms");
            }


            sw.Start();
            for (int i = c - 1; ++i < modifiersCount;)
            {
                var m = modifiers[i];

                var check = collection.TryGetStatDataT<FloatDynamicStatData>(tmpStatObj, out var data);

                if (!check)
                    throw new System.Exception("!check");

                data.StatModifiers.RemoveModifier(m, 1);
            }
            sw.Stop();

            collection.TryGetStatDataT<FloatDynamicStatData>(tmpStatObj, out var data3);
            data3.StatModifiers.FinishRemovingModifiers();


            {
                var check = collection.TryGetStatData(tmpStatObj, out var d);

                if (!check)
                    throw new System.Exception("!check");

                var fsd = (FloatDynamicStatData)d;
                Debug.Log($"all modifiers removed. {fsd}" +
                $" Initial (source) V was: {sourceV}; timer: {sw.Elapsed.TotalMilliseconds} ms");
            }

            {
                var check = collection.TryGetStatData(tmpStatObj, out var d);

                if (!check)
                    throw new System.Exception("!check");

                var fsd = (FloatDynamicStatData)d;

                Assert.AreEqual(sourceV, fsd.Max);
            }


            if (tmpStatObj != null)
            {
                GameObject.DestroyImmediate(tmpStatObj);
            }

            Debug.Log($"{tmpStatObj}");


        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator EditMode_TestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
