using System;
using System.Collections;

using UnityEngine;



public class PlayerAttack : MonoBehaviour
{

    public InputSystem_Actions.PlayerActions PlayerActions => GameInputManager.Instance.PlayerActions;
    public PlayerMovement PMovement => PlayerMovement.Instance;
    public PlayerElement PElement => PlayerElement.Instance;
    public PlayerHealth PHealth => PlayerHealth.Instance;

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
    private float _nextAttackTime;
    private float _currentPlungeAttackDamage;


    private float _chargedTimer;
    private bool _hasChargedAttack;
    private bool _isEnoughStaminaForChargeAttack = false;
    private bool _isChargedOnGround = false;



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

        if ( PlayerActions.Attack.WasPressedThisFrame() )
        {
            _lastPressAttackTimer = _data.attackInputBuffer;

            _chargedTimer = 0f;
            _hasChargedAttack = false;
            _isEnoughStaminaForChargeAttack= PHealth.CurrentStamina >= _data.chargeAttackStaminaCost;
            _isChargedOnGround = PMovement.IsGrounded;
        }

        if (PlayerActions.Attack.IsPressed()  
            && _isEnoughStaminaForChargeAttack && _isChargedOnGround
            && !GameInputManager.Instance.IsUpPressed()
            )
        {
            _chargedTimer += Time.deltaTime;
            if (!PMovement.IsGrounded)
            {
                _isChargedOnGround = false;
            }

            if (CanChargedAttack())
            {
                ExecuteChargeAttack();
                _nextAttackTime = Time.time + _data.baseAttackRate;
                _lastPressAttackTimer = 0f;
                _hasChargedAttack = true;
             

            }
        }

        if (PlayerActions.Attack.WasReleasedThisFrame())
        {
            _chargedTimer = 0f;
            _hasChargedAttack = false;
            _isEnoughStaminaForChargeAttack = false;
            _isChargedOnGround = false;
        }


        if ( CanNormalAttack())
        {
            ExecuteNormalAttack();
            _nextAttackTime = Time.time + _data.baseAttackRate;
            _lastPressAttackTimer = 0f;
        }




        if (PMovement.IsPlunging && PMovement.IsGrounded)
        {
            PMovement.IsPlunging = false ;
            ScanAndDamage(_plungeAttackPoint.position, _plungeAttackSize, _currentPlungeAttackDamage, DamageType.PlungeAttack);
        }
    }

    private bool CanChargedAttack()
    {
        return _chargedTimer >= _data.chargeAttackHoldTime && !_hasChargedAttack
            ;
    }

    private void ExecuteChargeAttack()
    {
        ScanAndDamage(_chargeAttackPoint.position, _chargeAttackSize, _data.baseChargeAttackDamage, DamageType.ChargeAttack);

        PHealth.ConsumeStamina(_data.chargeAttackStaminaCost);

        SpawnChargeAttackSlashVFX();
    }
    private bool CanNormalAttack()
    {
   

        return Time.time >= _nextAttackTime && _lastPressAttackTimer >0f &&  
            !PMovement.IsDashing &&! PMovement.IsPlunging;
    }
    private void ExecuteNormalAttack()
    {
        _lastPressAttackTimer = 0f;
    


        bool isUpPressed = GameInputManager.Instance.IsUpPressed();
        bool isDownPressed = GameInputManager.Instance.IsDownPressed();

        if (isUpPressed)
        {
           
            ScanAndDamage(_upAttackPoint.position, _upAttackSize, _data.baseNormalAttackDamage, DamageType.NormalAttack);
            SpawnNormalAttackSlashVFX(Vector2.up);

        }
        else if (isDownPressed && !PMovement.IsGrounded)
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
        PMovement.IsPlunging = true;

        float fallHeight = PMovement.GetFallHeight();

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
                element = PElement.GetElementTypeDamage(),
                type = damageType,
                isCrit = isCritStrike
            };

       
            #region Energy

            PHealth.GainEnergyFromAttack();

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


      
        bool isFacingRight = PMovement.IsFacingRight;

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
       

        ElementSO elementDamage = PElement.GetElementTypeDamage();
       
        slashFx.GetComponent<SlashFX>().SetColor(elementDamage.color);
        
       

    }
    private void SpawnChargeAttackSlashVFX()
    {
        bool isFacingRight = PMovement.IsFacingRight;
        GameObject slashFx = Instantiate(_chargeSlashFxPrefab, _chargeAttackPoint.position, Quaternion.identity);


        Vector3 scale= slashFx.transform.localScale;
        slashFx.transform.localScale = new Vector3(isFacingRight ? scale.x : -scale.x, scale.y, scale.z);

        ElementSO elementDamage = PElement.GetElementTypeDamage();
       
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
