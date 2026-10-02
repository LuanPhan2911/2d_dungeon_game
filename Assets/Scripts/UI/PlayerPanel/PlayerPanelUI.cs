    using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerPanelUI : MonoBehaviour
{


    [SerializeField] private StatBarUI _healthBar;
    [SerializeField] private StatBarUI _energyBar;
    [SerializeField] private StatBarUI _staminaBar;

    





    

    private void Start()
    {
        UpdateHealthUI();
        UpdateEnergyUI();

        PlayerHealth.Instance.OnHealthChanged +=UpdateHealthUI;
        PlayerHealth.Instance.OnEnergyChanged +=UpdateEnergyUI;
       

    }
    private void Update()
    {
        UpdateStaminaUI();
    }

    private void UpdateStaminaUI()
    {
        _staminaBar.SetImageFill(PlayerHealth.Instance.StaminaRatio);
     
    }

    private void UpdateHealthUI()
    {
        _healthBar.SetImageFill(PlayerHealth.Instance.HealthRatio);
   
    }
    private void UpdateEnergyUI()
    {
        _energyBar.SetImageFill(PlayerHealth.Instance.EnergyRatio);
       
    }

  


}
