using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{


    [SerializeField] private Bullet _bullet;

    [SerializeField] private float _attackRange = 10f;

    [SerializeField] private float _attackCooldown = 2f;




    private float _attackTimer;
    private BaseEnemy _enemy;
    private void Awake()
    {
         _attackTimer = _attackCooldown;
        _enemy = GetComponent<BaseEnemy>();
    }


    private void Update()
    {
      
        if (PlayerMovement.Instance.GetDistanceFromPosition(transform.position) <= _attackRange)
        {
            _attackTimer -= Time.deltaTime;
            if (_attackTimer <= 0f)
            {
                Attack();
                _attackTimer = _attackCooldown;
            }
        }
        else
        {
            _attackTimer = _attackCooldown;
        }
    }
    private void Attack()
    {
        Debug.Log("Enemy attacks with projectile!");
        Bullet bullet = Instantiate(_bullet, transform.position, Quaternion.identity);
        Vector2 direction = (PlayerMovement.Instance.transform.position - transform.position).normalized;

        Damage damage = new Damage
        {
            elementSO = _enemy.Element,
            amount = _enemy.BaseDamage,
            
            
        };
        bullet.Launch(direction, damage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
