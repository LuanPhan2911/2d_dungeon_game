
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class GameInputManager : MonoBehaviour
{
   public static GameInputManager Instance;


    private InputSystem_Actions _inputActions;


    public InputSystem_Actions.PlayerActions PlayerActions => _inputActions.Player;
    public InputSystem_Actions.UIActions UIActions => _inputActions.UI;

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

        PlayerActions.Enable();

        UIActions.Enable();
        DisableControlUI();



    }


    public void EnableControlUI()
    {
        UIActions.Cancel.Enable();
        UIActions.Prev.Enable();
        UIActions.Next.Enable();
    }

    public void DisableControlUI()
    {
        UIActions.Cancel.Disable();
        UIActions.Prev.Disable();
        UIActions.Next.Disable();
    }
   
    

    private void Start()
    {
       

    }
    private void OnDestroy()
    {
        _inputActions.Dispose();
    }




    public float GetHorizontalInput()
    {
        return PlayerActions.Move.ReadValue<Vector2>().x;
    }
    public float GetVerticalInput()
    {
        return PlayerActions.Move.ReadValue<Vector2>().y;
    }

    public bool IsUpPressed()
    {
        return PlayerActions.Move.ReadValue<Vector2>().y > 0;
    }
    public bool IsDownPressed()
    {
        return PlayerActions.Move.ReadValue<Vector2>().y < 0;
    }
    public bool IsLeftPressed()
    {
        return PlayerActions.Move.ReadValue<Vector2>().x < 0;
    }
    public bool IsRightPressed()
    {
        return PlayerActions.Move.ReadValue<Vector2>().x > 0;
    }

   


   
    public void Rebinding(InputAction action, int bindingIndex=0)
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

    public string GetDisplayString(InputAction action, int bindingIndex=0)
    {
        if (bindingIndex < 0 || bindingIndex >= action.bindings.Count)
        {
            Debug.LogError($"Invalid binding index: {bindingIndex} for action: {action.name}");
            return string.Empty;
        }
        return action.GetBindingDisplayString(bindingIndex);
    }
}
