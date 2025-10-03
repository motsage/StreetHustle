using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    [Header("UI")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle muteToggle;

    public Button saveButton;

    [Header("Audio")]
    public AudioMixer audioMixer; // Exposed params: "MasterVol", "MusicVol", "SfxVol"

    const string KEY_MASTER = "audio.master";
    const string KEY_MUSIC  = "audio.music";
    const string KEY_SFX    = "audio.sfx";
    const string KEY_MUTE   = "audio.mute";

    void Awake()
    {
        if (saveButton) saveButton.onClick.AddListener(SaveSettings);

        // Load saved values into UI on start
        LoadSettings();
    }

    void LoadSettings()
    {
        float master = PlayerPrefs.GetFloat(KEY_MASTER, 1f);
        float music  = PlayerPrefs.GetFloat(KEY_MUSIC, 0.8f);
        float sfx    = PlayerPrefs.GetFloat(KEY_SFX, 0.9f);
        bool mute    = PlayerPrefs.GetInt(KEY_MUTE, 0) == 1;

        if (masterSlider) masterSlider.value = master;
        if (musicSlider)  musicSlider.value  = music;
        if (sfxSlider)    sfxSlider.value    = sfx;
        if (muteToggle)   muteToggle.isOn    = mute;

        ApplySettings(master, music, sfx, mute);
    }

    public void SaveSettings()
    {
        float master = masterSlider ? masterSlider.value : 1f;
        float music  = musicSlider  ? musicSlider.value  : 0.8f;
        float sfx    = sfxSlider    ? sfxSlider.value    : 0.9f;
        bool mute    = muteToggle   && muteToggle.isOn;

        PlayerPrefs.SetFloat(KEY_MASTER, master);
        PlayerPrefs.SetFloat(KEY_MUSIC,  music);
        PlayerPrefs.SetFloat(KEY_SFX,    sfx);
        PlayerPrefs.SetInt(KEY_MUTE, mute ? 1 : 0);
        PlayerPrefs.Save();

        ApplySettings(master, music, sfx, mute);
    }

    void ApplySettings(float master, float music, float sfx, bool mute)
    {
        if (!audioMixer) return;

        audioMixer.SetFloat("MasterVol", mute ? -80f : LinearToDecibel(master));
        audioMixer.SetFloat("MusicVol", LinearToDecibel(music));
        audioMixer.SetFloat("SfxVol",   LinearToDecibel(sfx));
    }

    float LinearToDecibel(float value)
    {
        if (value <= 0.0001f) return -80f;
        return Mathf.Log10(value) * 20f;
    }
}