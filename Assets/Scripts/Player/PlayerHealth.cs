using System;
using UnityEngine;


public class PlayerHealth : MonoBehaviour, IDamageable
{

    [SerializeField] private PlayerHealthSO _dataSO;

    public static PlayerHealth Instance { get; private set;  }
    public bool IsInvincible;

    public event Action OnHealthChanged;
    public event Action OnHealPotionCountChanged;

    public float HealthRatio => _health / GetMaxHealth();
    public float BaseHealthRatio => _health / _dataSO.baseHealth;

    public float CurrentHealth => _health;

    public int CurrentHealPotionCount => _healPotionCount;



    private DebuffEffectManager _debuffEffectManager;
    private BuffEffectManager _buffEffectManager;
   

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

   

  
   
   
}
