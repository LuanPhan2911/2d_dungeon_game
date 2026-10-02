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
    public float StaminaRatio => _stamina / GetMaxStamina();

    public float CurrentEnergy => _energy;
    public float CurrentHealth => _health;

    public float CurrentStamina => _stamina;


    private void Awake()
    {
        Instance = this;
    }

    private float _health;
    private float _energy;
    private float _stamina;
    private float _staminaRegenDelayTimer;


    private float GetMaxHealth()
    {
        return _data.baseHealth;
    }
    private float GetMaxEnergy()
    {
        return _data.baseEnergy;
    }
    private float GetMaxStamina()
    {
        return _data.baseStamina;
    }

    void Start()
    {
        _health = GetMaxHealth();
        _energy = 0f;
    }

    private void Update()
    {
        HandleRegenStamina();
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
    private void SetStaminaDelayTimer()
    {
        _staminaRegenDelayTimer = _data.staminaRegainDelay;
    }

    public void ConsumeStamina(float amount) {
        _stamina = Mathf.Clamp(_stamina - amount, 0f, GetMaxStamina());
        SetStaminaDelayTimer();
    }

    
    private void HandleRegenStamina()
    {
        if(_stamina < GetMaxStamina())
        {
            _staminaRegenDelayTimer -= Time.deltaTime;
            if(_staminaRegenDelayTimer <= 0f)
            {
                _stamina += _data.staminaRegenRate * Time.deltaTime;
                _stamina = Mathf.Clamp(_stamina, 0f, GetMaxStamina());
            }
           
        }   
    }
   

    private void ChangeEnergy(float amount)
    {
        _energy = Mathf.Clamp(_energy + amount, 0f, GetMaxEnergy());
        OnEnergyChanged?.Invoke();
    }

    public void GainEnergyFromAttack()
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
