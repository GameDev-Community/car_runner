namespace Game.StatsBehaviours
{
    [System.Obsolete("reimplementing for new stats system", true)]
    public class RocketsBehaviour
    {
        //[SerializeField] private Core.CarController2 _controller;
        //[SerializeField] private RocketMissile[] _rocketsPrefabs;
        //[SerializeField] private Transform _rocketsSpawnPoint;
        ////?
        //[SerializeField] private UnityEvent _onRocketLaunched; //?

        //[SerializeField] TMPro.TextMeshProUGUI _rocketsTex;


        //protected override void Start()
        //{
        //    base.Start();

        //    Cint_OnValueChanged(StatData, 0, 0);
        //    StatData.OnValueChanged += Cint_OnValueChanged;
        //}

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.L))
        //        LaunchRocket();
        //}


        //public void LaunchRocket()
        //{
        //    var racer = Racer;

        //    if (!racer.TryGetStatData(StatObject, out var sdata))
        //    {
        //        throw new System.Collections.Generic.KeyNotFoundException(nameof(StatObject));
        //    }

        //    var ci = (ClampedInt)sdata;

        //    if (!ci.CanChange(-1))
        //    {
        //        return;
        //    }

        //    ci.Change(-1);

        //    var l = _rocketsPrefabs.Length;

        //    if (l == 0)
        //    {
        //        Debug.Log("no rockets prefab");
        //        return;
        //    }

        //    var ri = UnityEngine.Random.Range(0, l);
        //    var r = Instantiate(_rocketsPrefabs[ri], _rocketsSpawnPoint.position,
        //        _rocketsSpawnPoint.rotation, null);

        //    var tr = Racer.transform;
        //    r.Init(tr.position + tr.forward * 10);
        //    r.Speed += _controller.CurSpeed;

        //}

        //private void Cint_OnValueChanged(ClampedInt se, int dd, int sd)
        //{
        //    _rocketsTex.text = $"{StatObject.StatName}: {se.Value}";
        //}
    }
}
