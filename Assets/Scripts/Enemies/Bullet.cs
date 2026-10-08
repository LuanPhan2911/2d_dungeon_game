using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _lifeTime = 5f;

    [SerializeField] private LayerMask _obstacleLayer;

    private Damage _damage;


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
        if(collision.TryGetComponent(out PlayerHealth playerHealth))
        {

            playerHealth.TakeDamage(_damage);
            Destroy(gameObject);
        }
        else if(_obstacleLayer.Contains(collision.gameObject.layer))
        {
            Destroy(gameObject);
        }

    }

    public void Launch(Vector2 direction, Damage damage)
    {
        _rb.linearVelocity= direction * _speed;
        _damage = damage;
    }


}
