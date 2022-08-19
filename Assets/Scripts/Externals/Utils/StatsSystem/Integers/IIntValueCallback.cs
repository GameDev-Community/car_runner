namespace Externals.Utils.StatsSystem
{
    public interface IIntValueCallback : IValuable<int>
    {
        public event System.Action<IIntValueCallback, int> OnIntValueChanged;
    }
}
