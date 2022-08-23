using UnityEngine;

namespace Garage
{
    [System.Serializable]
    internal struct CarUpgradeElement
    {
        public string Name;
        public Texture UITexture;
        public UpgradeElementInfo[] UpgradeElementInfos;
    }
}