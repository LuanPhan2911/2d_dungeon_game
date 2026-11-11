using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractKeyUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _keyText;


    private void Start()
    {
        GameInputManager.Instance.OnRebindCompleted += GameInput_OnRebindCompleted;
        SetKeyText();

    }

    private void GameInput_OnRebindCompleted()
    {
        SetKeyText();
    }

    private void SetKeyText()
    {
        string text = GameInputManager.Instance.PlayerActions.Interact.GetBindingDisplayString(
            GameInputManager.Instance.CurrentBindingIndex);

        _keyText.text = text;
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
