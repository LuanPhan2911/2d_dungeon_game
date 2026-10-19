

using UnityEngine;



public class PlayerAttack : MonoBehaviour
{

    public InputSystem_Actions.PlayerActions PlayerActions => GameInputManager.Instance.PlayerActions;
    public PlayerMovement PMovement => PlayerMovement.Instance;
    public PlayerElement PElement => PlayerElement.Instance;
    public PlayerHealth PHealth => PlayerHealth.Instance;


    [SerializeField] private Transform _plungeAttackPoint;
    [SerializeField] private Transform _sideAttackPoint;
    [SerializeField] private Transform _chargeAttackPoint;

    [SerializeField] private Vector2 _horizontalAttackSize ;

    [SerializeField] private Vector2 _plungeAttackSize;

    [SerializeField] private Vector2 _chargeAttackSize;



    [SerializeField] private PlayerAttackSO _dataSO;
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
    private bool _isCharging;
    private bool _isFullyCharged;



    [Header("FX")]
    [SerializeField] private GameObject _slashFxPrefab;
    [SerializeField] private GameObject _chargeSlashFxPrefab;
    [SerializeField] private ChargeAttackFX _chargeAttackFx;



    public static PlayerAttack Instance { get; private set;  }

    private BuffEffectManager _buffEffectManager;

    private void Awake()
    {
        Instance = this;
        _buffEffectManager = GetComponent<BuffEffectManager>();
        
     
    }
    private void Start()
    {
        _currentCritRate = _dataSO.baseCritRate;
    }

   


  
    private void Update()
    {


        
        _lastPressAttackTimer = Mathf.Max(_lastPressAttackTimer - Time.deltaTime, 0f);

        if ( PlayerActions.Attack.WasPressedThisFrame() )
        {
            _lastPressAttackTimer = _dataSO.attackInputBuffer;
            _chargedTimer = 0f;
            _isCharging = true;
            _isFullyCharged = false;   
        }

        if (_isCharging && PlayerActions.Attack.IsPressed())
        {
            _chargedTimer += Time.deltaTime;
            if (_chargedTimer >= _dataSO.chargeAttackThreshold)
            {
                _chargeAttackFx.Play();
            }

            if (_chargedTimer >= _dataSO.chargeAttackTime && !_isFullyCharged)
            {
                _isFullyCharged = true;
               
                OnFullyCharged();
            }
        }

        if (_isCharging&& PlayerActions.Attack.WasReleasedThisFrame())
        {
            if (_isFullyCharged)
            {
                ExecuteChargeAttack();
                _nextAttackTime = Time.time + GetAttackRate();
                _lastPressAttackTimer = 0f;
            }
            _chargeAttackFx.Stop();
        }


        if ( CanNormalAttack())
        {
            ExecuteNormalAttack();
            _nextAttackTime = Time.time + GetAttackRate();
            _isCharging = false;
            _lastPressAttackTimer = 0f;
        }




        if (PMovement.IsPlunging && PMovement.IsGrounded)
        {
            PMovement.IsPlunging = false ;
            ScanAndDamage(_plungeAttackPoint.position, _plungeAttackSize, _currentPlungeAttackDamage);
        }
    }


    private void OnFullyCharged()
    {
        
        Debug.Log("On Full Charged");
    }

    private void ExecuteChargeAttack()
    {
        ScanAndDamage(_chargeAttackPoint.position, _chargeAttackSize, _dataSO.baseChargeAttackDamage);

        SpawnChargeAttackSlashVFX();
    }
    private bool CanNormalAttack()
    {
        return Time.time >= _nextAttackTime && _lastPressAttackTimer >0f &&  !PlayerActions.Attack.IsPressed()
            &&!PMovement.IsDashing &&! PMovement.IsPlunging;
    }
    private void ExecuteNormalAttack()
    {
        _lastPressAttackTimer = 0f;
    


     
        bool isDownPressed = GameInputManager.Instance.IsDownPressed();

       
         if (isDownPressed && !PMovement.IsGrounded)
        {
            Debug.Log("Plunge Attack");
            StartPlungeAttack();
            
        }
        else
        {
        
            ScanAndDamage(_sideAttackPoint.position, _horizontalAttackSize, _dataSO.baseNormalAttackDamage);
            SpawnNormalAttackSlashVFX();
            // horizontal attack


        }

       
       
       


    }

    private void StartPlungeAttack()
    {
        PMovement.IsPlunging = true;
        float fallHeight = PMovement.GetFallHeight();

        if (fallHeight >= _dataSO.hightHeightThreshold)
        {
            _currentPlungeAttackDamage = _dataSO.highPlungeAttackDamage;
        }
        else
        {
            _currentPlungeAttackDamage = _dataSO.lowPlungeAttackDamage;
        }

    }

    public bool IsCritStrike()
    {
        bool isCritStrike = UnityEngine.Random.value <= _currentCritRate;
        if (isCritStrike)
        {
            _currentCritRate = _dataSO.baseCritRate;

        }
        else
        {
            _currentCritRate = Mathf.Clamp(_currentCritRate + _dataSO.critRateIncreasement, 0, 1);
        }
        return isCritStrike;
    }


    public void ScanAndDamage(Vector3 position, Vector2 size, float damageAmount)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(position, size, 0, _enemyMask);
        if (hitEnemies.Length > 0)
        {
            Damage damage = new Damage
            {
                Amount = damageAmount,
                ElementSO = PElement.GetElementInfusedToWeapon(),
                SourceAttackGameObject= gameObject

            };

            #region Energy

            #endregion
           

            // 2. Damge to enemy
            foreach (Collider2D hit in hitEnemies)
            {
                if (hit.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damage);
                   
                }
            }
            

        }
      

    }
       
    public float GetCritDamageMultiplier()
    {
        return 1 + _dataSO.baseCritDamage;
    }
    private void SpawnNormalAttackSlashVFX( )
    {
        bool isFacingRight = PMovement.IsFacingRight;

        GameObject slashFx = Instantiate(_slashFxPrefab );

        slashFx.transform.position = _sideAttackPoint.position;
        slashFx.transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);


        ElementSO elementDamage = PElement.GetElementInfusedToWeapon();
       
        slashFx.GetComponent<SlashFX>().SetColor(elementDamage.color);
        
       

    }
    private void SpawnChargeAttackSlashVFX()
    {
        bool isFacingRight = PMovement.IsFacingRight;
        GameObject slashFx = Instantiate(_chargeSlashFxPrefab, _chargeAttackPoint.position, Quaternion.identity);


        Vector3 scale= slashFx.transform.localScale;
        slashFx.transform.localScale = new Vector3(isFacingRight ? scale.x : -scale.x, scale.y, scale.z);

        ElementSO elementDamage = PElement.GetElementInfusedToWeapon();
       
        slashFx.GetComponent<SlashFX>().SetColor(elementDamage.color);
    }

    private float GetAttackSpeedMultiplier()
    {
        float multiplier = 1f;
        if (_buffEffectManager.HasEffect(BuffEffectType.AttackSpeedBonus))
        {
            multiplier = (1 - _buffEffectManager.GetValues(BuffEffectType.AttackSpeedBonus));
        }
        return multiplier;
    }
    private float GetAttackRate()
    {
        float attackRate = _dataSO.baseAttackRate * GetAttackSpeedMultiplier();
        return attackRate;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_sideAttackPoint.position, _horizontalAttackSize);

       

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_plungeAttackPoint.position, _plungeAttackSize);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(_chargeAttackPoint.position, _chargeAttackSize);

    }
}
