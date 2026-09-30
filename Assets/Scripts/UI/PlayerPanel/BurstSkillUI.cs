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
    
      

        if (!PlayerElement.Instance.HasActivePlayerElement)
        {
            Hide();
        }
    }
    private void Update()

    {
        if (!PlayerElement.Instance.HasActivePlayerElement) return;


        if (PlayerSkill.Instance.IsBurstSkillCooldown)
        {
            _activeImage.gameObject.SetActive(true);
            _cooldownText.gameObject.SetActive(true);
            _activeImage.fillAmount = PlayerSkill.Instance.BurstSkillRatio;
            if (_currentDuration != PlayerSkill.Instance.BurstSkillDuration)
            {
                _cooldownText.text = $"{PlayerSkill.Instance.BurstSkillDuration}";

            }
            _currentDuration = PlayerSkill.Instance.BurstSkillDuration;
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
