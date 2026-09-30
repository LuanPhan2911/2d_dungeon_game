using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BurstSkillUI : MonoBehaviour
{
    [SerializeField] private Image _activeImage;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _cooldownText;


    private float _currentDuration;


    private void Start()
    {
        PlayerElement.Instance.OnElementSwapped += ElementSwap;
    
      

        if (!PlayerElement.Instance.HasActive)
        {
            Hide();
        }
    }
    private void Update()

    {
        if (!PlayerElement.Instance.HasActive) return;


        if (PlayerElement.Instance.IsBurstSkillCooldown)
        {
            _activeImage.gameObject.SetActive(true);
            _cooldownText.gameObject.SetActive(true);
            _activeImage.fillAmount = PlayerElement.Instance.BurstSkillRatio;
            if (_currentDuration != PlayerElement.Instance.BurstSkillDuration)
            {
                _cooldownText.text = $"{PlayerElement.Instance.BurstSkillDuration}";

            }
            _currentDuration = PlayerElement.Instance.BurstSkillDuration;
        }
        else
        {
            _activeImage.gameObject.SetActive(false);
            _cooldownText.gameObject.SetActive(false);
        }
    }

    private void ElementSwap()
    {
        _backgroundImage.color = PlayerElement.Instance.ActiveElementType.color;
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
