using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementalSkillUI : MonoBehaviour
{
    [SerializeField] private Image _activeImage;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _cooldownText;

    private float _currentDuration;

    private void Start()
    {
        PlayerElement.Instance.OnElementSwapped += ElementSwap;

        if (PlayerElement.Instance.CurrentPlayerElement == null)
        {
            Hide();
        }
    }
    private void ElementSwap()
    {
        _backgroundImage.color = PlayerElement.Instance.CurrentElementType.color;
        Show();
    }


    private void Update()
    {
       
      
        if (PlayerSkill.Instance.IsElementalSkillCooldown)
        {
            _activeImage.gameObject.SetActive(true);
            _cooldownText.gameObject.SetActive(true);
            _activeImage.fillAmount = PlayerSkill.Instance.ElementalSkillRatio;
            if(_currentDuration!= PlayerSkill.Instance.ElementalSkillDuration)
            {
                _cooldownText.text = $"{PlayerSkill.Instance.ElementalSkillDuration}";
               
            }
            _currentDuration = PlayerSkill.Instance.ElementalSkillDuration;
        }
        else
        {
            _activeImage.gameObject.SetActive(false);
            _cooldownText.gameObject.SetActive(false);
        }
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
