using System;
using System.Collections;

using UnityEngine;



public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform _upAttackPoint;
    [SerializeField] private Transform _plungeAttackPoint;
    [SerializeField] private Transform _sideAttackPoint;

    [SerializeField] private Vector2 _horizontalAttackSize ;
    [SerializeField] private Vector2 _upAttackSize ;
    [SerializeField] private Vector2 _plungeAttackSize;



    [SerializeField] private PlayerAttackSO _data;
    [SerializeField] private LayerMask _enemyMask;


    [Header("Sword Settting")]
   [SerializeField] private int _swordLevel=1;
    private float _attackSpeedMultiplier = 1f;
   

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
        if ((CanAttack()))
        {
            Attack();
        }


        if (PlayerMovement.Instance.IsPlunging && PlayerMovement.Instance.IsGrounded)
        {
            PlayerMovement.Instance.IsPlunging = false ;
            ScanAndDamage(_plungeAttackPoint.position, _plungeAttackSize);
        }
    }
    private bool CanAttack()
    {
        float attackCoolDown = _data.baseAttackCooldown / _attackSpeedMultiplier;

        return _lastPressAttackTimer>0f && !IsAttackCooldown && !_playerMovement.IsDashing;
    }
    private void Attack()
    {
        _lastPressAttackTimer = 0f;
        _attackTimer = _data.baseAttackCooldown;


        bool isUpPressed = GameInputManager.Instance.IsUpPressed();
        bool isDownPressed = GameInputManager.Instance.IsDownPressed();

        if (isUpPressed)
        {
           
            ScanAndDamage(_upAttackPoint.position, _upAttackSize);
            SpawnSlashVFX(Vector2.up);

        }
        else if (isDownPressed && !PlayerMovement.Instance.IsGrounded)
        {
            Debug.Log("Plunge Attack");
            StartPlungeAttack();
            
        }
        else
        {
        
            ScanAndDamage(_sideAttackPoint.position, _horizontalAttackSize);
            SpawnSlashVFX(Vector2.right);
            // horizontal attack


        }

       
       
       


    }

    private void StartPlungeAttack()
    {
        PlayerMovement.Instance.IsPlunging = true;
    }
 

    private bool ScanAndDamage(Vector3 position, Vector2 size)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(position, size, 0, _enemyMask);

        

       
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
                    });
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
        else if (direction == Vector2.right)
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
        Gizmos.DrawWireCube(_sideAttackPoint.position, _horizontalAttackSize);

        Gizmos.DrawWireCube(_upAttackPoint.position, _upAttackSize);
        Gizmos.DrawWireCube(_plungeAttackPoint.position, _plungeAttackSize);

    }
}
