using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    
    [SerializeField] private Button _closeButton;

    [SerializeField] private GameOptionPanel _gameOptionPanel;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(CloseButtonClick);
    }
    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(CloseButtonClick);
    }

    private void Start()
    {
        GameManager.Instance.OnGamePauseChanged += GameManager_OnGamePauseChanged;

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _gameOptionPanel.Show();
        
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    private void CloseButtonClick()
    {
        GameManager.Instance.Unpause();
    }
    private void GameManager_OnGamePauseChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePaused)
        {
            Show();
        }
        else
        {
            Hide();

        }
    }
}
