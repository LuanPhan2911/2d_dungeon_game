using UnityEngine;

public class PlayerAttack : MonoBehaviour
{


    public int ComboStep = 0;

    public bool IsAttacking;

    

    [SerializeField] private float _comboResetTime = 0.8f;
    [SerializeField] private int _jumpComboStep = 4;
    [SerializeField] private int _groundComboStep = 2;
    [SerializeField] private int _crouchComboStep = 1;

    [SerializeField] private Transform _attackPoint;

    [SerializeField] private Vector2 _attackOffset;

    [SerializeField] private float _attackRange;



    private float _lastClickTime;
    private int _currentComboStep;


    private PlayerMovement _player;

    private Rigidbody2D _rb;
    private PlayerAnimation _playerAnimation;

    private void Awake()
    {
        _player = GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void OnDrawGizmos()
    {
       
    }
    public void HandleAttack()
    {
        if (_player.IsWallSliding  )
        {
            IsAttacking = false;
            ResetCombo();
            return;
        }

        if(Time.time - _lastClickTime > _comboResetTime && !IsAttacking)
        {
            ResetCombo();
        }

        if (GameInputManager.Instance.PlayerAttackAction.WasPressedThisFrame())
        {
            _lastClickTime= Time.time;

            if (!IsAttacking)
            {
                Attack();
            }
        }
    }
    private void Attack()
    {
        IsAttacking = true;
        _rb.linearVelocity = new Vector2(0, _rb.linearVelocityY);



        int maxComboStep;
       
        if (_player.IsJumping)
        {
            maxComboStep = _groundComboStep;
        }
        else
        {
            maxComboStep = _jumpComboStep;
        }
        if(_currentComboStep != maxComboStep)
        {
            ResetCombo();
        }

        _currentComboStep= maxComboStep;


        ComboStep = ComboStep == maxComboStep ? 1 : ComboStep + 1;


        _playerAnimation.SetTriggerAttack();
        _playerAnimation.SetAttackCombo(ComboStep);
        
    }

    public void PerformHitDetection()
    {
        //Vector3 center = new Vector3(transform.position.x + _attackOffset.x * _player.FacingDirection,
        //   transform.position.y + _attackOffset.y, transform.position.z);

        //Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(center, _attackRange, _player.EnemyLayerMask);

        //foreach(Collider2D hit in hitEnemies)
        //{

            //if(hit.TryGetComponent(out IDamagable damagable))
            //{
            //    Vector2 knockbackDirection = new Vector2(hit.transform.position.x - transform.position.x, 0).normalized;
            //    damagable.TakeDamage(_player.Damage, knockbackDirection);
            //}
        //}

    }
    public void FinishAttack()
    {
        IsAttacking = false; 
    }

    private void ResetCombo()
    {
        ComboStep = 0;
    }
}
