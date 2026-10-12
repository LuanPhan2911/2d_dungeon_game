using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BurstSkillUI : MonoBehaviour
{
    [SerializeField] private Image _energyImage;
    [SerializeField] private Image _cooldownImage;
    [SerializeField] private Image _mainImage;
    [SerializeField] private TextMeshProUGUI _cooldownText;


    public PlayerElement PElement => PlayerElement.Instance;

    private float _currentDuration;


    private void Start()
    {
        PElement.OnElementSwapped += ElementSwap;
    
      

        if (!PElement.HasActive)
        {
            Hide();
        }
    }
    private void Update()

    {
        if (!PElement.HasActive) return;



        _energyImage.fillAmount = PElement.BurstEnergyRatio;
        if (PElement.IsBurstCooldown)
        {
            _cooldownImage.gameObject.SetActive(true);
            _cooldownText.gameObject.SetActive(true);
            _cooldownImage.fillAmount = PElement.BurstCooldownRatio;

            if (_currentDuration != PElement.BurstCooldownDuration)
            {
                _cooldownText.text = $"{PElement.BurstCooldownDuration}";

            }
            _currentDuration = PElement.BurstCooldownDuration;
        }
        else
        {
            _cooldownImage.gameObject.SetActive(false);
            _cooldownText.gameObject.SetActive(false);
        }
    }

    private void ElementSwap()
    {
        _energyImage.color = PElement.ActiveElementSO.color;
        _mainImage.sprite = PElement.ActivePlayerElementSO.burstSkillSprite;
        Show();
    }


    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
