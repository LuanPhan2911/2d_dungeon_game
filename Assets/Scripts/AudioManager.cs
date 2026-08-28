using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{




    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _musicAudioSource;

    [SerializeField] private AudioSource _audioSourcePrefab;

    [SerializeField] private AudioMixer _mainMixer;

    const string MUSIC_VOLUME = "MusicVolume";
    const string SOUND_FX_VOLUME = "SoundFXVolume";

    private float _musicVolume=0.5f;
    private float _soundFxVolume=0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;

        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetMusicVolume(_musicVolume);
        SetSoundFxVolume(_soundFxVolume);

        GameManager.Instance.OnGamePauseChanged += GameManager_OnGamePauseChanged;
    }

    private void GameManager_OnGamePauseChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePaused)
        {
            _musicAudioSource.Stop();
        }
        else
        {
            _musicAudioSource.Play();
        }
    }

    public void Play(AudioClip audioClip, Vector3 position)
    {
        // Instantiate new audio source
        AudioSource audioSource = Instantiate(_audioSourcePrefab, position, Quaternion.identity);

        // assign audio clip and volume
        audioSource.clip= audioClip;
        // play sound
        audioSource.Play();
        // get duration sound and destroy audio source
        float clipLength= audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength );
    }
    public void Play(AudioClip[] audioClips, Vector3 position)
    {
        AudioClip audioClip = audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
        Play(audioClip, position);
    }

    
  
    public void SetMusicVolume(float volume)
    {
        _musicVolume= volume;
        _mainMixer.SetFloat(MUSIC_VOLUME, ConverTodBValue(volume));
       
    }
    public void SetSoundFxVolume(float volume)
    {
        _soundFxVolume= volume;
        _mainMixer.SetFloat(SOUND_FX_VOLUME, ConverTodBValue(volume));
    }
    private float ConverTodBValue(float sliderValue)
    {
        return Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
    }
    public float GetMusicVolume()
    {
        return _musicVolume;
    }
    public float GetSoundFxVolume()
    {
        return _soundFxVolume;
    }
   
}
