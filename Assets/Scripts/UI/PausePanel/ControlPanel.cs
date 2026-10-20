
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
    [SerializeField] private BindingKeyUI _thirdElementBinding;


    [Header("UI")]

  
    [SerializeField] private BindingKeyUI _stateBinding;
    [SerializeField] private BindingKeyUI _weaponBinding;
 
    [SerializeField] private BindingKeyUI _bagBinding;
    [SerializeField] private BindingKeyUI _skillBinding;
    [SerializeField] private BindingKeyUI _mapBinding;

    [SerializeField] private BindingKeyUI _submitBinding;
    [SerializeField] private BindingKeyUI _cancelBinding;



    [SerializeField] private GameObject _overlayGameObject;

    const int KEYBOARD_BINDING_INDEX = 0;

    public GameInputManager GameInput => GameInputManager.Instance;
    public InputSystem_Actions.PlayerActions PlayerActions => GameInput.PlayerActions;
    public InputSystem_Actions.UIActions UIActions => GameInput.UIActions;

    protected override void Start()
    {
        GameInput.OnRebindStarted += GameInputManager_RebindingStart;
        GameInput.OnRebindCompleted += GameInputManager_RebindingCompleted;

        SetInputActionForBindingKey();
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
   
    private void SetInputActionForBindingKey()
    {
        // Player
        _upBinding.SetInputAction(PlayerActions.Move, GameInputManager.UpBindingIndex);
        _downBinding.SetInputAction(PlayerActions.Move, GameInputManager.DownBindingIndex);
        _leftBinding.SetInputAction(PlayerActions.Move, GameInputManager.LeftBindingIndex);
        _rightBinding.SetInputAction(PlayerActions.Move, GameInputManager.RightBindingIndex);

        _jumpBinding.SetInputAction(PlayerActions.Jump, KEYBOARD_BINDING_INDEX);
        _attackBinding.SetInputAction(PlayerActions.Attack, KEYBOARD_BINDING_INDEX);
        _parryBinding.SetInputAction(PlayerActions.Parry, KEYBOARD_BINDING_INDEX);
        _sprintBinding.SetInputAction(PlayerActions.Sprint, KEYBOARD_BINDING_INDEX);
        _burstBinding.SetInputAction(PlayerActions.Burst, KEYBOARD_BINDING_INDEX);
        _healBinding.SetInputAction(PlayerActions.Heal, KEYBOARD_BINDING_INDEX);
        _interactBinding.SetInputAction(PlayerActions.Interact, KEYBOARD_BINDING_INDEX);

        _firstElementBinding.SetInputAction(PlayerActions.First, KEYBOARD_BINDING_INDEX);
        _secondElementBinding.SetInputAction(PlayerActions.Second, KEYBOARD_BINDING_INDEX);
        _thirdElementBinding.SetInputAction(PlayerActions.Third, KEYBOARD_BINDING_INDEX);

        // UI
        _submitBinding.SetInputAction(UIActions.Submit, KEYBOARD_BINDING_INDEX);
        _cancelBinding.SetInputAction(UIActions.Cancel, KEYBOARD_BINDING_INDEX);
       
        _stateBinding.SetInputAction(UIActions.State, KEYBOARD_BINDING_INDEX);
        _weaponBinding.SetInputAction(UIActions.Weapon, KEYBOARD_BINDING_INDEX);
        _bagBinding.SetInputAction(UIActions.Bag, KEYBOARD_BINDING_INDEX);
        _skillBinding.SetInputAction(UIActions.Skill, KEYBOARD_BINDING_INDEX);
        _mapBinding.SetInputAction(UIActions.Map, KEYBOARD_BINDING_INDEX);
        




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
        _thirdElementBinding.SetBindingText(GameInput.GetDisplayString(PlayerActions.Third));

        // UI

        _submitBinding.SetBindingText(GameInput.GetDisplayString(UIActions.Submit));
        _cancelBinding.SetBindingText(GameInput.GetDisplayString(UIActions.Cancel));
        _stateBinding.SetBindingText(GameInput.GetDisplayString(UIActions.State));
        _weaponBinding.SetBindingText(GameInput.GetDisplayString(UIActions.Weapon));
        _bagBinding.SetBindingText(GameInput.GetDisplayString(UIActions.Bag));
        _skillBinding.SetBindingText(GameInput.GetDisplayString(UIActions.Skill));
        _mapBinding.SetBindingText(GameInput.GetDisplayString(UIActions.Map));





    }

   
}
