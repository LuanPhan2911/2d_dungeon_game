using System;
using UnityEngine;


public class PlayerHealth : MonoBehaviour, IDamageable
{

    [SerializeField] private PlayerHealthSO _dataSO;

    [SerializeField] private Transform _canvasParent;

    public static PlayerHealth Instance { get; private set;  }

    public PlayerElement PElement => PlayerElement.Instance;



    public event Action OnHealthChanged;
    public event Action OnEnergyChanged;


    public float HealthRatio => _health / GetMaxHealth();

    public float CurrentHealth => _health;




    private DebuffEffectManager _debuffEffectManager;
    private BuffEffectManager _buffEffectManager;


    private float _healPerDurationTimer;
    private float _healPerDurationIntervalTimer;
    private HealPerDuration _healPerduration;


    private void Awake()
    {
        Instance = this;
        _debuffEffectManager = GetComponent<DebuffEffectManager>();
        _buffEffectManager = GetComponent<BuffEffectManager>();
    }

    private float _health;


    public float GetMaxHealth()
    {
        return _dataSO.baseHealth;
    }
 

    void Start()
    {
        _health = GetMaxHealth();
    }

    private void Update()
    {
       
        HandleHealPerDuration();
    }


    private void ChangeHealth(float amount)
    {
        _health = Mathf.Clamp(_health + amount, 0f, GetMaxHealth());
        OnHealthChanged?.Invoke();
    }

   
    public void TakeDamage(Damage damage) {

        

        damage.amount *= GetResistanceMultiplier(damage.elementSO);

        damage.amount *= GetDamageBonusMultiplier();

        HandleEffect(damage);



        ChangeHealth(-damage.amount);

       


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


        foreach (DebuffEffectSO debuffEffectSO in _dataSO.debuffEffectSOArray)
        {
            if (debuffEffectSO.elementSO.type == damage.elementSO.type)
            {
                float valuePerTick = debuffEffectSO.baseValuePerTick;
                _debuffEffectManager.ApplyEffect(debuffEffectSO, valuePerTick);
            }
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


    private float GetValuePerTickOfDebuffEffect(DebuffEffectSO debuffEffectSO)
    {
        return debuffEffectSO.baseValuePerTick;
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
   
  
  
    
   

    public void RestoreHealth(float amount)
    {
        ChangeHealth(amount);
        
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
            RestoreHealth(_healPerduration.rate * GetMaxHealth());
            _healPerDurationIntervalTimer = _healPerduration.interval;
        }
    }
  
}
