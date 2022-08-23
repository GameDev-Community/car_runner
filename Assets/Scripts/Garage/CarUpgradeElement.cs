using UnityEngine;

namespace Garage
{
    [System.Serializable]
    internal struct CarUpgradeElement
    {
        public string Name;
        public Sprite UISprite;
        public UpgradeElementInfo[] UpgradeElementInfos;
    }
}