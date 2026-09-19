using System.Collections;

using UnityEngine;



public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform _upAttackPoint;
    [SerializeField] private Transform _downAttackPoint;
    [SerializeField] private Transform _sideAttackPoint;

    [SerializeField] private Vector2 _horizontalAttackSize = new Vector2(1.5f, 1);
    [SerializeField] private Vector2 _verticalAttackSize = new Vector2(1, 1.5f);

    [SerializeField] private PlayerSlashFX _slashFX;


    [SerializeField] private PlayerWeaponData _data;
    [SerializeField] private LayerMask _enemyMask;




    private int _currentDamage;
    private int _currentSwordLevel=1;

    private float _currentAttackSpeedMultiplier = 1f;
    private float _attackTimer;








    public bool IsRecoiling { get; private set; }
    public float LastPressAttackTimer { get; private set;  }


  




    private PlayerAnimation _playerAnimation;

    private PlayerMovement _playerMovement;

    private void Awake()
    {
        
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerMovement = GetComponent<PlayerMovement>();

        
    }
    private void Start()
    {
        UpdateCurrentDamage();
    }
    private void UpdateCurrentDamage()
    {
        switch (_currentSwordLevel)
        {
            case 1:
                _currentDamage= _data.level1Damage;
                break;
            case 2:
                _currentDamage= _data.level2Damage;
                break;
            case 3:
                _currentDamage= _data.level3Damage;
                break;
            default:
                _currentDamage = 1;
                Debug.Log("Unknown Sword Level");
                break;


        }
    }

    private void Update()
    {

        _attackTimer += Time.deltaTime;
        LastPressAttackTimer -= Time.deltaTime;

        if ( GameInputManager.Instance.PlayerAttackAction.WasPressedThisFrame() )
        {
            LastPressAttackTimer = _data.attackInputBuffer;
        }


        if ((CanAttack()))
        {
            Attack();
        }
    }

    private bool CanAttack()
    {
        float attackCoolDown = _data.baseAttackCooldown / _currentAttackSpeedMultiplier;

        return LastPressAttackTimer>0&& _attackTimer > attackCoolDown && 
            !_playerMovement.IsRecoiling && !_playerMovement.IsDashing
            && !_playerMovement.IsWallSliding;
    }

   
    private void Attack()
    {
        LastPressAttackTimer = 0f;
        _attackTimer = 0;

        float verticalInput = GameInputManager.Instance.GetVerticalInput();

        Vector2 attackDirection;

        if (verticalInput > 0)
        {
            attackDirection = Vector2.up;
            ScanAndDamage(_upAttackPoint.position, _verticalAttackSize, attackDirection);
            _playerAnimation.SetTriggerUpwardAttack();
        }
        else if (verticalInput < 0 && _playerMovement.LastOnGroundTime <= 0)
        {
            attackDirection = Vector2.down;
            StartCoroutine(DownAttackClingerCoroutine(_downAttackPoint.position, _verticalAttackSize));

            _playerAnimation.SetTriggerDownwardAttack();
        }
        else
        {
            attackDirection = Vector2.right;
            ScanAndDamage(_sideAttackPoint.position, _horizontalAttackSize, attackDirection);
            // horizontal attack
            _playerAnimation.SetTriggeHorizontalAttack();
        
        }
        _slashFX.Show(attackDirection);
        StartCoroutine(EndSlash());


    }

    private bool ScanAndDamage(Vector3 position, Vector2 size, Vector2 direction)
    {
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
                    damageable.TakeDamage(_currentDamage, enemyRecoilDirection);
                }
            }

           
            ApplyRecoil(direction);
            return true;
           
        }
        return false;

      

    }
   
    private IEnumerator DownAttackClingerCoroutine(Vector3 position, Vector2 size)
    {
        float elapse = 0;
        while(elapse < _data.downAttackDuration)
        {
            elapse += Time.deltaTime;
            if(ScanAndDamage(position, size, Vector2.down))
            {
                yield break;
            }
            
        }
        yield return null;
    }
    private IEnumerator EndSlash()
    {
        yield return new WaitForSeconds(_data.attackDuration);
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
            _playerMovement.HandleStopJump();
        }
        else
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
