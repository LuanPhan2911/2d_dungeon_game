using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequireItemBagSlot : MonoBehaviour
{

    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private Color _enoughTextColor = Color.green;
    [SerializeField] private Color _lackingTextColor = Color.red;
    private InventoryItem _requiredItem;
    public bool IsEnough => InventoryManager.Instance.HasItem(_requiredItem.dataSO ,_requiredItem.amount);




    public void SetItem(InventoryItem item)
    {
        _requiredItem = item;
        _itemImage.sprite = item.dataSO.sprite;
    }
    public void SetAmountText()
    {
        Color textColor = IsEnough ? _enoughTextColor : _lackingTextColor;
        _amountText.color = textColor;
        _amountText.text = $"{InventoryManager.Instance.GetItemQuantity(_requiredItem.dataSO)}/{_requiredItem.amount}";
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

}
