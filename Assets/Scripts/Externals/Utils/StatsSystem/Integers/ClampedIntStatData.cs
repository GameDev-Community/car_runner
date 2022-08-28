namespace Externals.Utils.StatsSystem
{
    public class ClampedIntStatData : ClampedInt, IStatData
    {
        private readonly StatObject _statObject;

        public ClampedIntStatData(StatObject statObject, int min, int max, int initial, int minBoundsDelta = 2)
            : base(min, max, initial, statObject.GetStatDataInfo().SaveRatio, minBoundsDelta)
        {
            _statObject = statObject;
        }


        public StatObject StatObject => _statObject;
    }
}
