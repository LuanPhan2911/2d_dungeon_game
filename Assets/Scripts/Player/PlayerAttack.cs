using System;
using System.Collections;

using UnityEngine;



public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform _upAttackPoint;
    [SerializeField] private Transform _downAttackPoint;
    [SerializeField] private Transform _sideAttackPoint;

    [SerializeField] private Vector2 _horizontalAttackSize = new Vector2(1.5f, 1);
    [SerializeField] private Vector2 _verticalAttackSize = new Vector2(1, 1.5f);



    [SerializeField] private PlayerAttackSO _data;
    [SerializeField] private LayerMask _enemyMask;


    [Header("Sword Settting")]
   [SerializeField] private int _swordLevel=1;
    private float _attackSpeedMultiplier = 1f;
   
    public bool IsRecoiling { get; private set; }

    #region Normal Attack

    public bool IsAttackCooldown => _attackTimer > 0f;

    #endregion


    #region Timer
    private float _lastPressAttackTimer;

    #endregion

    private float _currentCritRate;
    private float _attackTimer;



    private PlayerMovement _playerMovement;


    [Header("FX")]
    [SerializeField] private GameObject _slashFxPrefab;


    public static PlayerAttack Instance { get; private set;  }


    private void Awake()
    {
        Instance = this;
        
   
        _playerMovement = GetComponent<PlayerMovement>();     
    }
    private void Start()
    {
        _currentCritRate = _data.baseCritRate;
    }

    private int GetDamage(int swordLevel)
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

        _attackTimer = Mathf.Max(_attackTimer - Time.deltaTime, 0f);
        _lastPressAttackTimer = Mathf.Max(_lastPressAttackTimer - Time.deltaTime, 0f);

        if ( GameInputManager.Instance.PlayerActions.Attack.WasPressedThisFrame() )
        {
            _lastPressAttackTimer = _data.attackInputBuffer;
        }
        if ((CanNormalAttack()))
        {
            NormalAttack();
        } 
    }
    private bool CanNormalAttack()
    {
        float attackCoolDown = _data.baseAttackCooldown / _attackSpeedMultiplier;

        return _lastPressAttackTimer>0f && !IsAttackCooldown && !_playerMovement.IsDashing;
    }
    private void NormalAttack()
    {
        _lastPressAttackTimer = 0f;
        _attackTimer = _data.baseAttackCooldown;

        float verticalInput = GameInputManager.Instance.GetVerticalInput();

        Vector2 attackDirection;

        if (verticalInput > 0)
        {
            attackDirection = Vector2.up;
            ScanAndDamage(_upAttackPoint.position, _verticalAttackSize, attackDirection);
         
        }
        else if (verticalInput < 0 && _playerMovement.LastOnGroundTime <= 0)
        {
            attackDirection = Vector2.down;
            // TODO: Plunge Attack
            
        }
        else
        {
            attackDirection = Vector2.right;
            ScanAndDamage(_sideAttackPoint.position, _horizontalAttackSize, attackDirection);
            // horizontal attack
            
        
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
            bool isCritStrike = UnityEngine.Random.value <= _currentCritRate;

            float damageAmount = GetDamage(_swordLevel);
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

            #region Energy

            PlayerHealth.Instance.GainEnergyFromNormalAttack();

            #endregion

            // 2. Damge to enemy
            foreach (Collider2D hit in hitEnemies)
            {
                if(hit.TryGetComponent(out IDamageable damageable))
                {
                    
                   
                  
                    damageable.TakeDamage(new Damage
                    {
                        amount= damageAmount,
                        element= PlayerElement.Instance.GetElementTypeDamage(),
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

        ElementSO elementDamage = PlayerElement.Instance.GetElementTypeDamage();
       
        slashFx.GetComponent<SlashFX>().SetColor(elementDamage.color);
        
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
