using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _loadSavedGameButton;


    [SerializeField] private SavedGameUI _savedGameUI;
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        _newGameButton.onClick.AddListener(OnNewGameClicked);

        _loadSavedGameButton.onClick.AddListener(OnLoadSavedGameClicked);
    }

    private void OnNewGameClicked()
    {
        GameManager.Instance.NewGame();
    }
    private void OnLoadSavedGameClicked()
    {
        // load list saved games UI
        _savedGameUI.Show();


    }
}
