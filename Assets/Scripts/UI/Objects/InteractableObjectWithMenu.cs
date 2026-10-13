using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableObjectWithMenu : MonoBehaviour
{

    InputSystem_Actions.PlayerActions PlayerActions => GameInputManager.Instance.PlayerActions;

    [Header("UI References")]
    [SerializeField] private GameObject _interactionPanel;   // Kéo Panel tổng vào đây
    [SerializeField] private Button _firstTimeButton;        // Kéo nút "Nhấn F" vào đây
    [SerializeField] private GameObject _menuContainer;      // Kéo Container chứa danh sách nút vào đây
    [SerializeField] private Button _firstMenuButtonToSelect;// Nút đầu tiên trong danh sách để tự động highlight

    private bool _isPlayerNearby = false;
    private bool _isFirstTime = true;       // Biến kiểm tra xem có phải lần đầu không
    private bool _isMenuOpen = false;

    private void Start()
    {
        // Ẩn UI khi bắt đầu game
        if (_interactionPanel != null) _interactionPanel.SetActive(false);
    }

    private void Update()
    {
        // Nếu người chơi đang ở gần và nhấn phím F
        if (_isPlayerNearby && PlayerActions.Interact.WasPressedThisFrame())
        {
            // Trường hợp 1: Lần đầu tiên tương tác
            if (_isFirstTime)
            {
                TriggerFirstTimeAction();
            }
            // Trường hợp 2: Các lần sau, khi menu chức năng đã mở, phím F sẽ kích hoạt nút đang chọn
            else if (_isMenuOpen)
            {
                TriggerSelectedMenuAction();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerHealth playerHealth))
        {
            _isPlayerNearby = true;
            _interactionPanel.SetActive(true);

            if (_isFirstTime)
            {
                _firstMenuButtonToSelect.gameObject.SetActive(true);
                _menuContainer.SetActive(false);

                EventSystem.current.SetSelectedGameObject(_firstMenuButtonToSelect.gameObject);
            }
            else
            {
                OpenMenu();
            }


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth playerHealth))
        {
            _isPlayerNearby = false;
            _isMenuOpen = false;
            _interactionPanel.SetActive(false); 
            if(EventSystem.current!= null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    private void TriggerFirstTimeAction()
    {
        Debug.Log("Đã tương tác lần đầu tiên!");

        // Đánh dấu đã qua lần đầu
        _isFirstTime = false;

    }

    private void OpenMenu()
    {
        _firstTimeButton.gameObject.SetActive(false);
        _menuContainer.SetActive(true);
        _isMenuOpen = true;


        if (_firstMenuButtonToSelect!= null)
        {
            EventSystem.current.SetSelectedGameObject(null); // Reset select cũ
            _firstMenuButtonToSelect.Select();
        }
    }

    private void TriggerSelectedMenuAction()
    {
        GameObject currentSelected= EventSystem.current.currentSelectedGameObject;

        if(currentSelected!= null && currentSelected.transform.IsChildOf(_menuContainer.transform))
        {
            Button button = currentSelected.GetComponent<Button>();
            if(button!= null && button.interactable)
            {
                button.onClick.Invoke();
            }
        }
    }
}
