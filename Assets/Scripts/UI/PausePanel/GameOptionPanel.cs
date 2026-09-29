using UnityEngine;
using UnityEngine.UI;

public class GameOptionPanel : MonoBehaviour
{
    [SerializeField] private Button _controllerButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _mainMenuButon;

    [SerializeField] private SoundControlPanel _soundControlPanel;
    [SerializeField] private ControllerPanel _controllerPanel;


    private void OnEnable()
    {
        _controllerButton.onClick.AddListener(ControllerButtonClick);
        _soundButton.onClick.AddListener(SoundButtonClick);
       
    }
    private void OnDisable()
    {
        _controllerButton.onClick.RemoveListener(ControllerButtonClick);
        _soundButton.onClick.RemoveListener(SoundButtonClick);
        
    }

    private void ControllerButtonClick()
    {
        Hide();
       
        _controllerPanel.Show();
    }
    private void SoundButtonClick()
    {
        Hide();
       
        _soundControlPanel.Show();
    }
  
    public void Show()
    {
        gameObject.SetActive(true);
        _controllerButton.Select();

        _soundControlPanel.Hide();
        _controllerPanel.Hide();

    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
