using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    [SerializeField]
    AudioMixer _mixer;

    [SerializeField]
    AudioSource _musicPlayer;

    [Header("Sound Effects")]
    public AudioClip HighPopClip;
    public AudioClip LowPopClip;
    public AudioClip Click1Clip;
    public AudioClip Click2Clip;
    public AudioClip RippleClip;
    public AudioClip PowerUpClip;
    public AudioClip ClapExplosion;
    public AudioClip CameraReset;


    AudioSource _source;

    const string MUSIC_VOLUME_KEY = "Music Volume";

    public float MusicVolume
    {
        get
        {
            return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.6f);
        }

        set
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, value);
            SetMixerMusicVolume(value);
        }
    }


    const string SOUND_EFFECTS_VOLUME_KEY = "Sound Effects Volume";

    public float SoundEffectsVolume
    {
        get
        {
            return PlayerPrefs.GetFloat(SOUND_EFFECTS_VOLUME_KEY, 1);
        }

        set
        {
            PlayerPrefs.SetFloat(SOUND_EFFECTS_VOLUME_KEY, value);
            SetMixerSoundEffectsVolume(value);
        }
    }


    static Audio _instance;


    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            _source = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(this);
        }
    }


    void Start()
    {
        SetMixerMusicVolume(MusicVolume);
        SetMixerSoundEffectsVolume(SoundEffectsVolume);
    }


    void SetMixerMusicVolume(float volume)
    {
        _mixer.SetFloat(MUSIC_VOLUME_KEY, PercentToDecibels(volume));
    }


    void SetMixerSoundEffectsVolume(float volume)
    {
        _mixer.SetFloat(SOUND_EFFECTS_VOLUME_KEY, PercentToDecibels(volume));
    }


    float PercentToDecibels(float percent)
    {
        return 20 * Mathf.Log10(Mathf.Clamp(percent, 0.0001f, 1));
    }


    static void Play(AudioClip clip)
    {
        _instance._source.PlayOneShot(clip);
    }


    public static void PlayHighPop()
    {
        Play(_instance.HighPopClip);
    }


    public static void PlayLowPop()
    {
        Play(_instance.LowPopClip);
    }


    public static void PlayRipple()
    {
        Play(_instance.RippleClip);
    }


    public static void PlayClick1()
    {
        Play(_instance.Click1Clip);
    }


    public static void PlayClick2()
    {
        Play(_instance.Click2Clip);
    }


    public static void PlayPowerUp()
    {
        Play(_instance.PowerUpClip);
    }


    public static void PlayClapExplosion()
    {
        Play(_instance.ClapExplosion);
    }

    public static void PlayCameraReset()
    {
        Play(_instance.CameraReset);
    }
}
