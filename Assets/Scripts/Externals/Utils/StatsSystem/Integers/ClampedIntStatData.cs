namespace Externals.Utils.StatsSystem
{
    public class ClampedIntStatData : ClampedInt, IStatData
    {
        private readonly StatObject _statObject;

        public ClampedIntStatData(StatObject statObject, int min, int max, float initial, bool ratio, bool saveRatio, int minBoundsDelta = 1)
            : base(min, max, initial, ratio, saveRatio, minBoundsDelta)
        {
            _statObject = statObject;
        }


        public StatObject StatObject => _statObject;
    }
}
