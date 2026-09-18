using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{



    [SerializeField] private float _attackSpeed = 2f;


    [SerializeField] private Transform _upAttackPoint;
    [SerializeField] private Transform _downAttackPoint;
    [SerializeField] private Transform _sideAttackPoint;

    [SerializeField] private Vector2 _horizontalAttackSize = new Vector2(1.5f, 1);
    [SerializeField] private Vector2 _verticalAttackSize = new Vector2(1, 1.5f);

    [SerializeField] private PlayerSlashFX _slashFX;



    [SerializeField] private float _attackCooldown = 0.3f;
    [SerializeField] private float _attackDuration = 0.1f;

    [SerializeField] private LayerMask _enemyMask;







    public bool IsRecoiling { get; private set; }


    private float _attackTimer;




    private PlayerAnimation _playerAnimation;

    private PlayerMovement _playerMovement;

    private void Awake()
    {
        
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerMovement = GetComponent<PlayerMovement>();

        
    }

    private void Update()
    {

        _attackTimer += Time.deltaTime;
        if (CanAttack() && GameInputManager.Instance.PlayerAttackAction.WasPressedThisFrame() )
        {
            Debug.Log("Attack");

            Attack();
        }
    }

    private bool CanAttack()
    {
        return _attackTimer > _attackCooldown && !_playerMovement.IsRecoiling && !_playerMovement.IsDashing
            && !_playerMovement.IsWallSliding;
    }

   
    private void Attack()
    {
        _attackTimer = 0;

        float verticalInput = GameInputManager.Instance.GetVerticalInput();
        if (verticalInput > 0)
        {
            PerformAttack(_upAttackPoint.position, _verticalAttackSize, Vector2.up);
            _playerAnimation.SetTriggerUpwardAttack();
        }
        else if (verticalInput < 0 && _playerMovement.LastOnGroundTime <= 0)
        {
            PerformAttack(_downAttackPoint.position, _verticalAttackSize, Vector2.down);

            _playerAnimation.SetTriggerDownwardAttack();
        }
        else
        {
            PerformAttack(_sideAttackPoint.position, _horizontalAttackSize, Vector2.right);
            // horizontal attack
            _playerAnimation.SetTriggeHorizontalAttack();
        
        }
       
        
    }

    private void PerformAttack(Vector3 position, Vector2 size, Vector2 direction)
    {
        // 1. Show Animation

        _slashFX.Show(direction);


        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(position, size, 0, _enemyMask);

        Vector2 enemyRecoilDirection = direction;
        if(direction== Vector2.right)
        {
            enemyRecoilDirection = _playerMovement.IsFacingRight ? Vector2.right : Vector2.left;
        }
        if (hitEnemies.Length > 0)
        {
            // 2. Damge to enemy
            foreach(Collider2D hit in hitEnemies)
            {
                if(hit.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(1, enemyRecoilDirection);
                }
            }

           
            ApplyRecoil(direction);
           
        }

        StartCoroutine(EndSlash());


    }

    private IEnumerator EndSlash()
    {
        yield return new WaitForSeconds(_attackDuration);
        _slashFX.Hide();
    }

    private void ApplyRecoil(Vector2 direction)
    {
        if (direction == Vector2.down)
        {
            // pogo

            _playerMovement.HandlePogo();
        }else if(direction == Vector2.up)
        {
            // stop move up
        }else
        {
            // recoil
            Vector2 recoilDirection = _playerMovement.IsFacingRight ? Vector2.left : Vector2.right;
            StartCoroutine(_playerMovement.StartRecoil(recoilDirection));
        }

    }
   
  

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(_upAttackPoint.position, _verticalAttackSize);
        Gizmos.DrawWireCube(_downAttackPoint.position, _verticalAttackSize);
        Gizmos.DrawWireCube(_sideAttackPoint.position, _horizontalAttackSize);
    }
}
