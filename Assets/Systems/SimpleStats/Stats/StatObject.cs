using UnityEngine;

namespace DevourDev.Unity.Utils.SimpleStats
{
    [CreateAssetMenu(menuName = "DD/Stats/Stat Object")]
    public class StatObject : ScriptableObject
    {
        [SerializeField] private string _statName;

        [Tooltip("low-res")]
        [SerializeField] private Texture2D _iconTex;
        [Tooltip("hi-res")]
        [SerializeField] private Texture2D _previewTex;


        #region accessors
        public string StatName => _statName;
        public Texture2D IconTex => _iconTex;
        public Texture2D PreviewTex => _previewTex;
        #endregion
    }
}