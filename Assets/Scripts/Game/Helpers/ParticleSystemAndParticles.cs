using UnityEngine;

namespace Game.Helpers
{

    [System.Serializable]
    public class ParticleSystemAndParticles
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [Header("Либо готовый материал")]
        [SerializeField] private Material _material;
        [SerializeField] private bool _useMaterialAndChangeTexture = true;
        [Header("Либо текстуру, чтобы материал был создан в рантайме")]
        [SerializeField] private Texture2D _particlesTex;
        [SerializeField] private bool _transparent;


        public ParticleSystem InstantiateVfx(Vector3? worldPos, bool autoplay)
        {
            bool needDesposing;
            Material mat;

            if (_material != null)
            {
                if (_useMaterialAndChangeTexture)
                {
                    mat = new Material(_material)
                    {
                        mainTexture = _particlesTex
                    };
                    needDesposing = true;
                }
                else
                {
                    mat = _material;
                    needDesposing = false;
                }
            }
            else
            {
                throw new System.NotImplementedException($"Accessor with material reference/shader needed");

                mat = null;
                mat.mainTexture = _particlesTex;
                needDesposing = true;
            }

            ParticleSystem vfx;
            if (worldPos.HasValue)
            {
                vfx = GameObject.Instantiate(_particleSystem, worldPos.Value, Quaternion.identity);
            }
            else
            {
                vfx = GameObject.Instantiate(_particleSystem);
            }

            var dc = vfx.gameObject.AddComponent<Utils.DestroyCallback>();
            dc.Init(() =>
            {
                if (needDesposing)
                    GameObject.Destroy(mat);
            });

            if (autoplay)
                vfx.Play(true);

            return vfx;
        }
    }
}