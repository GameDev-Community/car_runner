using DevourDev.Base.Reflections;
using Externals.Utils.StatsSystem;
using System.Reflection;

namespace Game.Stats
{

    // всё хуйня, чтобы нормально сделать - надо всё переделывать
    // обойдёмся говном ебаным, ниче страшного
    //[System.Serializable]
    //public class SerializableStatData
    //{
    //    private const string _statDataFieldName = "_statObject";

    //    [UnityEngine.SerializeField] private IStatData _sd;
    //    [UnityEngine.SerializeField] private int _statObjectId;


    //    public SerializableStatData(IStatData sd)
    //    {
    //        _sd = sd;
    //        _statObjectId = _sd.StatObject.DatabaseElementID;
    //    }


    //    public IStatData Deserialize(StatObjectsDatabase db)
    //    {
    //        var ret = _sd;
    //        var soFI = ret.GetType().GetField(_statDataFieldName, BindingFlags.Instance | BindingFlags.NonPublic);
    //        soFI.SetValue(ret, db.GetElement(_statObjectId));
    //        return ret;
    //    }
    //}
}
