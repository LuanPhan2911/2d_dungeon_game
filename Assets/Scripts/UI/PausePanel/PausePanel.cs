



using UnityEngine;
using UnityEngine.UI;

public class PausePanel : Panel
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _quitButton;

    [SerializeField] private SettingPanel _settingPanel;

    private void Awake()
    {
        _continueButton.onClick.AddListener(ContinueButtonClick);
        _settingButton.onClick.AddListener(SettingButtonClick);
        _quitButton.onClick.AddListener(QuitButtonClick);
    }

    private void Start()
    {
        Hide();
    }

    public override void Hide()
    {
        GameManager.Instance.Unpause();
        base.Hide();

    }
    public override void Show()
    {
        base.Show();
        GameManager.Instance.Pause();
    }


    private void ContinueButtonClick()
    {
        Hide();
       
    }
    private void SettingButtonClick()
    {
        Hide();
        _settingPanel.Show();
    }
    private void QuitButtonClick()
    {
        Hide();
        Debug.Log("Go to main menu");
    }

}
