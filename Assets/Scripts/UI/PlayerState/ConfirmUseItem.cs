using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConfirmUseItem : MonoBehaviour
{
    [SerializeField] private PlayerStateUI _playerState;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private Slider _slider;



    private InventoryItem _item;
    private int _amount = 1;
    private GameObject _lastSelectedGameObject;


    private void OnEnable()
    {
        _amount = 1;
        UpdateUI();

        _slider.onValueChanged.AddListener((value) =>
        {
            _amount = (int)value;
            UpdateUI();
        });
    }
    private void OnDisable()
    {
        _amount = 1;
        UpdateUI();

        _slider.onValueChanged.RemoveAllListeners();
    }
    private void UpdateUI()
    {
        _slider.value = _amount;
        _amountText.text = $"{_amount}";
    }
    public void SetItem(InventoryItem item)
    {
        _item = item;
    }
    public void SetLastSelectedGameObject(GameObject lastSelectedGameObject)
    {
        _lastSelectedGameObject = lastSelectedGameObject;
    }
    public void SetMaxAmount(int amount)
    {
        _slider.maxValue = amount;
    }
    public void Confirm()
    {
        Debug.Log($"Use item: {_item} with amount: {_amount}");
        Hide();
    }
    public void Cancel()
    {
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);

        _playerState.IsSubMenuOpened = true;
        _playerState.SubMenuGameObject = gameObject;
        _slider.Select();
    }
    public void Hide()
    {
        gameObject.SetActive(false);

        _playerState.IsSubMenuOpened = false;
        _playerState.SubMenuGameObject = null;
        if (EventSystem.current!= null)
        {
            EventSystem.current.SetSelectedGameObject(_lastSelectedGameObject);
        }
    }
}
