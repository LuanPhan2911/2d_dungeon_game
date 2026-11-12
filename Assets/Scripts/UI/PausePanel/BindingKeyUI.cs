
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class BindingKeyUI : MonoBehaviour
{


    [SerializeField] private Button _button;

    [SerializeField] private KeyButtonUI _keyButtonUI;
   
    private InputAction _action;
    private int _index;
    private void Awake()
    {
        
        _button.onClick.AddListener(
            () => GameInputManager.Instance.Rebind(_action, _index));
       
    }

    public void SetInputAction(InputAction action, int index)
    {
      _action= action;
      _index = index;
    }

    public void SetKeyButtonUI(string displayString)
    {
        _keyButtonUI.SetDisplayString(displayString);
    }


}
