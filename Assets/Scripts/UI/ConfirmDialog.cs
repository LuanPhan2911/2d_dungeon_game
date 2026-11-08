using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmDialog : Panel
{
   
    public static ConfirmDialog Instance { get; private set; }


    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;

    public Action OnConfirm;
    public Action OnCancel;

    private void Awake()
    {
      
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _confirmButton.onClick.AddListener(ConfirmPressed);
        _cancelButton.onClick.AddListener(CancelPressed);
    }
    private void Start()
    {
        Hide();
    }

    public void Show(string message, Action onConfirm, Action onCancel= null)
    {
        _messageText.text = message;
        OnConfirm = onConfirm;
        OnCancel = onCancel;

        base.Show();
    }

    private void ConfirmPressed()
    {
        Hide();
        OnConfirm?.Invoke();
    }
    private void CancelPressed()
    {
        Hide();
        OnCancel?.Invoke();
    }
}
