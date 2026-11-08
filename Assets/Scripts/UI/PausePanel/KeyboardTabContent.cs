
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyboardTabContent : TabContent
{

    [SerializeField] private GameObject _overlayGameObject;
   

    [SerializeField] private Button _resetPlayerInputButton;
    [SerializeField] private Button _resetUIInputButton;

    [SerializeField] private string _confirmResetPlayerInputText;
    [SerializeField] private string _confirmResetUIInputText;


    [Header("Mouse")]
    [SerializeField] private Sprite _leftMouseSprite;
    [SerializeField] private Sprite _rightMouseSprite;
    [SerializeField] private Sprite _middleMouseSprite;
    [SerializeField] private Sprite _backwardMouseSprite;
    [SerializeField] private Sprite _forwardMouseSprite;


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
    [SerializeField] private BindingKeyUI _mapBinding;

    [SerializeField] private BindingKeyUI _submitBinding;
    [SerializeField] private BindingKeyUI _cancelBinding;



   

  

    public GameInputManager GameInput => GameInputManager.Instance;
    public InputSystem_Actions.PlayerActions PlayerActions => GameInput.PlayerActions;
    public InputSystem_Actions.UIActions UIActions => GameInput.UIActions;



    public Sprite GetMouseSprite(string mouseBinding)
    {
        switch (mouseBinding)
        {
            case GameInputManager.LEFT_MOUSE_STRING:
                return _leftMouseSprite;
            case GameInputManager.RIGHT_MOUSE_STRING:
                return _rightMouseSprite;
            case GameInputManager.MIDDLE_MOUSE_STRING:
                return _middleMouseSprite;
            case GameInputManager.BACKWARD_MOUSE_STRING:
                return _backwardMouseSprite;
            case GameInputManager.FORWARD_MOUSE_STRING:
                return _forwardMouseSprite;
            default:
                return null;
        }
    }

    private void Awake()
    {
        _resetPlayerInputButton.onClick.AddListener(() =>
        {
            ConfirmDialog.Instance.Show(_confirmResetPlayerInputText, () =>
            {
                GameInput.ResetPlayerInput(GameInputManager.KEYBOARD_SCHEME);
                UpdateUI();
                _resetPlayerInputButton.Select();
            }, () =>
            {
                _resetPlayerInputButton.Select();
            });
        });

        _resetUIInputButton.onClick.AddListener(() =>
        {
            ConfirmDialog.Instance.Show(_confirmResetUIInputText, () =>
            {
                GameInput.ResetUIInput(GameInputManager.KEYBOARD_SCHEME);
                UpdateUI();
                _resetUIInputButton.Select();
            }, () =>
            {
                _resetUIInputButton.Select();
            });
           
        }); 
    }

    private void Start()
    {
        GameInput.OnRebindStarted += GameInputManager_RebindingStart;
        GameInput.OnRebindCompleted += GameInputManager_RebindingCompleted;

        SetInputActionForBindingKey();
        UpdateUI();
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
        SetInputAction(_upBinding, PlayerActions.Move, GameInputManager.UP_BINDING);
        SetInputAction(_downBinding, PlayerActions.Move, GameInputManager.DOWN_BINDING);
        SetInputAction(_leftBinding, PlayerActions.Move, GameInputManager.LEFT_BINDING);
        SetInputAction(_rightBinding, PlayerActions.Move, GameInputManager.RIGHT_BINDING);

        SetInputAction(_jumpBinding, PlayerActions.Jump);
        SetInputAction(_attackBinding, PlayerActions.Attack);
        SetInputAction(_parryBinding, PlayerActions.Parry);
        SetInputAction(_sprintBinding, PlayerActions.Sprint);
        SetInputAction(_burstBinding, PlayerActions.Burst);
        SetInputAction(_healBinding, PlayerActions.Heal);
        SetInputAction(_interactBinding, PlayerActions.Interact);

        SetInputAction(_firstElementBinding, PlayerActions.First);
        SetInputAction(_secondElementBinding, PlayerActions.Second);
        SetInputAction(_thirdElementBinding, PlayerActions.Third             );

        // UI
        SetInputAction(_submitBinding, UIActions.Submit);
        SetInputAction(_cancelBinding, UIActions.Cancel);
       
        SetInputAction(_stateBinding, UIActions.Attribute);
        SetInputAction(_weaponBinding, UIActions.Charm);
        SetInputAction(_bagBinding, UIActions.Bag);
        SetInputAction(_mapBinding, UIActions.Map);
        




    }

    private void UpdateUI()
    {
        // Player
        SetText(_upBinding, PlayerActions.Move, GameInputManager.UP_BINDING);
        SetText(_downBinding, PlayerActions.Move, GameInputManager.DOWN_BINDING);
        SetText(_leftBinding, PlayerActions.Move, GameInputManager.LEFT_BINDING);
        SetText(_rightBinding, PlayerActions.Move, GameInputManager.RIGHT_BINDING);

        SetText(_jumpBinding, PlayerActions.Jump);
        SetText(_attackBinding, PlayerActions.Attack);
        SetText(_parryBinding, PlayerActions.Parry);
        SetText(_sprintBinding, PlayerActions.Sprint);
        SetText(_burstBinding, PlayerActions.Burst);
        SetText(_healBinding, PlayerActions.Heal);
        SetText(_interactBinding, PlayerActions.Interact);

        SetText(_firstElementBinding, PlayerActions.First);
        SetText(_secondElementBinding, PlayerActions.Second);
        SetText(_thirdElementBinding, PlayerActions.Third);

        // UI

        SetText(_submitBinding, UIActions.Submit);
        SetText(_cancelBinding, UIActions.Cancel);
        SetText(_stateBinding, UIActions.Attribute);
        SetText(_weaponBinding, UIActions.Charm);
        SetText(_bagBinding, UIActions.Bag);
        SetText(_mapBinding, UIActions.Map);





    }


    private void SetText(BindingKeyUI bindingKeyUI, InputAction action, string partName=null)
    {
        string displayString = GameInput.GetKeyboardDisplayString(action, partName);

       
        Sprite mouseSprite = GetMouseSprite(displayString);

        if (mouseSprite != null)
        {
            bindingKeyUI.SetMouseSprite(mouseSprite);
            bindingKeyUI.ShowMouseImage();

        }
        else
        {
            bindingKeyUI.SetKeyText(displayString);
            bindingKeyUI.HideMouseImage();
        }
    }
    private void SetInputAction(BindingKeyUI bindingKeyUI, InputAction action, string partName = null)
    {
        int bindingIndex = partName != null ? 
            GameInput.GetBindingIndexOfCompositeAction(action, GameInputManager.KEYBOARD_SCHEME, partName) : 
            GameInput.GetBindingIndexOfAction(action, GameInputManager.KEYBOARD_SCHEME);

      
        bindingKeyUI.SetInputAction(action, bindingIndex);
    }


}
