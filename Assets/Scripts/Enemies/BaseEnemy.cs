using System;
using UnityEngine;
using UnityEngine.Rendering;


[RequireComponent(typeof(DamageFlash))]
[RequireComponent(typeof(Rigidbody2D))]
public class BaseEnemy : MonoBehaviour, IDamageable
{



    public bool IsRecoiling { get; private set;  }
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
        damage.Amount *= GetResistanceMultiplier(damage.ElementSO);
       
        damage.Amount = Math.Max(1f, damage.Amount);

        _currentHealth -= damage.Amount;

        float healthRatio = Math.Clamp(_currentHealth / _dataSO.maxHealth, 0, _dataSO.maxHealth);

        OnHealthChange?.Invoke(healthRatio);

        _damageFlash.Flash(_dataSO.flashDuration);

        // apply effects
        HandleEffect(damage);

        // spawn damage text

        if (_currentHealth <= 0)
        {
            Death();
        }
    }


    private void HandleEffect(Damage damage)
    {
       if (damage.IsFromEffect) return;

       foreach(DebuffEffectSO debuffEffectSO in _dataSO.debuffEffectSOArray)
        {
            if(debuffEffectSO.elementSO.type== damage.ElementSO.type)
            {
                float valuePerTick = debuffEffectSO.baseValuePerTick;

                _debuffEffectManager.ApplyEffect(debuffEffectSO, valuePerTick);
            }
        }
    }

    public float GetResistanceMultiplier(ElementSO element)
    {
        float multiplier = 1;
        ElementalResistance[] resistances = _dataSO.resistances;

        for (int i = 0; i < resistances.Length; i++)
        {
            float resistance = resistances[i].resistance;

            if (resistances[i].element== element)
            {
                if (resistance < 0)
                {
                    multiplier = 1 - (resistance / 2);
                }
                else if (resistance >= 0f && resistance <= 0.75f)
                {
                    multiplier = 1 - resistance;
                }
                else if (resistance > 0.75f)
                {
                    multiplier = 1f / (4 * resistance + 1);
                }

            }
        }



        return multiplier;
    }

  


}
