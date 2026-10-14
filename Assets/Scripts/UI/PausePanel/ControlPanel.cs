
using UnityEngine;

using UnityEngine.UI;

public class ControlPanel : Panel
{
    [SerializeField] private BindingKeyUI _upBinding;
    [SerializeField] private BindingKeyUI _downBinding;
    [SerializeField] private BindingKeyUI _leftBinding;
    [SerializeField] private BindingKeyUI _rightBinding;



    [SerializeField] private Image _rebindingOverlayImage;

    public GameInputManager GameInput => GameInputManager.Instance;
    public InputSystem_Actions.PlayerActions PlayerActions => GameInput.PlayerActions;

    protected override void Start()
    {
        GameInput.OnRebindStarted += GameInputManager_RebindingStart;
        GameInput.OnRebindCompleted += GameInputManager_RebindingCompleted;
        UpdateUI();

        base.Start();
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
    public void UpRebinding()
    {
        GameInput.Rebinding(PlayerActions.Move, GameInputManager.UpBindingIndex );
       
    }
    public void DownRebinding()
    {
        GameInput.Rebinding(PlayerActions.Move, GameInputManager.DownBindingIndex );
       
    }
    public void LeftRebinding()
    {
        GameInput.Rebinding(PlayerActions.Move, GameInputManager.LeftBindingIndex );
       
    }
    public void RightRebinding()
    {
        GameInput.Rebinding(PlayerActions.Move, GameInputManager.RightBindingIndex );
       
    }

    private void UpdateUI()
    {
        _upBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Move, GameInputManager.UpBindingIndex));
        _downBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Move, GameInputManager.DownBindingIndex));
        _leftBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Move, GameInputManager.LeftBindingIndex));
        _rightBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Move, GameInputManager.RightBindingIndex));
    }

   
}
