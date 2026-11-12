using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyButtonUI : MonoBehaviour
{


    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _keyText;
    [SerializeField] private Image _mouseImage;

    [Header("Mouse")]
    [SerializeField] private Sprite _leftMouseSprite;
    [SerializeField] private Sprite _rightMouseSprite;
    [SerializeField] private Sprite _middleMouseSprite;
    [SerializeField] private Sprite _backwardMouseSprite;
    [SerializeField] private Sprite _forwardMouseSprite;

    private Sprite GetMouseSprite(string mouseBinding)
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



    private void ShowKeyboardText()
    {
        _keyText.gameObject.SetActive(true);
        _mouseImage.gameObject.SetActive(false);
        _backgroundImage.enabled=true;
    }
    private void ShowMouseImage()
    {
        _mouseImage.gameObject.SetActive(true);
        _keyText.gameObject.SetActive(false);
        _backgroundImage.enabled = false;
    }
    private void SetKeyboardText(string text)
    {
       
        _keyText.text = text;
        ShowKeyboardText();
    }
    private void SetMouseImage(string text)
    {
        Sprite sprite = GetMouseSprite(text);
        if(sprite!= null)
        {
            _mouseImage.sprite = sprite;
            ShowMouseImage();
        }
       
    }
    public void SetDisplayString(string displayString)
    {
        if (displayString == GameInputManager.NONE_STRING) return;

        string[] splitedString = displayString.Split(' ');
        string text = splitedString[0];
        string device = splitedString[1];
        if (device == $"[{GameInputManager.KEYBOARD_BINDING}]")
        {
            SetKeyboardText(text);
        }
        else if (device == $"[{GameInputManager.MOUSE_BINDING}]")
        {
           SetMouseImage(text);
        }


    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
