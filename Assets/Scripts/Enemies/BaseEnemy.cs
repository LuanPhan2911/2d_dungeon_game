using UnityEngine;


[RequireComponent(typeof(DamageFlash))]
[RequireComponent(typeof(KnockbackReceiver))]
public class BaseEnemy : MonoBehaviour, IDamagable
{


    [SerializeField] private int _maxHp;
    [SerializeField] private float _knockbackForce = 2f;
    [SerializeField] private float _knockbackDuration = 0.2f;
    [SerializeField] private float _flashDuration = 0.2f;
    [SerializeField] private int _touchPlayerDamage = 1;

    private DamageFlash _damageFlash;

    private KnockbackReceiver _knockbackReceiver;
    private EnemyGroundMoving _enemyGroundMoving;


    private void Awake()
    {
        _damageFlash = GetComponent<DamageFlash>();
        _knockbackReceiver = GetComponent<KnockbackReceiver>();
        _enemyGroundMoving = GetComponent<EnemyGroundMoving>();
    }
    private int _Hp;

    private void Start()
    {
        _Hp = _maxHp;
    }

    private void Update()
    {
        if (!_knockbackReceiver.IsKnockbacked)
        {

            _enemyGroundMoving.Move();
        }
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }


     public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        _Hp -= damage;
        _damageFlash.PingPongFlash(_flashDuration);
        _knockbackReceiver.Knockback(knockbackDirection, _knockbackForce, _knockbackDuration);

        if (_Hp <= 0)
        {
            Death();
        }
    }
  
    private void OnCollisionStay2D(Collision2D collision)
    {


        if (collision.collider.TryGetComponent(out PlayerTakenDamage playerTakenDamage))
        {



            playerTakenDamage.TakeDamage(_touchPlayerDamage);
        }
    }


}
