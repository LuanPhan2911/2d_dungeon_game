using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BindingKeyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bindingText;

    private InputAction _action;
    private int _bindingIndex = 0;

    public void SetInputAction(InputAction action, int bindingIndex)
    {
        _action = action;
        _bindingIndex = bindingIndex;
    }
    public void RebindInput()
    {
        GameInputManager.Instance.Rebinding(_action, _bindingIndex);
    }
   
    public void SetBindingText(string text)
    {
        _bindingText.text = text;
    }
}
