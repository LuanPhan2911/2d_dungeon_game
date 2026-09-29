using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementalSkillUI : MonoBehaviour
{
    [SerializeField] private Image _activeImage;
    [SerializeField] private TextMeshProUGUI _cooldownText;

    private float _currentDuration;


   

    private void Update()
    {

       

        if (PlayerAttack.Instance.IsElementalSkillCooldown)
        {
            _activeImage.gameObject.SetActive(true);
            _cooldownText.gameObject.SetActive(true);
            if(_currentDuration!= PlayerAttack.Instance.ElementalSkillDuration)
            {
                _cooldownText.text = $"{PlayerAttack.Instance.ElementalSkillDuration}";
               
            }
            _currentDuration = PlayerAttack.Instance.ElementalSkillDuration;
        }
        else
        {
            _activeImage.gameObject.SetActive(false);
            _cooldownText.gameObject.SetActive(false);
        }
    }


    
}
