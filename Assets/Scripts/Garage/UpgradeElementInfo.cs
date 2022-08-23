using Externals.Utils.StatsSystem;

namespace Garage
{
    [System.Serializable]
    internal struct UpgradeElementInfo
    {
        public int Price;
        public StatModifierCreator[] Modifiers; // заменить на более примитивную структуру, если нужно
    }
}