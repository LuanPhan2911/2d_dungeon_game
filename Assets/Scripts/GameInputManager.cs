using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class GameInputManager : MonoBehaviour
{
   public static GameInputManager Instance;


    private InputSystem_Actions _inputActions;
    public InputAction PlayerMoveAction;
    public InputAction PlayerJumpAction;

    public InputAction PlayerCrochAction;

    public InputAction PlayerRunAction;

    public InputAction UIPauseAction;

    public enum PlayerBindingAction
    {
      LeftBinding, RightBinding, UpBinding, DownBinding,
      JumpedBinding,
    }

    public  const int UpBindingIndex = 1;
    public const int DownBindingIndex = 2;
    public const int LeftBindingIndex = 3;
    public const int RightBindingIndex = 4;

    private const string PLAYER_PREFS_BINDINGS = "PlayerPrefsBinding";





    public event System.Action OnRebindStarted;
   public event System.Action OnRebindCompleted;


    private void Awake()
    {
        if (Instance != null && Instance!= this)
        {
            Destroy(gameObject);
            return;

        }
        Instance = this;
        _inputActions = new InputSystem_Actions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            _inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }



        PlayerMoveAction = _inputActions.Player.Move;
        PlayerJumpAction = _inputActions.Player.Jump;
        PlayerCrochAction = _inputActions.Player.CrochWalk;
        PlayerRunAction= _inputActions.Player.Run;
        // UI
        UIPauseAction = _inputActions.UI.Pause;


        PlayerMoveAction.Enable();
        PlayerJumpAction.Enable();
        PlayerCrochAction.Enable();
        PlayerRunAction.Enable();
        UIPauseAction.Enable();

    }

    private void Start()
    {
        UIPauseAction.performed += PauseAction_Performed;

    }
    private void OnDestroy()
    {
        UIPauseAction.performed -= PauseAction_Performed;
        _inputActions.Dispose();
    }




    public float GetHorizontalInput()
    {
        return PlayerMoveAction.ReadValue<Vector2>().x;
    }
    public float GetVerticalInput()
    {
        return PlayerMoveAction.ReadValue<Vector2>().y;
    }

    public bool IsUpPressed()
    {
        return PlayerMoveAction.ReadValue<Vector2>().y > 0;
    }
    public bool IsDownPressed()
    {
        return PlayerMoveAction.ReadValue<Vector2>().y < 0;
    }
    public bool IsLeftPressed()
    {
        return PlayerMoveAction.ReadValue<Vector2>().x < 0;
    }
    public bool IsRightPressed()
    {
        return PlayerMoveAction.ReadValue<Vector2>().x > 0;
    }

    
    private void PauseAction_Performed(InputAction.CallbackContext obj)
    {

        if (GameManager.Instance.IsGamePaused)
        {
            GameManager.Instance.Unpause();
        }
        else
        {
            GameManager.Instance.Pause();
        }
    }

    public void StartRebinding(PlayerBindingAction bindingAction)
    {
        InputAction actionToRebind = null;
        int bindingIndex = -1;
        switch (bindingAction)
        {
            case PlayerBindingAction.LeftBinding:
                actionToRebind = PlayerMoveAction;
                bindingIndex = LeftBindingIndex;
                break;
            case PlayerBindingAction.RightBinding:
                actionToRebind = PlayerMoveAction;
                bindingIndex = RightBindingIndex;
                break;
            case PlayerBindingAction.UpBinding:
                actionToRebind = PlayerMoveAction;
                bindingIndex = UpBindingIndex;
                break;
            case PlayerBindingAction.DownBinding:
                actionToRebind = PlayerMoveAction;
                bindingIndex = DownBindingIndex;
                break;
            case PlayerBindingAction.JumpedBinding:
                actionToRebind = PlayerJumpAction;
                bindingIndex = 0; // Assuming Jump has only one binding
                break;
            default:
                Debug.LogError($"Unknown binding action: {bindingAction}");
                return;
        }
        Rebinding(actionToRebind, bindingIndex);
    }


   
    private void Rebinding(InputAction action, int bindingIndex)
    {
        // 1. Disable the action before modifying its bindings
        _inputActions.Disable();

        OnRebindStarted?.Invoke();

        // 2. Configure and start the interactive rebind
        action.PerformInteractiveRebinding()
            .WithTargetBinding(bindingIndex)
            // Optional: Prevent assigning the Escape key (commonly used to cancel)
            .WithCancelingThrough("<Keyboard>/escape")
            // Callback for when the user successfully presses a new key
            .OnComplete(FinishRebinding)
            // Callback if the user cancels the rebind operation
            .OnCancel(CancelRebinding)
           
            .Start();

        
    }

    private void FinishRebinding(RebindingOperation rebindingOperation)
    {
        // Clean up memory allocated by the operation
        rebindingOperation.Dispose();

        // Re-enable the action so it works with the new key
        _inputActions.Enable();
        OnRebindCompleted?.Invoke();


        string bindingJson = _inputActions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, bindingJson);
        PlayerPrefs.Save();

        Debug.Log($"New binding applied");
    }

    private void CancelRebinding(RebindingOperation rebindingOperation)
    {
        rebindingOperation.Dispose();

        _inputActions.Enable();
        OnRebindCompleted?.Invoke();
        Debug.Log("Rebinding canceled.");
    }

    private string GetBindingDisplayString(InputAction action, int bindingIndex)
    {
        if (bindingIndex < 0 || bindingIndex >= action.bindings.Count)
        {
            Debug.LogError($"Invalid binding index: {bindingIndex} for action: {action.name}");
            return string.Empty;
        }
        return action.GetBindingDisplayString(bindingIndex);
    }


    public string GetBindingKey(PlayerBindingAction action)
    {
        switch (action)
        {
            case PlayerBindingAction.LeftBinding:
                return GetBindingDisplayString(PlayerMoveAction, LeftBindingIndex);
            case PlayerBindingAction.RightBinding:
                return GetBindingDisplayString(PlayerMoveAction, RightBindingIndex);
            case PlayerBindingAction.UpBinding:
                return GetBindingDisplayString(PlayerMoveAction, UpBindingIndex);
            case PlayerBindingAction.DownBinding:
                return GetBindingDisplayString(PlayerMoveAction, DownBindingIndex);
            case PlayerBindingAction.JumpedBinding:
                return GetBindingDisplayString(PlayerJumpAction, 0); // Assuming Jump has only one binding
            default:
                Debug.LogError($"Unknown binding action: {action}");
                return string.Empty;
        }
    }

    public bool IsCrochPresed()
    {
        return PlayerCrochAction.IsPressed();
    }
    public bool IsCrochWasReleasedThisFrame()
    {
        return PlayerCrochAction.WasReleasedThisFrame();
    }
    public bool IsCrochWasPressedThisFrame()
    {
        return PlayerCrochAction.WasPressedThisFrame();
    }

   

}
