using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class SoundSetting : MonoBehaviour
    {
        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer audioMixer;

        [Header("Sliders")]
        [SerializeField] private Slider sm; // Master
        [SerializeField] private Slider sb; // BGM
        [SerializeField] private Slider ss; // SFX

        private const string MasterParameter = "Master";
        private const string BgmParameter = "BGM";
        private const string SfxParameter = "SFX";

        private const string MasterSaveKey = "Volume.Master";
        private const string BgmSaveKey = "Volume.BGM";
        private const string SfxSaveKey = "Volume.SFX";

        private const float DefaultVolume = 1f;
        private const float MinVolume = 0.0001f;
        private const float MuteDb = -80f;

        private void Awake()
        {
            SetupSlider(sm);
            SetupSlider(sb);
            SetupSlider(ss);
        }

        private void OnEnable()
        {
            if (sm != null)
                sm.onValueChanged.AddListener(SetMasterVolume);

            if (sb != null)
                sb.onValueChanged.AddListener(SetBgmVolume);

            if (ss != null)
                ss.onValueChanged.AddListener(SetSfxVolume);
        }

        private void Start()
        {
            float masterValue = PlayerPrefs.GetFloat(MasterSaveKey, DefaultVolume);
            float bgmValue = PlayerPrefs.GetFloat(BgmSaveKey, DefaultVolume);
            float sfxValue = PlayerPrefs.GetFloat(SfxSaveKey, DefaultVolume);

            if (sm != null)
                sm.SetValueWithoutNotify(masterValue);

            if (sb != null)
                sb.SetValueWithoutNotify(bgmValue);

            if (ss != null)
                ss.SetValueWithoutNotify(sfxValue);

            ApplyVolume(MasterParameter, masterValue);
            ApplyVolume(BgmParameter, bgmValue);
            ApplyVolume(SfxParameter, sfxValue);
        }

        private void OnDisable()
        {
            if (sm != null)
                sm.onValueChanged.RemoveListener(SetMasterVolume);

            if (sb != null)
                sb.onValueChanged.RemoveListener(SetBgmVolume);

            if (ss != null)
                ss.onValueChanged.RemoveListener(SetSfxVolume);

            PlayerPrefs.Save();
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.Save();
        }

        private void SetupSlider(Slider slider)
        {
            if (slider == null)
                return;

            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.wholeNumbers = false;
        }

        private void SetMasterVolume(float value)
        {
            ApplyVolume(MasterParameter, value);
            PlayerPrefs.SetFloat(MasterSaveKey, value);
            PlayerPrefs.Save();
        }

        private void SetBgmVolume(float value)
        {
            ApplyVolume(BgmParameter, value);
            PlayerPrefs.SetFloat(BgmSaveKey, value);
            PlayerPrefs.Save();
        }

        private void SetSfxVolume(float value)
        {
            ApplyVolume(SfxParameter, value);
            PlayerPrefs.SetFloat(SfxSaveKey, value);
            PlayerPrefs.Save();
        }

        private void ApplyVolume(string parameterName, float sliderValue)
        {
            if (audioMixer == null)
                return;

            float decibel = ConvertSliderValueToDecibel(sliderValue);

            bool success = audioMixer.SetFloat(parameterName, decibel);

            if (!success)
                Debug.LogWarning($"AudioMixer 파라미터 못 찾음: {parameterName}", this);
        }

        private float ConvertSliderValueToDecibel(float sliderValue)
        {
            if (sliderValue <= 0f)
                return MuteDb;

            return Mathf.Log10(Mathf.Max(sliderValue, MinVolume)) * 20f;
        }
    }
}