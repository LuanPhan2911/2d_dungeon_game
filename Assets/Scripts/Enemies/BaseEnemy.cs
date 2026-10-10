using System;
using UnityEngine;
using UnityEngine.Rendering;


[RequireComponent(typeof(DamageFlash))]
[RequireComponent(typeof(Rigidbody2D))]
public class BaseEnemy : MonoBehaviour, IDamageable
{



    public bool IsRecoiling { get; private set;  }
    [SerializeField] private DamageTextUI _damageTextPrefab;
    [SerializeField] private Transform _canvasParent;



    private float _currentHealth;

    [SerializeField] private EnemySO _dataSO;
    private DamageFlash _damageFlash;
    private Rigidbody2D _rb;
    private DebuffEffectManager _debuffEffectManager;


    public event Action<float> OnHealthChange;


    public ElementSO Element => _dataSO.element;
    public float BaseDamage => _dataSO.baseDamage;

   


    private void Awake()
    {
        _damageFlash = GetComponent<DamageFlash>();
        _rb=GetComponent<Rigidbody2D>();
        _debuffEffectManager = GetComponent<DebuffEffectManager>();
    }


    private void Start()
    {
        _currentHealth = _dataSO.maxHealth ;
    }

   

    public virtual void Death()
    {
        Destroy(gameObject);
    }


     public void TakeDamage(Damage damage)
    {



        
        if (damage.canCrit && PlayerAttack.Instance.IsCritStrike())
        {
            damage.isCrit = true;
            damage.amount *= PlayerAttack.Instance.GetCritDamageMultiplier();
        }
        damage.amount *= GetResistanceMultiplier(damage.elementSO);
        damage.amount *= GetDamageBonusMultiplier();

        damage.amount = Math.Max(1f, damage.amount);

        _currentHealth -= damage.amount;
        float healthRatio = Math.Clamp(_currentHealth / _dataSO.maxHealth, 0, _dataSO.maxHealth);

        OnHealthChange?.Invoke(healthRatio);

        _damageFlash.Flash(_dataSO.flashDuration);

        // apply effects
        HandleEffect(damage);

        // spawn damage text

        SpanwDamageTextUI(damage);

        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    private void SpanwDamageTextUI(Damage damage)
    {
        DamageTextUI damageTextUI = Instantiate(_damageTextPrefab, _canvasParent);
        damageTextUI.Spawn(damage);
    }

    private void HandleEffect(Damage damage)
    {
       if (damage.isFromEffect) return;

       foreach(DebuffEffectSO debuffEffectSO in _dataSO.debuffEffectSOArray)
        {
            if(debuffEffectSO.elementSO.type== damage.elementSO.type)
            {
                float valuePerTick = debuffEffectSO.baseValuePerTick;

                if (damage.sourceAttackGameObject)
                {
                    valuePerTick *= GetDebuffEffectIncreasementMultiplier(damage.sourceAttackGameObject);
                }
               

                _debuffEffectManager.ApplyEffect(debuffEffectSO, valuePerTick);
            }
        }
    }


    private float GetDebuffEffectIncreasementMultiplier(GameObject sourceAttackGameObject)
    {
        float multiplier = 1f;
        if(sourceAttackGameObject.TryGetComponent(out BuffEffectManager buffEffectManager))
        {
            if (buffEffectManager.HasEffect(BuffEffectType.DebuffIncreasement))
            {
                multiplier = (1 + buffEffectManager.GetValues(BuffEffectType.DebuffIncreasement));
            }
        }
        return multiplier;
    }

    public float GetResistanceMultiplier(ElementSO element)
    {
        float multiplier = 1;
        ElementalResistance[] resistances = _dataSO.resistances;
        for (int i = 0; i < resistances.Length; i++)
        {
            if (resistances[i].element== element)
            {
                multiplier = 1 - resistances[i].resistance;
                break;
            }
        }



        return multiplier;
    }

    private float GetDamageBonusMultiplier()
    {
        float multiplier = 1f;

        #region Muliplier from status effects
        if (_debuffEffectManager.IsEffectActive(DebuffEffectType.Burn))
        {
            multiplier *= (1 + DebuffEffectManager.BURNING_DAMAGE_BONUS);
        }

        #endregion
        return multiplier;
    }




}
