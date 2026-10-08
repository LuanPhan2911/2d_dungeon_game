using System;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(DamageFlash))]
[RequireComponent(typeof(Rigidbody2D))]
public class BaseEnemy : MonoBehaviour, IDamageable
{



    public bool IsRecoiling { get; private set;  }
    [SerializeField] private DamageTextUI _damageTextPrefab;
    [SerializeField] private Transform _canvasParent;



    private float _currentHealth;

    [SerializeField] private EnemySO _enemySO;
    private DamageFlash _damageFlash;
    private Rigidbody2D _rb;
    private StatusEffectManager _effectManager;


    public event Action<float> OnHealthChange;


    public ElementSO Element => _enemySO.element;
    public float BaseDamage => _enemySO.baseDamage;

   


    private void Awake()
    {
        _damageFlash = GetComponent<DamageFlash>();
        _rb=GetComponent<Rigidbody2D>();
        _effectManager = GetComponent<StatusEffectManager>();
    }


    private void Start()
    {
        _currentHealth = _enemySO.maxHealth ;
    }

   

    public virtual void Death()
    {
        Destroy(gameObject);
    }


     public void TakeDamage(Damage damage)
    {



        damage.amount *= GetResistanceMultiplier(damage.elementSO);


        damage.amount *= GetDamageMultiplier();



        _currentHealth -= damage.amount;
        float healthRatio = Math.Clamp(_currentHealth / _enemySO.maxHealth, 0, _enemySO.maxHealth);

        OnHealthChange?.Invoke(healthRatio);

        _damageFlash.Flash(_enemySO.flashDuration);

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


       if(damage.elementSO== _enemySO.posionEffectSO.elementSO)
        {
            _effectManager.ApplyEffect(_enemySO.posionEffectSO);
        } else if (damage.elementSO == _enemySO.burnEffectSO.elementSO)
        {
            _effectManager.ApplyEffect(_enemySO.burnEffectSO);
        }
    }

    public float GetResistanceMultiplier(ElementSO element)
    {
        float multiplier = 1;
        ElementalResistance[] resistances = _enemySO.resistances;
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

    private float GetDamageMultiplier()
    {
        float multiplier = 1f;

        #region Muliplier from status effects
        if (_effectManager.IsEffectActive(EffectType.Burn))
        {
            multiplier*= (1+ _effectManager.GetDamageBonusPercent(EffectType.Burn));
        }


        #endregion
        return multiplier;
    }




}
