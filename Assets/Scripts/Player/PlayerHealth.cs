using System;
using UnityEngine;


public class PlayerHealth : MonoBehaviour, IDamageable
{

    [SerializeField] private PlayerHealthSO _dataSO;


    public static PlayerHealth Instance { get; private set;  }

    public PlayerElement PElement => PlayerElement.Instance;

    public bool IsInvincible;



    public event Action OnHealthChanged;
    public event Action OnHealPotionCountChanged;


    public float HealthRatio => _health / GetMaxHealth();
    public float BaseHealthRatio => _health / _dataSO.baseHealth;

    public float CurrentHealth => _health;

    public int CurrentHealPotionCount => _healPotionCount;



    private DebuffEffectManager _debuffEffectManager;
    private BuffEffectManager _buffEffectManager;


    private float _healPerDurationTimer;
    private float _healPerDurationIntervalTimer;
    private HealPerDuration _healPerduration;


    private float _health;

    private int _healPotionCount;
    private float _healPotionTimer;
    private bool _isUseHealPotion;
 


    private void Awake()
    {
        Instance = this;
        _debuffEffectManager = GetComponent<DebuffEffectManager>();
        _buffEffectManager = GetComponent<BuffEffectManager>();

        _health = GetMaxHealth();
        _healPotionCount = GetMaxHealPotionCount();


    }

 


    public float GetMaxHealth()
    {
        return _dataSO.baseHealth;
    }
    public int GetMaxHealPotionCount()
    {
        return _dataSO.baseHealPotionCount;
    }
    private float GetMaxHealPotionRate()
    {
        return _dataSO.baseHealPotionRate;
    }
    private float GetHealPotionTime()
    {
        return _dataSO.healPotionTime;
    }


    private void Start()
    {
        OnHealthChanged?.Invoke();
        OnHealPotionCountChanged?.Invoke();
    }


    private void Update()
    {


        HandleHealPotion();
        HandleHealPerDuration();

       
    }
    private void HandleHealPotion()
    {
       

        _healPotionTimer -= Time.deltaTime;
        if (!_isUseHealPotion && GameInputManager.Instance.PlayerActions.Heal.WasPressedThisFrame())
        {
            if (_healPotionCount <= 0) return;


            _healPotionCount--;
            _healPotionTimer = GetHealPotionTime();
            _isUseHealPotion = true;
            OnHealPotionCountChanged?.Invoke();
        }
        if (_isUseHealPotion && _healPotionTimer <= 0)
        {
            float amount = GetMaxHealth() * GetMaxHealPotionRate();
            Heal(amount);
            _isUseHealPotion = false;

        }
    }


    private void ChangeHealth(float amount)
    {
        _health = Mathf.Clamp(_health + amount, 0f, GetMaxHealth());
        OnHealthChanged?.Invoke();
    }

    public void Heal(float amount)
    {
     
        ChangeHealth(amount);
    }
   
    public void TakeDamage(Damage damage) {

        if (IsInvincible) return;
        

        damage.Amount *= GetResistanceMultiplier(damage.ElementSO);

        damage.Amount *= GetDamageBonusMultiplier();

        HandleEffect(damage);
        ChangeHealth(-damage.Amount);

       


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
        if (damage.IsFromEffect) return;


        foreach (DebuffEffectSO debuffEffectSO in _dataSO.debuffEffectSOArray)
        {
            if (debuffEffectSO.elementSO.type == damage.ElementSO.type)
            {
                float valuePerTick = debuffEffectSO.baseValuePerTick;
                _debuffEffectManager.ApplyEffect(debuffEffectSO, valuePerTick);
            }
        }


        // counter attack

        if (_buffEffectManager.HasEffect(BuffEffectType.CounterAttack)){

            float amount = damage.Amount * _buffEffectManager.GetValues(BuffEffectType.CounterAttack);

            Damage counterDamage = new Damage
            {
                Amount = amount,
                CanCrit =false,
                IsFromEffect = true,
                ElementSO = damage.ElementSO,
                SourceAttackGameObject = gameObject

            };
            if(damage.SourceAttackGameObject && 
                damage.SourceAttackGameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(counterDamage);
            }
        }
    }

    public float GetResistanceMultiplier(ElementSO element)
    {
        float multiplier = 1;

        float resistance = GetResistanceByElement(element);
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
  

    public void SetHealPerDuration(HealPerDuration healPerDuration)
    {
        _healPerduration = healPerDuration;
        _healPerDurationTimer = healPerDuration.duration;
        _healPerDurationIntervalTimer = healPerDuration.interval;
    }
    private void HandleHealPerDuration()
    {
        if (_healPerDurationTimer <= 0f) return;

        _healPerDurationTimer -= Time.deltaTime;

        if(_healPerDurationIntervalTimer > 0f)
        {
            _healPerDurationIntervalTimer -= Time.deltaTime;

        }
        else
        {
            ChangeHealth(_healPerduration.rate * GetMaxHealth());
            _healPerDurationIntervalTimer = _healPerduration.interval;
        }
    }


    public float GetResistanceByElement(ElementSO element)
    {
        float value = 0f;
        PlayerElementSlot onFieldPlayerElementSlot = PElement.GetOnFieldPlayerElementSlot();
        if (PElement.HasPlayerElementSlot && onFieldPlayerElementSlot!= null)
        {
            float allResistances = _buffEffectManager.HasEffect(BuffEffectType.AllResistances) ?
                     _buffEffectManager.GetValues(BuffEffectType.AllResistances) : 0f;

            foreach (var item in onFieldPlayerElementSlot.PlayerElementType.DataSO.resistances)
            {
                if (item.element == element)
                {
                    value += item.resistance + allResistances;
                    break;
                }
            }
        }

        foreach (var item in _buffEffectManager.GetResiatanceBonusArray())
        {
            if (item.element == element)
            {
                value += item.resistance;
                break;
            }
        }
        return value;
    }
}
