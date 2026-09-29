    using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerPanelUI : MonoBehaviour
{

    [SerializeField] private Image _energyImage;
    [SerializeField] private Image _healthImage;





    

    private void Start()
    {
        PlayerHealth.Instance.OnHealthChanged +=UpdateHealthUI;
        PlayerHealth.Instance.OnEnergyChanged +=UpdateEnergyUI;

    }

   

    private void UpdateHealthUI( float ratio)
    {
        
        _healthImage.fillAmount = ratio;
    }
    private void UpdateEnergyUI( float ratio)
    {
        _energyImage.fillAmount = ratio;
    }

    private void UpdateElementalSkillUI(float cooldown)
    {

    }



}
