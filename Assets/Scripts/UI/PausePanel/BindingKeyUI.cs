
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class BindingKeyUI : MonoBehaviour
{


    [SerializeField] private Button _button;

    [SerializeField] private TextMeshProUGUI _bindingText;

    [SerializeField] private Image _mouseImage;

    private InputAction _action;
    private int _index;
    private void Awake()
    {
        
        _button.onClick.AddListener(
            () => GameInputManager.Instance.Rebind(_action,
            GameInputManager.KEYBOARD_SCHEME, _index));
       
    }

    public void SetInputAction(InputAction action, int index)
    {
      _action= action;
      _index = index;
    }
   
    public void SetKeyText(string text)
    {
        _bindingText.text = text;
    }
    public void SetMouseSprite(Sprite sprite)
    {
        _mouseImage.sprite = sprite;
    }  
    public void ShowMouseImage()
    {
        _mouseImage.gameObject.SetActive(true);
        _bindingText.gameObject.SetActive(false);
    }
    public void HideMouseImage()
    {
        _mouseImage.gameObject.SetActive(false);
        _bindingText.gameObject.SetActive(true);
    }
    
    
}
