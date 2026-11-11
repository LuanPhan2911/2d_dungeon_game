
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance;
    private InputSystem_Actions _inputActions;
    public InputSystem_Actions.PlayerActions PlayerActions => _inputActions.Player;
    public InputSystem_Actions.UIActions UIActions => _inputActions.UI;

    public const string UP_BINDING = "Up";
    public const string DOWN_BINDING = "Down";
    public const string LEFT_BINDING = "Left";
    public const string RIGHT_BINDING = "Right";


    public const string LEFT_MOUSE_STRING = "Left Button";
    public const string RIGHT_MOUSE_STRING = "Right Button";
    public const string MIDDLE_MOUSE_STRING = "Middle Button";
    public const string BACKWARD_MOUSE_STRING = "Back";
    public const string FORWARD_MOUSE_STRING = "Forward";
    public const string NONE_STRING = "None";


    public const string KEYBOARD_SCHEME = "Keyboard&Mouse";
    private const string PLAYER_PREFS_BINDINGS = "PlayerPrefsBinding";


    public const int KEYBOARD_BINDING_INDEX = 0;



    public event System.Action OnRebindStarted;
    public event System.Action OnRebindCompleted;


    private int _currentBindingIndex;

    public int CurrentBindingIndex => _currentBindingIndex;



   

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
        _currentBindingIndex = KEYBOARD_BINDING_INDEX;

        PlayerActions.Enable();
        UIActions.Enable();

        UIActions.Prev.Disable();
        UIActions.Next.Disable();




    }


   


    public void DisablePlayerInput()
    {
        PlayerActions.Disable();
        UIActions.Prev.Enable();
        UIActions.Next.Enable();
    }

    public void EnablePlayerInput()
    {
        PlayerActions.Enable();
        UIActions.Prev.Disable();
        UIActions.Next.Disable();
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

    
   

    public void Rebind(InputAction action, int bindingIndex)
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


        SaveBinding();

        Debug.Log($"New binding applied");
    }
    private void SaveBinding()
    {
        string bindingJson = _inputActions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, bindingJson);
        PlayerPrefs.Save();
    }

    private void CancelRebinding(RebindingOperation rebindingOperation)
    {
        rebindingOperation.Dispose();

        _inputActions.Enable();
        OnRebindCompleted?.Invoke();
        Debug.Log("Rebinding canceled.");
    }
    public int GetBindingIndexOfCompositeAction(InputAction action, string controlType, string partName)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            InputBinding binding = action.bindings[i];
            if (binding.isPartOfComposite && binding.name.ToLower() == partName.ToLower()
                && binding.groups.Contains(controlType))
            {
                return i;
            }
        }
        Debug.LogError($"Binding part '{partName}' not found in action '{action.name}'.");

        return -1; // Return -1 if not found
    }
    public int GetBindingIndexOfAction(InputAction action, string controlType)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            InputBinding binding = action.bindings[i];
            if (!binding.isPartOfComposite && binding.groups.Contains(controlType))
            {
                return i;
            }
        }
        Debug.LogError($"Binding for control type '{controlType}' not found in action '{action.name}'.");
        return -1; // Return -1 if not found
    }




    private string GetDisplayString(InputAction action, string controlType, string partName=null)
    {
        int bindingIndex = partName != null ? 
            GetBindingIndexOfCompositeAction(action, controlType, partName) : 
            GetBindingIndexOfAction(action, controlType);

        if (bindingIndex== -1)
        {
            return NONE_STRING;
        }
        string displayString = action.GetBindingDisplayString(bindingIndex, InputBinding.DisplayStringOptions.DontUseShortDisplayNames);
       
        if(string.IsNullOrEmpty(displayString))
        {
            return NONE_STRING;
        }
        return displayString;
    }

    public string GetKeyboardDisplayString(InputAction action, string partName = null)
    {
        return GetDisplayString(action, KEYBOARD_SCHEME, partName);
    }

    private void RemoveBindingOverride(InputAction action, string controlType, string partName = null)
    {
        int bindingIndex = partName != null ?
            GetBindingIndexOfCompositeAction(action, controlType, partName) :
            GetBindingIndexOfAction(action, controlType);
        if (bindingIndex != -1)
        {
            action.RemoveBindingOverride(bindingIndex);
        }
    }

    public void ResetPlayerInput(string controlType)
    {
        RemoveBindingOverride(PlayerActions.Move, controlType, UP_BINDING);
        RemoveBindingOverride(PlayerActions.Move, controlType, DOWN_BINDING);
        RemoveBindingOverride(PlayerActions.Move, controlType, LEFT_BINDING);
        RemoveBindingOverride(PlayerActions.Move, controlType, RIGHT_BINDING);

        RemoveBindingOverride(PlayerActions.Jump, controlType);
        RemoveBindingOverride(PlayerActions.Attack, controlType);
        RemoveBindingOverride(PlayerActions.Parry, controlType);
        RemoveBindingOverride(PlayerActions.Sprint, controlType);

        RemoveBindingOverride(PlayerActions.Burst, controlType);
        RemoveBindingOverride(PlayerActions.Heal, controlType);
        RemoveBindingOverride(PlayerActions.Interact, controlType);

        RemoveBindingOverride(PlayerActions.First, controlType);
        RemoveBindingOverride(PlayerActions.Second, controlType);
        RemoveBindingOverride(PlayerActions.Third, controlType);



        SaveBinding();
    }

    public void ResetUIInput(string controlType)
    {
        RemoveBindingOverride(UIActions.Submit, controlType);
        RemoveBindingOverride(UIActions.Cancel, controlType);
        RemoveBindingOverride(UIActions.Attribute, controlType);
        RemoveBindingOverride(UIActions.Charm, controlType);
        RemoveBindingOverride(UIActions.Bag, controlType);
        RemoveBindingOverride(UIActions.Map, controlType);



        SaveBinding();
    }
}
