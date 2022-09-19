namespace DevourDev.Unity.Application
{
    public class AppSettings
    {
        public class SoundsSettings
        {
            public event System.Action<bool> OnEnableBgmChanged;
            public event System.Action<float> OnBgmVolumeChanged;
            public event System.Action<float> OnSfxVolumeChanged;
            public event System.Action<float> OnVoiceVolumeChanged;

            private bool _enableBgm = true;
            private float _bgmVol = 0.5f;
            private float _sfxVol = 0.5f;
            private float _voiceVol = 0.5f;


            public bool EnableBgm
            {
                get => _enableBgm;
                set
                {
                    if (_enableBgm == value)
                        return;

                    _enableBgm = value;
                    OnEnableBgmChanged?.Invoke(value);
                }
            }

            public float BgmVolume
            {
                get => _bgmVol;
                set
                {
                    if (_bgmVol == value)
                        return;

                    _bgmVol = value;
                    OnBgmVolumeChanged?.Invoke(value);
                }
            }

            public float SfxVolume
            {
                get => _sfxVol;
                set
                {
                    if (_sfxVol == value)
                        return;

                    _sfxVol = value;
                    OnSfxVolumeChanged?.Invoke(value);
                }
            }

            public float VoiceVolume
            {
                get => _voiceVol;
                set
                {
                    if (_voiceVol == value)
                        return;

                    _voiceVol = value;
                    OnVoiceVolumeChanged?.Invoke(value);
                }
            }
        }


        public SoundsSettings Sounds { get; private set; }


        public AppSettings()
        {
            Sounds = new();
        }
    }
}