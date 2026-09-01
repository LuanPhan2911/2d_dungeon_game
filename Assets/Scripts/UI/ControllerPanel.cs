using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControllerPanel : MonoBehaviour
{
    [SerializeField] private Button _upButton;
    [SerializeField] private Button _downButton;

    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Button _jumpButton;


    [SerializeField] private TextMeshProUGUI _upBindingText, _downBindingText, 
        _leftBindingText, _rightBindingText, _jumpBindingText;

    [SerializeField] private Image _rebindingOverlayImage;


    private void OnEnable()
    {
        _upButton.onClick.AddListener(() => RebindingAction(GameInputManager.Instance.PlayerMoveAction, _upBindingText, GameInputManager.UpBindingIndex));
        _downButton.onClick.AddListener(() => RebindingAction(GameInputManager.Instance.PlayerMoveAction, _downBindingText, GameInputManager.DownBindingIndex));
        _leftButton.onClick.AddListener(() => RebindingAction(GameInputManager.Instance.PlayerMoveAction, _leftBindingText, GameInputManager.LeftBindingIndex));
        _rightButton.onClick.AddListener(() => RebindingAction(GameInputManager.Instance.PlayerMoveAction, _rightBindingText, GameInputManager.RightBindingIndex));
        _jumpButton.onClick.AddListener(() => RebindingAction(GameInputManager.Instance.PlayerJumpAction, _jumpBindingText, 0));

        GameInputManager.Instance.OnRebindStarted += GameInputManager_RebindingStart;
        GameInputManager.Instance.OnRebindCompleted += GameInputManager_RebindingCompleted;
    }
    private void OnDisable()
    {
        GameInputManager.Instance.OnRebindStarted -= GameInputManager_RebindingStart;
        GameInputManager.Instance.OnRebindCompleted -= GameInputManager_RebindingCompleted;
    }


    private void Start()
    {
       
        UpdateUI();
    }
    private void GameInputManager_RebindingStart()
    {
        _rebindingOverlayImage.gameObject.SetActive(true);
    }
    private void GameInputManager_RebindingCompleted()
    {
        _rebindingOverlayImage.gameObject.SetActive(false);
        UpdateUI();
    }
    private void RebindingAction(InputAction action, TextMeshProUGUI bindingText, int bindingIndex=0 )
    {
        GameInputManager.Instance.StartCompositeRebinding(action, bindingIndex);

        bindingText.text= "Any";

    }

    private void UpdateUI()
    {
        _upBindingText.text= GameInputManager.Instance.GetBindingDisplayString(GameInputManager.Instance.PlayerMoveAction, GameInputManager.UpBindingIndex);
    
        _downBindingText.text = GameInputManager.Instance.GetBindingDisplayString(GameInputManager.Instance.PlayerMoveAction, GameInputManager.DownBindingIndex);
    
        _leftBindingText.text = GameInputManager.Instance.GetBindingDisplayString(GameInputManager.Instance.PlayerMoveAction, GameInputManager.LeftBindingIndex);
        
        _rightBindingText.text = GameInputManager.Instance.GetBindingDisplayString(GameInputManager.Instance.PlayerMoveAction, GameInputManager.RightBindingIndex);
        _jumpBindingText.text = GameInputManager.Instance.GetBindingDisplayString(GameInputManager.Instance.PlayerJumpAction, 0);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
