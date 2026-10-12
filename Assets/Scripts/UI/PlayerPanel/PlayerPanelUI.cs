    using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerPanelUI : MonoBehaviour
{
    [SerializeField] private StatBarUI _healthBar;
  
    private void Start()
    {
        UpdateHealthUI();
        PlayerHealth.Instance.OnHealthChanged +=UpdateHealthUI;
    }
   
    private void UpdateHealthUI()
    {
        _healthBar.SetImageFill(PlayerHealth.Instance.HealthRatio);
   
    }

}
