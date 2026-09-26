using System.Collections;

using UnityEngine;



public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform _upAttackPoint;
    [SerializeField] private Transform _downAttackPoint;
    [SerializeField] private Transform _sideAttackPoint;

    [SerializeField] private Vector2 _horizontalAttackSize = new Vector2(1.5f, 1);
    [SerializeField] private Vector2 _verticalAttackSize = new Vector2(1, 1.5f);



    [SerializeField] private PlayerWeaponData _data;
    [SerializeField] private LayerMask _enemyMask;


    [Header("Sword Settting")]
   [SerializeField] private int _swordLevel=1;
    private float _attackSpeedMultiplier = 1f;
    private float _attackTimer;


    [Header("Element Type")]
   [SerializeField] private int _elementIndex = 0;



    private float _currentCritRate;
    private float _currentCritDamage;

   
    public bool IsRecoiling { get; private set; }
    public float LastPressAttackTimer { get; private set;  }


    private PlayerAnimation _playerAnimation;

    private PlayerMovement _playerMovement;

    [Header("FX")]
    [SerializeField] private GameObject _slashFxPrefab;


    private void Awake()
    {
        
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerMovement = GetComponent<PlayerMovement>();

        
    }
    private void Start()
    {
        _currentCritRate = _data.baseCritRate;
        _currentCritDamage = _data.baseCritDamage;
    }

    public ElementData GetElement(int elementIndex)
    {
        if (elementIndex >= _data.elements.Length || elementIndex < 0) return null;
        return _data.elements[elementIndex];
    }
    private int GetSwordDamage(int swordLevel)
    {
        switch (swordLevel)
        {
            case 1:
               return _data.level1Damage;
                
            case 2:
                return _data.level2Damage;
               
            case 3:
                return _data.level3Damage;
               
            default:
                Debug.Log("Unknown Sword Level");
                return 1;
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
        float attackCoolDown = _data.baseAttackCooldown / _attackSpeedMultiplier;

        return LastPressAttackTimer>0&& _attackTimer > attackCoolDown && 
            !_playerMovement.IsRecoiling && !_playerMovement.IsDashing;
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
        SpawnSlashVFX(attackDirection);
       


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
           

            #region Crit rate
            bool isCritStrike = Random.value <= _currentCritRate;

            float damageAmount = GetSwordDamage(_swordLevel);
            if (isCritStrike)
            {
                damageAmount *= GetCritDamageMultiplier();
                _currentCritRate = _data.baseCritRate;
               
            }
            else
            {
                _currentCritRate =Mathf.Clamp(_currentCritRate+ _data.critRateIncreasement, 0, 1);
               
            }


            #endregion

            // 2. Damge to enemy
            foreach (Collider2D hit in hitEnemies)
            {
                if(hit.TryGetComponent(out IDamageable damageable))
                {
                    
                    ElementData element = GetElement(_elementIndex);
                  
                    damageable.TakeDamage(new Damage
                    {
                        amount= damageAmount,
                        element= element,
                        isCrit=isCritStrike
                    }, enemyRecoilDirection);
                }
            }

           
          
            return true;
           
        }
        return false;

      

    }
    private float GetCritDamageMultiplier()
    {
        return 1 + _data.baseCritDamage;
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

    private void SpawnSlashVFX(Vector2 direction )
    {

        Transform parent = null;
        bool isFacingRight = _playerMovement.IsFacingRight;
        Quaternion rotation = Quaternion.identity;
        if (direction == Vector2.up)
        {
            rotation = Quaternion.Euler(0, 0, isFacingRight ? 90 : -90);
            parent = _upAttackPoint;
        }
        else if (direction == Vector2.down)
        {
            parent = _downAttackPoint;
            rotation = Quaternion.Euler(0, 0, isFacingRight ? -90 : 90);

        }
        else
        {
            parent = _sideAttackPoint;
           
        }

       

        GameObject slashFx = Instantiate(_slashFxPrefab, parent);

        ElementData element = GetElement(_elementIndex);
        if (element != null)
        {
            slashFx.GetComponent<SlashFX>().SetColor(element.color);
        }
        slashFx.transform.rotation = rotation;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(_upAttackPoint.position, _verticalAttackSize);
        Gizmos.DrawWireCube(_downAttackPoint.position, _verticalAttackSize);
        Gizmos.DrawWireCube(_sideAttackPoint.position, _horizontalAttackSize);
    }
}
