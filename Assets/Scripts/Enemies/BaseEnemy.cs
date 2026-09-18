using System.Collections;
using UnityEngine;


[RequireComponent(typeof(DamageFlash))]
[RequireComponent(typeof(Rigidbody2D))]
public class BaseEnemy : MonoBehaviour, IDamageable
{


    public bool CanRecoil;

    public bool IsRecoiling { get; private set;  }
    [SerializeField] private int _maxHp;
    [SerializeField] private float _recoilForce = 2f;
    [SerializeField] private float _recoilDuration = 0.2f;
    [SerializeField] private float _flashDuration = 0.2f;



   
    private DamageFlash _damageFlash;
    private Rigidbody2D _rb;

   


    private void Awake()
    {
        _damageFlash = GetComponent<DamageFlash>();
        _rb=GetComponent<Rigidbody2D>();
    }
    private int _Hp;

    private void Start()
    {
        _Hp = _maxHp;
    }

   

    public virtual void Death()
    {
        Destroy(gameObject);
    }


     public void TakeDamage(int damage, Vector2 recoilDirection)
    {
        _Hp -= damage;
        _damageFlash.Flash(_flashDuration);

        if (CanRecoil)
        {
            StartCoroutine(StartRecoil(recoilDirection));
        }
        if (_Hp <= 0)
        {
            Death();
        }
    }

    private IEnumerator StartRecoil(Vector2 direction)
    {

        IsRecoiling = true;

        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(direction *  _recoilForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(_recoilDuration);

        IsRecoiling = false;
    }
  
   


}
