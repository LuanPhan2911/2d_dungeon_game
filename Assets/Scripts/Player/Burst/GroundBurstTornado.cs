

using System.Collections.Generic;
using UnityEngine;


public class GroundBurstTornado : MonoBehaviour
{

    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private Vector2 _damageRangeSize;

    [SerializeField] private float _pullSpeed = 10f;


    private Animator _animator;
 

    const string END_TORNADO = "EndTornado";
    private HorizontalMove _horizontalMove;
    private Damage _damage;
    private float _damageInterval=0.5f;
    private float _damageIntervalTimer;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _horizontalMove = GetComponent<HorizontalMove>();
        
    }

    private void Start()
    {
        _horizontalMove.OnMovedToDestination += MovedToDestination;
        _horizontalMove.OnMovedToObstacle += MovedToObstacle;
    }

    private void Update()
    {

        if(_damageIntervalTimer > 0f)
        {
            _damageIntervalTimer -= Time.deltaTime;
        }
        if (_damageIntervalTimer <= 0f)
        {
            ScanAndDamage();
           
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_enemyLayerMask.Contains(collision.gameObject.layer))
        {
            if (collision.TryGetComponent(out Rigidbody2D enemyRb))
            {
                enemyRb.linearVelocity = Vector2.zero;
                
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_enemyLayerMask.Contains(collision.gameObject.layer))
        {
            if(collision.TryGetComponent( out Rigidbody2D enemyRb))
            {
                Vector2 enemyPosition = enemyRb.transform.position;
                Vector2 centerPosition = transform.position;


                enemyRb.transform.position = 
                    Vector2.MoveTowards( enemyPosition, centerPosition, _pullSpeed * Time.deltaTime);

            } 
        }
    }

   
    private void ScanAndDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(transform.position, _damageRangeSize, 0, _enemyLayerMask);

        
        if(hitEnemies.Length > 0)
        {
            Damage damage = new Damage
            {
                Amount = _damage.Amount,
                ElementSO = _damage.ElementSO,
                CanCrit = _damage.CanCrit,
                IsCrit = _damage.IsCrit,
                IsFromEffect = _damage.IsFromEffect,
                SourceAttackGameObject = _damage.SourceAttackGameObject
            };
            
            foreach (Collider2D hit in hitEnemies)
            {
                if (hit.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damage);

                }
            }
            _damageIntervalTimer = _damageInterval;
        }
    }

    public void Launch(bool isMoveRight, float distance, float duration)
    {
        _horizontalMove.IsMoveRight = isMoveRight;
        _horizontalMove.Distance = distance;
        _horizontalMove.Duration = duration;
        
    }
    public void SetDamage(Damage damage, float damageInterval)
    {
        _damage = damage;
        _damageInterval = damageInterval;
    }
    private void MovedToDestination()
    {
        _animator.SetTrigger(END_TORNADO);
    }
    private void MovedToObstacle()
    {
        _horizontalMove.StopMove();
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, _damageRangeSize);

     
    }


}
