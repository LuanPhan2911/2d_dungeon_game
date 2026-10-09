using UnityEngine;

public class SteelBurstSlash : MonoBehaviour
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _duration=1f;



    private Damage _damage;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        Destroy(gameObject, _duration);
    }

   


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
        }
    }

    public void Lauch(Vector2 direction, Damage damage)
    {
        _damage = damage;

        _rb.linearVelocity = direction * _speed;
       
    }
}
