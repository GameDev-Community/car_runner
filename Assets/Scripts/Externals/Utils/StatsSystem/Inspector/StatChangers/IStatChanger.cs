namespace Externals.Utils.StatsSystem
{
    public interface IStatChanger
    {
        public void Apply(StatsCollection statsCollection, bool inverse = false);
    }
}
