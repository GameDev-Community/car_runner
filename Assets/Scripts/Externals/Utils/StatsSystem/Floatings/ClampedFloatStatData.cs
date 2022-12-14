namespace Externals.Utils.StatsSystem
{
    public class ClampedFloatStatData : ClampedFloat, IStatData
    {
        private readonly StatObject _statObject;


        public ClampedFloatStatData(StatObject statObject, float min, float max, float initial, float minBoundsDelta = 1e-10f)
            : base(min, max, initial, statObject.GetStatDataInfo().SaveRatio, minBoundsDelta)
        {
            _statObject = statObject;
        }


        public StatObject StatObject => _statObject;
    }
}
