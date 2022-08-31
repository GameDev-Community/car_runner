using DevourDev.Base.Reflections;
using DevourDev.Unity.ScriptableObjects;
using DevourDev.Unity.Utils.Editor;
using DevourDev.Unity.Utils.Editor.Window;
using Externals.Utils;
using Externals.Utils.StatsSystem;
using Externals.Utils.StatsSystem.Modifiers;
using Game.Stats;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Garage
{
#if UNITY_EDITOR
    public class StatDataInitializerWindow : ExtendedEditorWindow
    {
        /// <summary>
        /// 0: FloatDynamicStatData
        /// 1: ClampedFloatStatData
        /// 2: FloatModifiableStatData
        /// 3: FloatStatData 
        /// 4: IntDynamicStatData
        /// 5: ClampedIntStatData 
        /// 6: IntModifiableStatData
        /// 7: IntStatData
        /// </summary>
        [SerializeField] private StatDataRuntimeCreator _creator;
        private System.Action<StatDataRuntimeCreator> _onCompleteCallback;


        private void OnGUI()
        {
            SerializedObject.Update();
            var soProp = PR("_statObject");
            DrawP(soProp);

            int actionID = -1;
            var statObject = (StatObject)soProp.objectReferenceValue;

            if (statObject == null)
                goto End;

            var sdInfo = statObject.GetStatDataInfo();
            bool isInteger = sdInfo.NumericsType == StatObject.NumericsType.Integer;
            bool isClamped = sdInfo.Clamped;
            bool isModifiable = sdInfo.Modifiable;

            if (isClamped)
            {
                if (isInteger)
                {
                    if (isModifiable)
                    {
                        actionID = 4;
                        FindAndDrawPR("_intDynamicStatDataCreator");
                    }
                    else
                    {
                        actionID = 5;
                        FindAndDrawPR("_clampedIntStatDataCreator");
                    }
                }
                else
                {
                    if (isModifiable)
                    {
                        actionID = 0;
                        FindAndDrawPR("_floatDynamicStatDataCreator");
                    }
                    else
                    {
                        actionID = 1;
                        FindAndDrawPR("_clampedFloatStatDataCreator");
                    }
                }
            }
            else
            {
                if (isInteger)
                {
                    if (isModifiable)
                    {
                        actionID = 6;
                        FindAndDrawPR("_intModifiableStatDataCreator");
                    }
                    else
                    {
                        actionID = 7;
                        FindAndDrawPR("_intStatDataCreator");
                    }
                }
                else
                {
                    if (isModifiable)
                    {
                        actionID = 2;
                        FindAndDrawPR("_floatModifiableStatDataCreator");
                    }
                    else
                    {
                        actionID = 3;
                        FindAndDrawPR("_floatStatDataCreator");
                    }
                }
            }

End:

            _creator.SetField("_actionID", actionID);

            Apply();

            bool add = GUILayout.Button("Add");
            bool cancel = GUILayout.Button("Cancel");

            if (add || cancel)
            {

                _onCompleteCallback.Invoke(_creator);

                Close();
                return;
            }


            //UnityEditor.SerializedProperty P(string fieldName) => SerializedObject.FindProperty(fieldName);
            UnityEditor.SerializedProperty PR(string fieldName) => SerializedObject.FindProperty(nameof(_creator)).FindPropertyRelative(fieldName);

            void DrawP(UnityEditor.SerializedProperty p) => EditorGUILayout.PropertyField(p);

            void FindAndDrawPR(string fieldName)
            {
                SerializedProperty sp = PR(fieldName);
                DrawP(sp);

                Jopa(sp);
            }

            //void FindAndDrawP(string fieldName)
            //{
            //    SerializedProperty sp = P(fieldName);
            //    DrawP(sp);

            //    Jopa(sp);
            //}

            System.Reflection.FieldInfo FI(System.Type t, string fieldName)
                => t.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);


            void Jopa(SerializedProperty sp)
            {
                object statDataCreatorObject = actionID switch
                {
                    0 => _creator.FloatDynamicStatDataCreator,
                    1 => _creator.ClampedFloatStatDataCreator,
                    2 => _creator.FloatModifiableStatDataCreator,
                    3 => _creator.FloatStatDataCreator,
                    4 => _creator.IntDynamicStatDataCreator,
                    5 => _creator.ClampedIntStatDataCreator,
                    6 => _creator.IntModifiableStatDataCreator,
                    7 => _creator.IntStatDataCreator,
                    _ => null,
                };

                if (statDataCreatorObject == null)
                    return;

                statDataCreatorObject.SetField("_statObject", statObject);

                if (isModifiable)
                {
                    var initialModifiersName = "_initialModifiers";
                    var x = sp.FindPropertyRelative(initialModifiersName);


                    if (x == null)
                    {
                        UnityEngine.Debug.LogError("initial modifiers not found but isModifiable");
                        return;
                    }

                    StatModifierCreator[] arr = statDataCreatorObject.GetFieldValue<StatModifierCreator[]>(initialModifiersName);

                    if (arr == null)
                        return;

                    var c = arr.Length;

                    if (c > 0)
                    {
                        var smcsoFI = FI(typeof(StatModifierCreator), "_statObject");

                        for (int i = -1; ++i < c;)
                        {
                            smcsoFI.SetValue(arr[i], statObject);
                        }
                    }
                }
            }
        }


        public static void Open(System.Action<StatDataRuntimeCreator> onCompleteCallback)
        {
            var w = GetWindow<StatDataInitializerWindow>("Stat Data Creator");
            w.SerializedObject = new SerializedObject(w);
            w._onCompleteCallback = onCompleteCallback;
        }
    }
    public class CarCreatorWindow : ExtendedEditorWindow
    {
        [SerializeField] private string _carName;
        [SerializeField] private string _description;
        [SerializeField] private Texture2D _lowresTex;
        [SerializeField] private Texture2D _hiresTex;
        [SerializeField] private List<StatDataRuntimeCreator> _initialStats;

        private CarCreator _creator;


        [MenuItem("Window/Car Creator")]
        public static void Open()
        {
            var w = GetWindow<CarCreatorWindow>("Car creator");
            var creator = new CarCreator();
            w._creator = creator;
            w.SerializedObject = new SerializedObject(w);
            w._initialStats = new();
        }

        private void OnDestroy()
        {
            _creator = null;
        }


        private void OnGUI()
        {
            SerializedObject.FDP(nameof(_carName));
            SerializedObject.FDP(nameof(_description));

            SerializedObject.FDP(nameof(_lowresTex));
            SerializedObject.FDP(nameof(_hiresTex));

            //todo: добавить вертикальный список:
            // (стат: название StatObject, тип StatData) (кнопка: изменить) (кнопка: удалить)
            // (стат: название StatObject, тип StatData) (кнопка: изменить) (кнопка: удалить)
            // (стат: название StatObject, тип StatData) (кнопка: изменить) (кнопка: удалить)
            // (стат: название StatObject, тип StatData) (кнопка: изменить) (кнопка: удалить)

            //todo2: добавить изменение стата выше (а не только в _initialModifiers)

            for (int i = 0; i < _initialStats.Count; i++)
            {
                StatDataRuntimeCreator sd = _initialStats[i];
                GUILayout.BeginHorizontal();

                bool flag = false;

                GUILayout.Label(sd.StatObject.MetaInfo.Name);

                if (GUILayout.Button("change"))
                {
                    flag = true;
                }

                if (GUILayout.Button("remove"))
                {
                    flag = true;

                    _initialStats.RemoveAt(i);
                }

                GUILayout.EndHorizontal();

                if (flag)
                    goto End;
            }

            if (GUILayout.Button("Create Stat"))
            {
                StatDataInitializerWindow.Open(AddStatData);
            }


            if (GUILayout.Button("Create Car Object"))
            {
                var pathToAsset = EditorUtility.SaveFilePanelInProject("Save Car Object", _carName + ".asset", "asset", "Chose new asset location");

                if (pathToAsset.Length > 0)
                {
                    Apply();
                    _creator.MetaInfo = new MetaInfo(_carName, _description, _lowresTex, _hiresTex);
                    _creator.SourceStats = _initialStats.ToArray();
                    var x = _creator.Create(pathToAsset);
                    return;
                }
            }

End:

            Apply();
        }


        private void AddStatData(StatDataRuntimeCreator statDataInitializer)
        {
            if (statDataInitializer == null)
                return;

            _initialStats.Add(statDataInitializer);
            Repaint();
        }
    }

    public class CarCreator
    {
        private MetaInfo _metaInfo;
        private StatDataRuntimeCreator[] _sourceStats;
        private UpgradeObject[] _upgrades;


        public MetaInfo MetaInfo { get => _metaInfo; set => _metaInfo = value; }
        public StatDataRuntimeCreator[] SourceStats { get => _sourceStats; set => _sourceStats = value; }
        public UpgradeObject[] Upgrades { get => _upgrades; set => _upgrades = value; }


        public CarObject Create(string pathToSaveAsset)
        {
            var carObj = ScriptableObject.CreateInstance<CarObject>();
            carObj.SetField(nameof(_metaInfo), _metaInfo);
            carObj.SetField(nameof(_sourceStats), _sourceStats);
            //carObj.SetField(nameof(_upgrades), _upgrades);
            AssetDatabase.CreateAsset(carObj, pathToSaveAsset);
            EditorUtility.SetDirty(carObj);
            return carObj;
        }
    }
#endif

    [CreateAssetMenu(menuName = "Game/Garage/Cars/Car Object")]
    public class CarObject : GameDatabaseElement
    {
        [SerializeField] private StatObjectsDatabase _statsDatabaseDEBUG;
        [SerializeField] private MetaInfo _metaInfo;
        [SerializeField, HideInInspector] private StatDataRuntimeCreator[] _sourceStats; //ahaiasidasodaspphooooooi
        [SerializeField] private GameObject _carPreviewPrefab;
        [SerializeField] private UpgradeObject[] _upgrades;
        [SerializeField] private int _statsCount;


        public MetaInfo MetaInfo => _metaInfo;


        private void OnValidate()
        {
            _statsCount = _sourceStats.Length;
            if (_statsDatabaseDEBUG == null)
                return;

            foreach (var item in _sourceStats)
            {
                var sd = item.Create();
                UnityEngine.Debug.Log(sd.GetType().ToString());

                UnityEngine.Debug.Log(sd.StatObject);

                if (sd is FloatModifiableStatData fmsd)
                {
                    UnityEngine.Debug.Log(fmsd.Value);
                }
                else if (sd is FloatDynamicStatData fdsd)
                {
                    UnityEngine.Debug.Log(fdsd.Value);
                }
            }
        }
    }

    //#if UNITY_EDITOR
    //    [CustomEditor(typeof(CarObject))]
    //    public class CarObjectEditor : Editor
    //    {
    //        //add buttons: "edit", "create new"
    //    }
    //#endif

    /// <summary>
    /// A class to allow the conversion of doubles to string representations of
    /// their exact decimal values. The implementation aims for readability over
    /// efficiency.
    /// </summary>
    public class DoubleConverter
    {
        /// <summary>
        /// Converts the given double to a string representation of its
        /// exact decimal value.
        /// </summary>
        /// <param name="d">The double to convert.</param>
        /// <returns>A string representation of the double's exact decimal value.</return>
        public static string ToExactString(double d)
        {
            if (double.IsPositiveInfinity(d))
                return "+Infinity";
            if (double.IsNegativeInfinity(d))
                return "-Infinity";
            if (double.IsNaN(d))
                return "NaN";

            // Translate the double into sign, exponent and mantissa.
            long bits = BitConverter.DoubleToInt64Bits(d);
            // Note that the shift is sign-extended, hence the test against -1 not 1
            bool negative = (bits < 0);
            int exponent = (int)((bits >> 52) & 0x7ffL);
            long mantissa = bits & 0xfffffffffffffL;

            // Subnormal numbers; exponent is effectively one higher,
            // but there's no extra normalisation bit in the mantissa
            if (exponent == 0)
            {
                exponent++;
            }
            // Normal numbers; leave exponent as it is but add extra
            // bit to the front of the mantissa
            else
            {
                mantissa = mantissa | (1L << 52);
            }

            // Bias the exponent. It's actually biased by 1023, but we're
            // treating the mantissa as m.0 rather than 0.m, so we need
            // to subtract another 52 from it.
            exponent -= 1075;

            if (mantissa == 0)
            {
                return "0";
            }

            /* Normalize */
            while ((mantissa & 1) == 0)
            {    /*  i.e., Mantissa is even */
                mantissa >>= 1;
                exponent++;
            }

            /// Construct a new decimal expansion with the mantissa
            ArbitraryDecimal ad = new ArbitraryDecimal(mantissa);

            // If the exponent is less than 0, we need to repeatedly
            // divide by 2 - which is the equivalent of multiplying
            // by 5 and dividing by 10.
            if (exponent < 0)
            {
                for (int i = 0; i < -exponent; i++)
                    ad.MultiplyBy(5);
                ad.Shift(-exponent);
            }
            // Otherwise, we need to repeatedly multiply by 2
            else
            {
                for (int i = 0; i < exponent; i++)
                    ad.MultiplyBy(2);
            }

            // Finally, return the string with an appropriate sign
            if (negative)
                return "-" + ad.ToString();
            else
                return ad.ToString();
        }

        /// <summary>Private class used for manipulating
        class ArbitraryDecimal
        {
            /// <summary>Digits in the decimal expansion, one byte per digit
            byte[] digits;
            /// <summary> 
            /// How many digits are *after* the decimal point
            /// </summary>
            int decimalPoint = 0;

            /// <summary> 
            /// Constructs an arbitrary decimal expansion from the given long.
            /// The long must not be negative.
            /// </summary>
            internal ArbitraryDecimal(long x)
            {
                string tmp = x.ToString(CultureInfo.InvariantCulture);
                digits = new byte[tmp.Length];
                for (int i = 0; i < tmp.Length; i++)
                    digits[i] = (byte)(tmp[i] - '0');
                Normalize();
            }

            /// <summary>
            /// Multiplies the current expansion by the given amount, which should
            /// only be 2 or 5.
            /// </summary>
            internal void MultiplyBy(int amount)
            {
                byte[] result = new byte[digits.Length + 1];
                for (int i = digits.Length - 1; i >= 0; i--)
                {
                    int resultDigit = digits[i] * amount + result[i + 1];
                    result[i] = (byte)(resultDigit / 10);
                    result[i + 1] = (byte)(resultDigit % 10);
                }
                if (result[0] != 0)
                {
                    digits = result;
                }
                else
                {
                    Array.Copy(result, 1, digits, 0, digits.Length);
                }
                Normalize();
            }

            /// <summary>
            /// Shifts the decimal point; a negative value makes
            /// the decimal expansion bigger (as fewer digits come after the
            /// decimal place) and a positive value makes the decimal
            /// expansion smaller.
            /// </summary>
            internal void Shift(int amount)
            {
                decimalPoint += amount;
            }

            /// <summary>
            /// Removes leading/trailing zeroes from the expansion.
            /// </summary>
            internal void Normalize()
            {
                int first;
                for (first = 0; first < digits.Length; first++)
                    if (digits[first] != 0)
                        break;
                int last;
                for (last = digits.Length - 1; last >= 0; last--)
                    if (digits[last] != 0)
                        break;

                if (first == 0 && last == digits.Length - 1)
                    return;

                byte[] tmp = new byte[last - first + 1];
                for (int i = 0; i < tmp.Length; i++)
                    tmp[i] = digits[i + first];

                decimalPoint -= digits.Length - (last + 1);
                digits = tmp;
            }

            /// <summary>
            /// Converts the value to a proper decimal string representation.
            /// </summary>
            public override String ToString()
            {
                char[] digitString = new char[digits.Length];
                for (int i = 0; i < digits.Length; i++)
                    digitString[i] = (char)(digits[i] + '0');

                // Simplest case - nothing after the decimal point,
                // and last real digit is non-zero, eg value=35
                if (decimalPoint == 0)
                {
                    return new string(digitString);
                }

                // Fairly simple case - nothing after the decimal
                // point, but some 0s to add, eg value=350
                if (decimalPoint < 0)
                {
                    return new string(digitString) +
                           new string('0', -decimalPoint);
                }

                // Nothing before the decimal point, eg 0.035
                if (decimalPoint >= digitString.Length)
                {
                    return "0." +
                        new string('0', (decimalPoint - digitString.Length)) +
                        new string(digitString);
                }

                // Most complicated case - part of the string comes
                // before the decimal point, part comes after it,
                // eg 3.5
                return new string(digitString, 0,
                                   digitString.Length - decimalPoint) +
                    "." +
                    new string(digitString,
                                digitString.Length - decimalPoint,
                                decimalPoint);
            }
        }
    }
}
