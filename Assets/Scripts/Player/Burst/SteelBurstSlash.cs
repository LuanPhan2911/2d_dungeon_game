using UnityEngine;

public class SteelBurstSlash : MonoBehaviour
{
 
    private Damage _damage;

    private SpriteRenderer _spriteRenderer;
    private HorizontalMove _horizontalMove;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _horizontalMove = GetComponent<HorizontalMove>();
    }

    private void Start()
    {
        _horizontalMove.OnMovedToDestination += OnMovedToDestination;
        _horizontalMove.OnMovedToObstacle += OnMovedToDestination;
    }

    private void OnMovedToDestination()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
        }
    }

    public void Launch(bool isMoveRight, float distance, float duration)
    {
      
        _spriteRenderer.flipX = !isMoveRight;
        _horizontalMove.IsMoveRight = isMoveRight;
        _horizontalMove.Distance = distance;
        _horizontalMove.Duration = duration;
    }
    public void SetDamage(Damage damage)
    {
        _damage = damage;
    }
}
