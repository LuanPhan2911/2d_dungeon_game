
using UnityEngine;


public class SoundTabContent : TabContent
{
    [SerializeField] private RangeSliderWithValue _musicSlider;
    [SerializeField] private RangeSliderWithValue _soundFxSlider;

   
   
    private  void Start()
    {
        _musicSlider.SetValue(AudioManager.Instance.GetMusicVolume() * _musicSlider.MaxValue);
        _soundFxSlider.SetValue(AudioManager.Instance.GetSoundFxVolume() * _soundFxSlider.MaxValue);

        _musicSlider.OnValueChanged += MusicSliderChange;
        _soundFxSlider.OnValueChanged += SoundFxSliderChange;
    }
    private void MusicSliderChange(float value)
    {
        AudioManager.Instance.SetMusicVolume(value / _musicSlider.MaxValue);
    }
    private void SoundFxSliderChange(float value)
    {
        AudioManager.Instance.SetSoundFxVolume(value / _soundFxSlider.MaxValue);
    }
}
