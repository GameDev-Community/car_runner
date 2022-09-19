using UnityEngine;

namespace DevourDev.Unity.Application
{
    public class AppManager : MonoBehaviour
    {
        private static AppManager _inst;
        private static AppSettings _activeSettings;

        private static readonly System.Random _r = new();


        public static AppSettings ActiveSettings => _activeSettings;
        public static System.Random Random => _r;



        private void Awake()
        {
            _inst = this;
            InitSettings();
        }

        private void InitSettings()
        {
            _activeSettings = new();
        }
    }
}