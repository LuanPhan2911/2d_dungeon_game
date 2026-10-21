using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BagPanel : MonoBehaviour
{
    [SerializeField] private BagSlotUI[] _bagSlotArray;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemDescriptionText;

    private GameObject _lastSelectedSlotGameObject;


    private void Start()
    {
        UpdateUI();

        InventoryManager.Instance.OnInventoryChanged += InventoryManager_OnInventoryChanged;
    }
    private void Update()
    {
        GameObject currentSelectedSlotGameObject = EventSystem.current != null ?
           EventSystem.current.currentSelectedGameObject : null;

        if (currentSelectedSlotGameObject != _lastSelectedSlotGameObject)
        {
            ShowItemInfo(currentSelectedSlotGameObject);
            _lastSelectedSlotGameObject = currentSelectedSlotGameObject;
        }
    }

    private void ShowItemInfo(GameObject currentSelectedGameObject)
    {

        _itemNameText.gameObject.SetActive(false);
        _itemDescriptionText.gameObject.SetActive(false);
        if (currentSelectedGameObject!= null &&
            currentSelectedGameObject.TryGetComponent(out BagSlotUI bagSlotUI))
        {
            
            if(bagSlotUI.Item!= null)
            {
                _itemNameText.gameObject.SetActive(true);
                _itemDescriptionText.gameObject.SetActive(true);

                _itemNameText.text = bagSlotUI.Item.dataSO.itemName;
                _itemDescriptionText.text = bagSlotUI.Item.dataSO.description;
            }
           
        }
    }

    private void InventoryManager_OnInventoryChanged()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        InventoryItem[] inventoryItemArray = InventoryManager.Instance.InventoryItemArray;
        for(int i=0; i< inventoryItemArray.Length; i++)
        {
            InventoryItem item = inventoryItemArray[i];

            _bagSlotArray[i].SetInventoryItem(item);
            _bagSlotArray[i].SetImage(item.dataSO.sprite);
            _bagSlotArray[i].SetAmount(item.amount);
        }
        for(int i= inventoryItemArray.Length; i< _bagSlotArray.Length; i++)
        {
            _bagSlotArray[i].MakeSlotEmpty();
        }
    }

   

    
}
