using DevourDev.Base.Math;

namespace DevourDev.Unity.Utils.SimpleStats.Modifiers
{
    /// <summary>
    /// Мб представить Max в виде самостоятельных Modifiable статов.
    /// Соответственно, нужно будет добавить protected обработчиков для
    /// обрабатывания модификаций. Например, добавили модифаер на + 400
    /// плоского значения - максимальный лимит увеличился на 400
    /// вместе с основным значением.
    /// Что делать с Минимумом - не очевидно. Пока что можно оставить.
    /// 
    /// Еще не стоит забывать, что оригинальная система динамических
    /// статов не подразумевала влияние статов напрямую на Current - 
    /// вместо этого Modifiable статой считалась лишь Max стата, а
    /// Current изменялась разве что при saveRatio. Здесь, возможно,
    /// будет лучше сделать так же либо похожим образом.
    /// </summary>
    [System.Obsolete("Не очевидно поведение границ при модификации модификаторами.", true)]
    public class ModifiableMinMaxData : ModifiableStatData
    {
        /// <summary>
        /// sender, dirty delta, safe delta
        /// </summary>
        public event System.Action<ModifiableMinMaxData, float, float> OnMinLimitReached;
        /// <summary>
        /// sender, dirty delta, safe delta
        /// </summary>
        public event System.Action<ModifiableMinMaxData, float, float> OnMaxLimitReached;
        /// <summary>
        /// sender, prev value
        /// </summary>
        public event System.Action<ModifiableMinMaxData, float> OnMinLimitChanged;
        /// <summary>
        /// sender, prev value
        /// </summary>
        public event System.Action<ModifiableMinMaxData, float> OnMaxLimitChanged;

        private float _min;
        private float _max;


        public ModifiableMinMaxData(StatObject statObject, float min, float max) : base(statObject)
        {
            _min = min;
            _max = max;
        }


        public float Min => _min;
        public float Max => _max;


        public void SetMinLimit(float newMinLimit, bool saveRatio)
        {
            var prev = _min;
            _min = newMinLimit;

            OnMinLimitChanged?.Invoke(this, prev);

            if (saveRatio)
            {
                float newV = MathModule.SaveRatio(Source, prev, _max, _min, _max);
                SetSourceValue(newV);
            }
        }

        public void SetMaxLimit(float newMaxLimit, bool saveRatio)
        {
            var prev = _max;
            _max = newMaxLimit;

            OnMaxLimitChanged?.Invoke(this, prev);

            if (saveRatio)
            {
                float newV = MathModule.SaveRatio(Source, _min, prev, _min, _max);
                SetSourceValue(newV);
            }
        }

        protected override float ProcessSourceValue(float raw)
        {
            return System.Math.Clamp(raw, _min, _max);
        }

        protected override float ProcessModifiedValue(float raw)
        {
            return System.Math.Clamp(raw, _min, _max);
        }
    }
}

