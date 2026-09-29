using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundControlPanel : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundFxSlider;
   
    private void OnEnable()
    {
        
        _musicSlider.onValueChanged.AddListener(MusicSliderChange);
        _soundFxSlider.onValueChanged.AddListener(SoundFxSliderChange);
    }
    private void OnDisable()
    {
        _musicSlider.onValueChanged.RemoveListener(MusicSliderChange);
        _soundFxSlider.onValueChanged.RemoveListener(SoundFxSliderChange);
       
    }
    private void Start()
    {
        _musicSlider.value= AudioManager.Instance.GetMusicVolume();
        _soundFxSlider.value= AudioManager.Instance.GetSoundFxVolume();
       
    }
    private void MusicSliderChange(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }
    private void SoundFxSliderChange(float value)
    {
        AudioManager.Instance.SetSoundFxVolume(value);
    }
    
 
    

    public void Show()
    {
        gameObject.SetActive(true);
        _musicSlider.Select();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
