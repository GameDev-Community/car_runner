using Externals.Utils.Valuables;

namespace Externals.Utils.StatsSystem
{
    public interface IValueCallback<TValue> : IValuable<TValue>
    {
        public event System.Action<IValueCallback<TValue>, TValue> OnValueChanged;
    }
}
