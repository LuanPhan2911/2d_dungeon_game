
using UnityEngine;

using UnityEngine.UI;

public class ControlPanel : Panel
{
    [Header("Player")]
    [SerializeField] private BindingKeyUI _upBinding;
    [SerializeField] private BindingKeyUI _downBinding;
    [SerializeField] private BindingKeyUI _leftBinding;
    [SerializeField] private BindingKeyUI _rightBinding;
    [SerializeField] private BindingKeyUI _jumpBinding;
    [SerializeField] private BindingKeyUI _attackBinding;
    [SerializeField] private BindingKeyUI _parryBinding;
    [SerializeField] private BindingKeyUI _sprintBinding;
    [SerializeField] private BindingKeyUI _burstBinding;
    [SerializeField] private BindingKeyUI _healBinding;
 
    [SerializeField] private BindingKeyUI _interactBinding;
    [SerializeField] private BindingKeyUI _firstElementBinding;
    [SerializeField] private BindingKeyUI _secondElementBinding;


    [Header("UI")]

    [SerializeField] private BindingKeyUI _submitBinding;
    [SerializeField] private BindingKeyUI _cancelBinding;
    [SerializeField] private BindingKeyUI _characterBinding;
    [SerializeField] private BindingKeyUI _bagBinding;
    [SerializeField] private BindingKeyUI _mapBinding;



    [SerializeField] private GameObject _overlayGameObject;

    public GameInputManager GameInput => GameInputManager.Instance;
    public InputSystem_Actions.PlayerActions PlayerActions => GameInput.PlayerActions;
    public InputSystem_Actions.UIActions UIActions => GameInput.UIActions;

    protected override void Start()
    {
        GameInput.OnRebindStarted += GameInputManager_RebindingStart;
        GameInput.OnRebindCompleted += GameInputManager_RebindingCompleted;
        
        
        UpdateUI();

        base.Start();
    }
    private void GameInputManager_RebindingStart()
    {
        _overlayGameObject.SetActive(true);
    }
    private void GameInputManager_RebindingCompleted()
    {
        _overlayGameObject.SetActive(false);
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
        // Player
        _upBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Move, GameInputManager.UpBindingIndex));
        _downBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Move, GameInputManager.DownBindingIndex));
        _leftBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Move, GameInputManager.LeftBindingIndex));
        _rightBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Move, GameInputManager.RightBindingIndex));

        _jumpBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Jump));
        _attackBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Attack));
        _parryBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Parry));
        _sprintBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Sprint));
        _burstBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Burst));
        _healBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Heal));
        _interactBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Interact));

        _firstElementBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.First));
        _secondElementBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Second));

        // UI

        _submitBinding.SetBindingText(GameInput.GetDisplayString(UIActions.Submit));
        _cancelBinding.SetBindingText(GameInput.GetDisplayString(UIActions.Cancel));
        _characterBinding.SetBindingText(GameInput.GetDisplayString(UIActions.Character));
        _bagBinding.SetBindingText(GameInput.GetDisplayString(UIActions.Bag));
        _mapBinding.SetBindingText(GameInput.GetDisplayString(UIActions.Map));





    }

   
}
