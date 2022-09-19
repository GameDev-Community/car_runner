using UnityEngine;

namespace Externals.Utils
{

    [System.Serializable]
    public class MetaInfo
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Texture2D _lowresTex;
        [SerializeField] private Texture2D _hiresTex;


        public MetaInfo(string name, string description, Texture2D lowresTex, Texture2D hiresTex)
        {
            _name = name;
            _description = description;
            _lowresTex = lowresTex;
            _hiresTex = hiresTex;
        }


        public string Name => _name;
        public string Description => _description;
        public Texture2D LowresTex => _lowresTex;
        public Texture2D HiresTex => _hiresTex;
    }
}
