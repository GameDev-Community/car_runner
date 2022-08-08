using UnityEngine;
using Utils;

namespace Game.Stats
{
    [CreateAssetMenu(menuName = "Game/Stats/Stat Object")]
    public class StatObject : ScriptableObject
    {
        [SerializeField] private string _statName;
        [SerializeField] private string _description;
        [SerializeField] private Texture2D _iconTex;
        //hi-res texture?

        [Space]
        [SerializeField] private MeasuringMode _measuringMode;


        public string StatName => _statName;
        public string Description => _description;
        public Texture2D IconTex => _iconTex;
    }

}