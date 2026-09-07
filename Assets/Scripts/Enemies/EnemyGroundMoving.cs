using UnityEngine;

public class EnemyGroundMoving : MonoBehaviour
{

    [SerializeField] private float _speed = 1f;
    [SerializeField] private int _directionX =-1;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _wallMask;
    [SerializeField] private float _groundCheckDistance = 0.5f;
    [SerializeField] private float _wallCheckDistance = 0.25f;
    [SerializeField] private Vector2 _offset;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        
    }

    private void OnDrawGizmos()
    {
        
        Vector2 origin = new Vector2(transform.position.x + _offset.x * _directionX, 
            transform.position.y +_offset.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * _groundCheckDistance);
        Gizmos.DrawLine(origin, origin + Vector2.right * _directionX * _wallCheckDistance);

    }


    public void Move()
    {
        _rb.linearVelocity = new Vector2(_directionX * _speed, _rb.linearVelocityY);
    }
    private void FixedUpdate()
    {
        // wall check
        WallCheck();
        // ground check
        GroundCheck();

    }

    private void FlipDirection()
    {
        _directionX *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

    }


    private void GroundCheck()
    {
        Vector2 origin = new Vector2(transform.position.x + _offset.x * _directionX,
              transform.position.y + _offset.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, _groundCheckDistance, _groundMask);
        if (!hit)
        {
            FlipDirection();
        }
    }
    private void WallCheck()
    {
        Vector2 origin = new Vector2(transform.position.x + _offset.x * _directionX,
             transform.position.y + _offset.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right* _directionX, _wallCheckDistance, _wallMask);
        if (hit)
        {
            FlipDirection();
        }
    }

}
