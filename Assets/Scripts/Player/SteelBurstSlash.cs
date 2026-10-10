using UnityEngine;

public class SteelBurstSlash : MonoBehaviour
{
    [SerializeField] private float _duration=1f;
    [SerializeField] private float _distance = 5f;



    private Damage _damage;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        float speed = _distance / _duration;

        _spriteRenderer.flipX = direction == Vector2.left;

        _rb.linearVelocity = direction * speed;
       
    }
}
