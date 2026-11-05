using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RangeSliderWithValue : MonoBehaviour
{
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _valueText;

    [SerializeField] private float _maxValue = 10;
    [SerializeField] private float _minValue = 0;
    [SerializeField] private float _stepSize = 1;

    public Action<float> OnValueChanged;




    public float Value => _slider.value;
    public float MaxValue => _maxValue;


    private void Awake()
    {

        _slider.maxValue = _maxValue;
        _slider.minValue = _minValue;
        _leftButton.onClick.AddListener(OnLeftButtonClicked);
        _rightButton.onClick.AddListener(OnRightButtonClicked);
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
    }
    public void SetValue(float value)
    {
        _slider.value = value;
    }

    private void OnLeftButtonClicked()
    {
        _slider.value -= _stepSize;
    }

    private void OnRightButtonClicked()
    {
        _slider.value += _stepSize;
    }
    private void OnSliderValueChanged(float value)
    {
        _valueText.text = value.ToString("0.##");
        OnValueChanged?.Invoke(value);
    }
}
