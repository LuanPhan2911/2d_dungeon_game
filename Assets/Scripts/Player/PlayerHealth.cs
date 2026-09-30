using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private PlayerHealthSO _data;

    public static PlayerHealth Instance { get; private set;  }

    public event Action OnHealthChanged;
    public event Action OnEnergyChanged;


    public float HealthRatio => _health / GetMaxHealth();
    public float EnergyRatio => _energy / GetMaxEnergy();

    public float CurrentEnergy => _energy;
    public float CurrentHealth => _health;


    private void Awake()
    {
        Instance = this;
    }

    private float _health;
    private float _energy;


    private float GetMaxHealth()
    {
        return _data.baseHealth;
    }
    private float GetMaxEnergy()
    {
        return _data.baseEnergy;
    }

    void Start()
    {
        _health = GetMaxHealth();
        _energy = 0f;
    }

    public string GetHealthText()
    {
        return $"{_health}/{GetMaxHealth()}";
    }
    public string GetEnergyText()
    {
        return $"{_energy}/{GetMaxEnergy()}";
    }


    private void ChangeHealth(float amount)
    {
        _health = Mathf.Clamp(_health + amount, 0f, GetMaxHealth());

        if(_health <= 0f)
        {
            Debug.Log("player die");
        }

      


        OnHealthChanged?.Invoke();
    }

    private void ChangeEnergy(float amount)
    {
        _energy = Mathf.Clamp(_energy + amount, 0f, GetMaxEnergy());
        OnEnergyChanged?.Invoke();
    }

    public void GainEnergyFromNormalAttack()
    {
        ChangeEnergy(_data.gainingEnergyFromNormalAttack);
    }
    public void GainEnergyFormElementalSKill()
    {
        ChangeEnergy(_data.gainingEnergyFromElementalSkill);
    }

    public bool IsEnoughEnergyToUseBurstSkill()
    {
        return _energy >= _data.energyNeedForBurstSkill;
    }

    public void UseEnergyForBurstSkill()
    {
        ChangeEnergy(-_data.energyNeedForBurstSkill);
    }
  
}
