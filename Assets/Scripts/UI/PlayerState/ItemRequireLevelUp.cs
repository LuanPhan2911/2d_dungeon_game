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

 
    private void Start()
    {


       
        
        _slider.onValueChanged.AddListener((value) =>
        {
            _amount = (int)value;
            UpdateSliderUI();
        });

        UpdateSliderUI();
      
       
      
    }

   
   
    private void UpdateSliderUI()
    {
        _sliderAmountText.text = $"{_amount}";
    }
}
