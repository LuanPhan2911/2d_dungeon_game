    using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerPanelUI : MonoBehaviour
{


    [SerializeField] private StatBarUI _healthBar;
    [SerializeField] private StatBarUI _energyBar;





    

    private void Start()
    {
        UpdateHealthUI();
        UpdateEnergyUI();

        PlayerHealth.Instance.OnHealthChanged +=UpdateHealthUI;
        PlayerHealth.Instance.OnEnergyChanged +=UpdateEnergyUI;

    }

   

    private void UpdateHealthUI()
    {
        _healthBar.SetImageFill(PlayerHealth.Instance.HealthRatio);
        _healthBar.SetText(PlayerHealth.Instance.GetHealthText());
    }
    private void UpdateEnergyUI()
    {
        _energyBar.SetImageFill(PlayerHealth.Instance.EnergyRatio);
        _energyBar.SetText(PlayerHealth.Instance.GetEnergyText());
    }

  


}
