
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class BagSlotUI : MonoBehaviour
{

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private ConfirmUseItem _confirmUseItem;

    private InventoryItem _item;


    public InventoryItem Item => _item;
   
    public void UseItem()
    {
        if (_item == null || !_item.dataSO.canBeUsed) return;

        _confirmUseItem.SetItem(_item);
        _confirmUseItem.SetMaxAmount(_item.amount);
        _confirmUseItem.SetLastSelectedGameObject(gameObject);
        _confirmUseItem.Show();

    }

    public void SetInventoryItem(InventoryItem item)
    {
        _item = item;
    }

    public void SetImage(Sprite sprite)
    {
        _image.gameObject.SetActive(true);
        _image.sprite = sprite;
    }
    public void SetAmount(int amount)
    {
        _amountText.gameObject.SetActive(true);
        _amountText.text = $"{amount}";
    }
   
    public void MakeSlotEmpty()
    {
        _image.gameObject.SetActive(false);
        _amountText.gameObject.SetActive(false);
    }
}
