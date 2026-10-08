using System;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private PlayerHealthSO _data;

    [SerializeField] private Transform _canvasParent;

    [SerializeField] private DamageTextUI _damageTextPrefab;
    public static PlayerHealth Instance { get; private set;  }

    public PlayerElement PElement => PlayerElement.Instance;



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
        OnHealthChanged?.Invoke();
    }

    private void SpanwDamageTextUI(Damage damage)
    {
        DamageTextUI damageTextUI = Instantiate(_damageTextPrefab, _canvasParent);
        damageTextUI.Spawn(damage);
    }

    public void TakeDamage(Damage damage) {

      

       damage.amount *= GetDamageResistanceMultiplier(damage.elementSO);

        ChangeHealth(-damage.amount);

        SpanwDamageTextUI(damage);


        if (_health <= 0f)
        {
            Debug.Log("Player Died");
            return;
        }
    }

    public float GetDamageResistanceMultiplier(ElementSO element)
    {
        float multiplier = 1;
        Debug.Log(2);
        if (!PElement.HasActive) return multiplier;
        

        ElementalResistance[] resistances = PElement.ActiveElement.PlayerElementSO.resistances;
        for (int i = 0; i < resistances.Length; i++)
        {
            if (resistances[i].element == element)
            {
                multiplier = 1 - resistances[i].resistance;
                break;
            }
        }


        Debug.Log(multiplier);
        return multiplier;
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

    public void ConsumeEnergy(float amount)
    {
        ChangeEnergy(-amount);
        
    }
   
  
}
