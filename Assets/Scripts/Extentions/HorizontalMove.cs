using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class HorizontalMove : MonoBehaviour
{


   
    [SerializeField] private LayerMask _obstacleLayerMask;
    public bool IsMoveRight = true;
    public float Distance = 10f;
    public float Duration = 5f;


    public event Action OnMovedToDestination;
    public event Action OnMovedToObstacle;
   
    private bool _isMoveToTarget;

    private Rigidbody2D _rb;
    private float _timer = 0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        Move();
    }
    private void Update()
    {
        if(_timer > 0f)
        {
            _timer -= Time.deltaTime;
        }
        if (_timer <= 0f && !_isMoveToTarget)
        {
            _isMoveToTarget = true;
            OnMovedToDestination?.Invoke();
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (_obstacleLayerMask.Contains(collision.gameObject.layer))
    //    {
    //        OnMovedToObstacle?.Invoke();
    //    }
    //}

  

    private void Move()
    {
        Vector2 direction = IsMoveRight ? Vector2.right : Vector2.left;
        _timer = Duration;
        float speed = Distance / Duration;
        _rb.linearVelocity = direction * speed;
    }
    public void StopMove()
    {
        _rb.linearVelocity = Vector2.zero;
    }
    
}
