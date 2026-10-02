using System;
using System.Collections;

using UnityEngine;



public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform _upAttackPoint;
    [SerializeField] private Transform _plungeAttackPoint;
    [SerializeField] private Transform _sideAttackPoint;
    [SerializeField] private Transform _chargeAttackPoint;

    [SerializeField] private Vector2 _horizontalAttackSize ;
    [SerializeField] private Vector2 _upAttackSize ;
    [SerializeField] private Vector2 _plungeAttackSize;

    [SerializeField] private Vector2 _chargeAttackSize;



    [SerializeField] private PlayerAttackSO _data;
    [SerializeField] private LayerMask _enemyMask;


    [Header("Sword Settting")]


   

    #region Normal Attack


    #endregion


    #region Timer
    private float _lastPressAttackTimer;

    #endregion

    private float _currentCritRate;
    private float _attackTimer;
    private float _nextAttackTime;
    private float _currentPlungeAttackDamage;


    private bool _isCharging;
    private float _chargeTimer;

    private bool _isFullyCharged;


    [Header("FX")]
    [SerializeField] private GameObject _slashFxPrefab;
    [SerializeField] private GameObject _chargeSlashFxPrefab;
  


    public static PlayerAttack Instance { get; private set;  }


    private void Awake()
    {
        Instance = this;
        
     
    }
    private void Start()
    {
        _currentCritRate = _data.baseCritRate;
    }

   


  
    private void Update()
    {


        _lastPressAttackTimer = Mathf.Max(_lastPressAttackTimer - Time.deltaTime, 0f);

        if ( GameInputManager.Instance.PlayerActions.Attack.WasPressedThisFrame() )
        {
            _lastPressAttackTimer = _data.attackInputBuffer;

            _isCharging = true;
            _chargeTimer = 0f;
            _isFullyCharged = false;
        }

        if (GameInputManager.Instance.PlayerActions.Attack.IsPressed() && _isCharging)
        {
            _chargeTimer+=Time.deltaTime;
            if(_chargeTimer>= _data.chargeAttackHoldTime && !_isFullyCharged)
            {
                _isFullyCharged = true;
                Debug.Log("Fully Charged");
            }
        }
        if(GameInputManager.Instance.PlayerActions.Attack.WasReleasedThisFrame()&& _isCharging  )
        {
            _isCharging = false;
            if (_isFullyCharged)
            {
                ExecuteChargeAttack();
                _nextAttackTime= Time.time + _data.baseAttackRate;
                _lastPressAttackTimer = 0f;


            }
           
        }

        if(Time.time >= _nextAttackTime && CanNormalAttack())
        {
            ExecuteNormalAttack();
            _nextAttackTime = Time.time + _data.baseAttackRate;
            _lastPressAttackTimer = 0f;
            _isCharging = false;
        }




        if (PlayerMovement.Instance.IsPlunging && PlayerMovement.Instance.IsGrounded)
        {
            PlayerMovement.Instance.IsPlunging = false ;
            ScanAndDamage(_plungeAttackPoint.position, _plungeAttackSize, _currentPlungeAttackDamage, DamageType.PlungeAttack);
        }
    }

    private void ExecuteChargeAttack()
    {
        ScanAndDamage(_chargeAttackPoint.position, _chargeAttackSize, _data.baseChargeAttackDamage, DamageType.ChargeAttack);


        SpawnChargeAttackSlashVFX();
    }
    private bool CanNormalAttack()
    {
   

        return _lastPressAttackTimer>0f &&  !GameInputManager.Instance.PlayerActions.Attack.IsPressed() &&
            !PlayerMovement.Instance.IsDashing &&! PlayerMovement.Instance.IsPlunging;
    }
    private void ExecuteNormalAttack()
    {
        _lastPressAttackTimer = 0f;
        _attackTimer = _data.baseAttackRate;


        bool isUpPressed = GameInputManager.Instance.IsUpPressed();
        bool isDownPressed = GameInputManager.Instance.IsDownPressed();

        if (isUpPressed)
        {
           
            ScanAndDamage(_upAttackPoint.position, _upAttackSize, _data.baseNormalAttackDamage, DamageType.NormalAttack);
            SpawnNormalAttackSlashVFX(Vector2.up);

        }
        else if (isDownPressed && !PlayerMovement.Instance.IsGrounded)
        {
            Debug.Log("Plunge Attack");
            StartPlungeAttack();
            
        }
        else
        {
        
            ScanAndDamage(_sideAttackPoint.position, _horizontalAttackSize, _data.baseNormalAttackDamage, DamageType.NormalAttack);
            SpawnNormalAttackSlashVFX(Vector2.right);
            // horizontal attack


        }

       
       
       


    }

    private void StartPlungeAttack()
    {
        PlayerMovement.Instance.IsPlunging = true;

        float fallHeight = PlayerMovement.Instance.GetFallHeight();

        if(fallHeight >= _data.highHeightThreshold)
        {
            _currentPlungeAttackDamage = _data.baseHighPlungeAttackDamage;
        }
        else
        {
            _currentPlungeAttackDamage = _data.baseLowPlungeAttackDamage;
        }
    }
 

    private bool ScanAndDamage(Vector3 position, Vector2 size, float damageAmount, DamageType damageType)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(position, size, 0, _enemyMask);

        

       
        if (hitEnemies.Length > 0)
        {


            #region Crit rate
            bool isCritStrike = UnityEngine.Random.value <= _currentCritRate;


            if (isCritStrike)
            {
                damageAmount *= GetCritDamageMultiplier();
                _currentCritRate = _data.baseCritRate;

            }
            else
            {
                _currentCritRate = Mathf.Clamp(_currentCritRate + _data.critRateIncreasement, 0, 1);

            }


            #endregion

            Damage damage = new Damage
            {
                amount = damageAmount,
                element = PlayerElement.Instance.GetElementTypeDamage(),
                type = damageType,
                isCrit = isCritStrike
            };

       
            #region Energy

            PlayerHealth.Instance.GainEnergyFromAttack();

            #endregion

            // 2. Damge to enemy
            foreach (Collider2D hit in hitEnemies)
            {
                if(hit.TryGetComponent(out IDamageable damageable))
                {
                    
                   
                  
                    damageable.TakeDamage(damage);
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
    private void SpawnNormalAttackSlashVFX(Vector2 direction )
    {


      
        bool isFacingRight = PlayerMovement.Instance.IsFacingRight;

        GameObject slashFx = Instantiate(_slashFxPrefab );
     
        if (direction == Vector2.up)
        {
        
            slashFx.transform.position= _upAttackPoint.position;
            slashFx.transform.rotation = Quaternion.Euler(0, 0, 90f);
            slashFx.transform.localScale = new Vector3(1, isFacingRight ? 1 : -1, 1);

        }
        else if (direction == Vector2.right)
        {
            slashFx.transform.position = _sideAttackPoint.position;
            slashFx.transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);



        }
       

        ElementSO elementDamage = PlayerElement.Instance.GetElementTypeDamage();
       
        slashFx.GetComponent<SlashFX>().SetColor(elementDamage.color);
        
       

    }
    private void SpawnChargeAttackSlashVFX()
    {
        bool isFacingRight = PlayerMovement.Instance.IsFacingRight;
        GameObject slashFx = Instantiate(_chargeSlashFxPrefab, _chargeAttackPoint.position, Quaternion.identity);


        Vector3 scale= slashFx.transform.localScale;
        slashFx.transform.localScale = new Vector3(isFacingRight ? scale.x : -scale.x, scale.y, scale.z);

        ElementSO elementDamage = PlayerElement.Instance.GetElementTypeDamage();
       
        slashFx.GetComponent<SlashFX>().SetColor(elementDamage.color);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_sideAttackPoint.position, _horizontalAttackSize);

        Gizmos.DrawWireCube(_upAttackPoint.position, _upAttackSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_plungeAttackPoint.position, _plungeAttackSize);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(_chargeAttackPoint.position, _chargeAttackSize);

    }
}
