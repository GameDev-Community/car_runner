namespace Externals.Utils.StatsSystem
{
    public interface IFloatValueCallback : IValuable<float>
    {
        /// <summary>
        /// sender, delta
        /// </summary>
        public event System.Action<IFloatValueCallback, float> OnFloatValueChanged;
    }
}
