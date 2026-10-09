using System;
using UnityEngine;


public class PlayerHealth : MonoBehaviour, IDamageable
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



    private DebuffEffectManager _debuffEffectManager;
    private BuffEffectManager _buffEffectManager;


    private void Awake()
    {
        Instance = this;
        _debuffEffectManager = GetComponent<DebuffEffectManager>();
        _buffEffectManager = GetComponent<BuffEffectManager>();
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

        

        damage.amount *= GetResistanceMultiplier(damage.elementSO);

        damage.amount *= GetDamageBonusMultiplier();

        HandleEffect(damage);



        ChangeHealth(-damage.amount);

        SpanwDamageTextUI(damage);


        if (_health <= 0f)
        {
            Debug.Log("Player Died");
            return;
        }
    }
    private float GetDamageBonusMultiplier()
    {
        float multiplier = 1f;

        #region Muliplier from status effects

        if (_buffEffectManager.HasEffect(BuffEffectType.DamageReduction))
        {
            multiplier *= (1 - _buffEffectManager.GetValues(BuffEffectType.DamageReduction));
        }


        #endregion
        return multiplier;
    }

    private void HandleEffect(Damage damage)
    {
        if (damage.isFromEffect) return;


        // apply effect
        if (damage.elementSO.type == ElementalType.Grass)
        {
            _debuffEffectManager.ApplyEffect(_data.posionEffectSO);
        }
        else if (damage.elementSO.type == ElementalType.Fire)
        {
            _debuffEffectManager.ApplyEffect(_data.burnEffectSO);
        }


        // counter attack

        if (_buffEffectManager.HasEffect(BuffEffectType.CounterAttack)){

            float amount = damage.amount * _buffEffectManager.GetValues(BuffEffectType.CounterAttack);

            Damage counterDamage = new Damage
            {
                amount = amount,
                canCrit =false,
                isFromEffect = true,
                elementSO = PElement.SteelElementSO,
                sourceAttackGameObject = gameObject

            };
            if(damage.sourceAttackGameObject && 
                damage.sourceAttackGameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(counterDamage);
            }
        }
    }


   

    public float GetResistanceMultiplier(ElementSO element)
    {
        float multiplier = 1;

        float resistance = PElement.GetResistanceByElement(element);
        if(resistance < 0)
        {
            multiplier = 1 - (resistance / 2);
        }else if(resistance>=0f && resistance <= 0.75f)
        {
            multiplier = 1 - resistance;
        }else if(resistance> 0.75f)
        {
            multiplier = 1f / (4 * resistance + 1);
        }
        

        return multiplier ;
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
