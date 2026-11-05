using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemRequireLevelUp : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemAmountText;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderAmountText;
    private int _amount=0;

    public Currency Currency => InventoryManager.Instance.Currency;
    private void Start()
    {


        _slider.interactable = Currency.amount > 0;
        _slider.maxValue = Currency.amount;
        
        _slider.onValueChanged.AddListener((value) =>
        {
            _amount = (int)value;
            UpdateSliderUI();
        });

        UpdateItemSlotUI();
        UpdateSliderUI();
        InventoryManager.Instance.OnCurrencyChanged += Inventory_OnCurrencyChanged;
       
      
    }

    private void Inventory_OnCurrencyChanged()
    {
        _slider.interactable = Currency.amount > 0 ;
        UpdateItemSlotUI();
    }

    private void UpdateItemSlotUI()
    {
        _itemImage.sprite = Currency.sprite;
        _itemAmountText.text = $"{Currency.amount}";
    }
    private void UpdateSliderUI()
    {
        _sliderAmountText.text = $"{_amount}";
    }
}
