using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{


    [SerializeField] private Image _image;

    private RectTransform _rectTranform;
    private float _defaultWidth;
    public PlayerHealth PHealth => PlayerHealth.Instance;

    private void Awake()
    {
        _rectTranform = GetComponent<RectTransform>();
        _defaultWidth = _rectTranform.rect.width;
    }

    private void Start()
    {
        float newWidth = _defaultWidth * PHealth.BaseHealthRatio;
        _rectTranform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);


        PHealth.OnHealthChanged += PlayerHealth_OnHealthChanged;
    }

    private void PlayerHealth_OnHealthChanged()
    {
        SetImageFill(PHealth.HealthRatio);
    }

    private void SetImageFill(float amount)
    {
        _image.fillAmount = amount;
    }
}
