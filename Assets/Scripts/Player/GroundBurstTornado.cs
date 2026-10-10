

using UnityEngine;


public class GroundBurstTornado : MonoBehaviour
{

    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private Vector2 _damageRangeSize;

    private Animator _animator;
 

    const string END_TORNADO = "EndTornado";
    private HorizontalMove _horizontalMove;
    private Damage _damage;
    private float _damageInterval;
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
    private void ScanAndDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(transform.position, _damageRangeSize, 0, _enemyLayerMask);

        
        if(hitEnemies.Length > 0)
        {
            Damage damage = new Damage
            {
                amount = _damage.amount,
                elementSO = _damage.elementSO,
                canCrit = _damage.canCrit,
                isCrit = _damage.isCrit,
                isFromEffect = _damage.isFromEffect,
                sourceAttackGameObject = _damage.sourceAttackGameObject
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
