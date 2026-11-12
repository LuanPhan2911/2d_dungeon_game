using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationDialog : Panel
{
   
    public static NotificationDialog Instance { get; private set; }


    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Button _closeButton;

    public Action OnClose;

    private void Awake()
    {
      
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _closeButton.onClick.AddListener(ClosePressed);
    }
    private void Start()
    {
        Hide();
    }

    public void Show(string message, Action onClose= null)
    {
        _messageText.text = message;
      
        OnClose = onClose;

        base.Show();
    }

    private void ClosePressed()
    {
        Hide();
        OnClose?.Invoke();
    }
}
