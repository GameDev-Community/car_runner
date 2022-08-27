using Externals.Utils.Valuables;

namespace Externals.Utils.StatsSystem
{
    public interface IValueCallback<TValue> : IValuable<TValue>
    {
        /// <summary>
        /// sender, raw delta, safe delta
        /// </summary>
        public event System.Action<IValueCallback<TValue>, TValue> OnValueChanged;
    }
}
