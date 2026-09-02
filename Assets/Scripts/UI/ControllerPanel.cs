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
        _upButton.onClick.AddListener(() => RebindingAction(GameInputManager.PlayerBindingAction.UpBinding, _upBindingText));
        _downButton.onClick.AddListener(() => RebindingAction(GameInputManager.PlayerBindingAction.DownBinding, _downBindingText));
        _leftButton.onClick.AddListener(() => RebindingAction(GameInputManager.PlayerBindingAction.LeftBinding, _leftBindingText));
        _rightButton.onClick.AddListener(() => RebindingAction(GameInputManager.PlayerBindingAction.RightBinding, _rightBindingText));
        _jumpButton.onClick.AddListener(() => RebindingAction(GameInputManager.PlayerBindingAction.JumpedBinding, _jumpBindingText));

        GameInputManager.Instance.OnRebindStarted += GameInputManager_RebindingStart;
        GameInputManager.Instance.OnRebindCompleted += GameInputManager_RebindingCompleted;
    }
    private void OnDisable()
    {
        GameInputManager.Instance.OnRebindStarted -= GameInputManager_RebindingStart;
        GameInputManager.Instance.OnRebindCompleted -= GameInputManager_RebindingCompleted;

        _upButton.onClick.RemoveAllListeners();
        _downButton.onClick.RemoveAllListeners();
        _leftButton.onClick.RemoveAllListeners();
        _rightButton.onClick.RemoveAllListeners();
        _jumpButton.onClick.RemoveAllListeners();
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
    private void RebindingAction(GameInputManager.PlayerBindingAction bindingAction, TextMeshProUGUI bindingText)
    {
        GameInputManager.Instance.StartRebinding(bindingAction );
        bindingText.text = "Any";
    }

    private void UpdateUI()
    {
        _upBindingText.text= GameInputManager.Instance.GetBindingKey(GameInputManager.PlayerBindingAction.UpBinding);
    
        _downBindingText.text = GameInputManager.Instance.GetBindingKey(GameInputManager.PlayerBindingAction.DownBinding);
    
        _leftBindingText.text = GameInputManager.Instance.GetBindingKey(GameInputManager.PlayerBindingAction.LeftBinding        );
        
        _rightBindingText.text = GameInputManager.Instance.GetBindingKey(GameInputManager.PlayerBindingAction.RightBinding);

        _jumpBindingText.text = GameInputManager.Instance.GetBindingKey(GameInputManager.PlayerBindingAction.JumpedBinding);
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
