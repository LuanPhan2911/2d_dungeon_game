using System;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(DamageFlash))]
[RequireComponent(typeof(Rigidbody2D))]
public class BaseEnemy : MonoBehaviour, IDamageable
{


    public bool CanRecoil;

    public bool IsRecoiling { get; private set;  }
    [SerializeField] private DamageTextUI _damageTextPrefab;
    [SerializeField] private Transform _canvasParent;



    private float _currentHealth;

    [SerializeField] private EnemyData _data;
    private DamageFlash _damageFlash;
    private Rigidbody2D _rb;


    public event Action<float> OnHealthChange;

   


    private void Awake()
    {
        _damageFlash = GetComponent<DamageFlash>();
        _rb=GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        _currentHealth = _data.maxHealth ;
    }

   

    public virtual void Death()
    {
        Destroy(gameObject);
    }


     public void TakeDamage(Damage damage, Vector2 recoilDirection)
    {

       
        
        damage.amount *= GetResistanceMultiplier(damage.element);
    
    

        _currentHealth -= damage.amount;
        float healthRatio = Math.Clamp(_currentHealth / _data.maxHealth, 0, _data.maxHealth);

        OnHealthChange?.Invoke(healthRatio);

        _damageFlash.Flash(_data.flashDuration);

        // spawn damage text
      
        DamageTextUI damageTextUI = Instantiate(_damageTextPrefab, _canvasParent);

        damageTextUI.SetText(damage);

        if (CanRecoil)
        {
            StartCoroutine(StartRecoil(recoilDirection));
        }
        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    private IEnumerator StartRecoil(Vector2 direction)
    {

        IsRecoiling = true;

        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(direction *  _data.recoilForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(_data.recoilDuration);

        IsRecoiling = false;
    }

    public float GetResistanceMultiplier(ElementData element)
    {
        float multiplier = 1;
        ElementalResistance[] resistances = _data.resistances;
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
  
   


}
