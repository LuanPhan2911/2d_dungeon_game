using UnityEngine;

public class PlayerAttack : MonoBehaviour
{


    public int ComboStep = 0;

    

    [SerializeField] private float _comboResetTime = 0.8f;
    [SerializeField] private int _jumpComboStep = 4;
    [SerializeField] private int _groundComboStep = 2;
    [SerializeField] private int _crouchComboStep = 1;

    [SerializeField] private Transform _attackPoint;

    [SerializeField] private Vector2 _attackOffset;

    [SerializeField] private float _attackRange;



    private float _lastClickTime;
    private int _currentComboStep;


    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        int direction = _player != null ? _player.FacingDirection : 1;

        Vector3 center = new Vector3(transform.position.x + _attackOffset.x * direction, 
            transform.position.y + _attackOffset.y, transform.position.z);
        Gizmos.DrawWireSphere(center, _attackRange);
    }
    public void HandleAttack()
    {
        if (_player.IsWallSliding  || _player.IsStopAction)
        {
            _player.IsAttacking = false;
            ResetCombo();
            return;
        }

        if(Time.time - _lastClickTime > _comboResetTime && !_player.IsAttacking)
        {
            ResetCombo();
        }

        if (GameInputManager.Instance.PlayerAttackAction.WasPressedThisFrame())
        {
            _lastClickTime= Time.time;

            if (!_player.IsAttacking)
            {
                Attack();
            }
        }
    }
    private void Attack()
    {
        _player.IsAttacking = true;
        _player.Rb.linearVelocity = new Vector2(0, _player.Rb.linearVelocityY);

        

        int maxComboStep;
        if (_player.IsCrouching)
        {
            maxComboStep = _crouchComboStep;
        }
        else if (_player.IsGrounded)
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


        _player.PlayerAnimation.SetTriggerAttack();
        _player.PlayerAnimation.SetAttackCombo(ComboStep);
        
    }

    public void PerformHitDetection()
    {
        Vector3 center = new Vector3(transform.position.x + _attackOffset.x * _player.FacingDirection,
           transform.position.y + _attackOffset.y, transform.position.z);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(center, _attackRange, _player.EnemyLayerMask);

        foreach(Collider2D hit in hitEnemies)
        {
            if(hit.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(_player.Strength);
            }
        }

    }
    public void FinishAttack()
    {
        _player.IsAttacking = false; 
    }

    private void ResetCombo()
    {
        ComboStep = 0;
    }
}
