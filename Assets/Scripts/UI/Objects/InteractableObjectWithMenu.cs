using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableObjectWithMenu : MonoBehaviour
{

    InputSystem_Actions.PlayerActions PlayerActions => GameInputManager.Instance.PlayerActions;

    [Header("UI References")]
    [SerializeField] private GameObject _interactionPanel;   // Kéo Panel tổng vào đây
    [SerializeField] private Button _firstInteractButton;        // Kéo nút "Nhấn F" vào đây
    [SerializeField] private GameObject _menuContainer;      // Kéo Container chứa danh sách nút vào đây

    [SerializeField] private Button _firstMenuButtonToSelect;// Nút đầu tiên trong danh sách để tự động highlight
    private bool _isPlayerNearby = false;
    private bool _isInteracted = false;

    private void Start()
    {
        // Ẩn UI khi bắt đầu game
        if (_interactionPanel != null) _interactionPanel.SetActive(false);

        GameInputManager.Instance.UIActions.Cancel.performed += GameInput_Cancel;
    }

    private void GameInput_Cancel(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        CloseMenu();
    }

    private void Update()
    {
       
        if (_isPlayerNearby && PlayerActions.Interact.WasPressedThisFrame())
        {
            if (!_isInteracted)
            {
                OpenMenu();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerHealth playerHealth))
        {
            _isPlayerNearby = true;
            _interactionPanel.SetActive(true);

            if (!_isInteracted)
            {
                _firstInteractButton.gameObject.SetActive(true);
                _menuContainer.SetActive(false);
                EventSystem.current.SetSelectedGameObject(_firstInteractButton.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth playerHealth))
        {
            _isPlayerNearby = false;
            _isInteracted = false;
            _interactionPanel.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void OpenMenu()
    {
        _firstInteractButton.gameObject.SetActive(false);
        _menuContainer.SetActive(true);
        _isInteracted = true;

        GameInputManager.Instance.PlayerActions.Disable();
        GameInputManager.Instance.UIActions.Cancel.Enable();

        _firstMenuButtonToSelect.Select();
    }
    public void CloseMenu()
    {
        _firstInteractButton.gameObject.SetActive(true);
        _menuContainer.SetActive(false);
        _isInteracted = false;

        GameInputManager.Instance.PlayerActions.Enable();
        GameInputManager.Instance.UIActions.Cancel.Disable();

        EventSystem.current.SetSelectedGameObject(_firstInteractButton.gameObject);
    }
}
