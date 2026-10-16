using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifeTime = 5f;

    [SerializeField] private LayerMask _obstacleLayer;
   

    private Damage _damage;
    private bool _isReflected = false;


    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!_isReflected && collision.TryGetComponent(out PlayerParry playerParry) )
        {
            // parry success
            if (playerParry.CheckParryValid(transform.position))
            {
               
                ReflectProjectile();
                return;
            }
            // parry fail
            if (collision.TryGetComponent(out PlayerHealth playerHealth))
            {

                playerHealth.TakeDamage(_damage);
                Destroy(gameObject);
            }

        }

        // reflect bullet attack enemy
        if(_isReflected && collision.TryGetComponent(out BaseEnemy enemy))
        {
            _damage.SourceAttackGameObject = PlayerHealth.Instance.gameObject;
            _damage.CanCrit = false;

            enemy.TakeDamage(_damage);
            Destroy(gameObject);

        }
        
       // bullet hit wall or ground 
        
        if(_obstacleLayer.Contains(collision.gameObject.layer))
        {
            Destroy(gameObject);
        }

    }
    private void ReflectProjectile()
    {
        _isReflected = true;
        _rb.linearVelocity = -_rb.linearVelocity;

        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

    }

    public void Launch(Vector2 direction, Damage damage)
    {
        _rb.linearVelocity= direction * _speed;
        _damage = damage;
    }


}
