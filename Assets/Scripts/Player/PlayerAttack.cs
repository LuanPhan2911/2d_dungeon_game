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



    [SerializeField] private PlayerAttackData _data;
    [SerializeField] private ElementData _defaultElement;
    [SerializeField] private LayerMask _enemyMask;


    [Header("Sword Settting")]
   [SerializeField] private int _swordLevel=1;
    private float _attackSpeedMultiplier = 1f;
  


    [Header("Element")]


    public ElementData[] Elements;
    public ElementData SelectedElement { get; private set; }
    public ElementData GetElementDamage()
    {
        if(SelectedElement==null || !IsInfusedElementToWeapon)
        {
            return _defaultElement;
        }
        return SelectedElement;
    }

    public void SetSelectedElement(ElementData element)
    {
        SelectedElement = element;
        _elementChangeTimer = _data.elementChangeCooldown;
        _infusedElementToWeaponTimer = 0f;
        OnElementChanged?.Invoke();
    }
    public event Action OnElementChanged;
    public bool IsElementChangeCooldown => _elementChangeTimer > 0f;
    private float _elementChangeTimer;




   

   
    public bool IsRecoiling { get; private set; }

    #region Normal Attack

    public bool IsAttackCooldown => _attackTimer > 0f;

    #endregion

    #region Elemental Skill
    private float _elementalSkillTimer;
    public bool IsElementalSkillCooldown => _elementalSkillTimer > 0f;
    public float ElementalSkillDuration =>  _elementalSkillTimer> 1f ? 
        Mathf.Round(_elementalSkillTimer):  Mathf.Floor(_elementalSkillTimer * 10f) / 10f;
    public float ElementalSKillRatio => _elementalSkillTimer / _data.baseElementalSkillCooldown;



 

    #endregion


    #region Burst Skill

    private float _burstSkillTimer;
    private float _infusedElementToWeaponTimer;
    public bool IsBurstSkillCooldown => _burstSkillTimer > 0f;
    public bool IsInfusedElementToWeapon=> _infusedElementToWeaponTimer>0f;

    private float _burstSkillPressTimer;
    private bool _isBurstSkillHolding;
    #endregion



    #region Timer
    private float _lastPressAttackTimer;
    private float _lastPressElementalSkillTimer;
    private float _lastPressBurstSkillTimer;

    #endregion

    private float _currentCritRate;
    private float _attackTimer;









    private PlayerAnimation _playerAnimation;

    private PlayerMovement _playerMovement;
    private PlayerHealth _playerHealth;

    [Header("FX")]
    [SerializeField] private GameObject _slashFxPrefab;


    public static PlayerAttack Instance { get; private set;  }


    private void Awake()
    {
        Instance = this;
        
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerHealth = GetComponent<PlayerHealth>();

        
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
        _elementalSkillTimer = Mathf.Max(_elementalSkillTimer - Time.deltaTime, 0f);
        _burstSkillTimer = Mathf.Max(_burstSkillTimer - Time.deltaTime, 0f);
        _infusedElementToWeaponTimer = Mathf.Max(_infusedElementToWeaponTimer - Time.deltaTime, 0f);
        _elementChangeTimer = Mathf.Max(_elementChangeTimer - Time.deltaTime, 0f);


        _lastPressAttackTimer = Mathf.Max(_lastPressAttackTimer - Time.deltaTime, 0f);
        _lastPressElementalSkillTimer = Mathf.Max(_lastPressElementalSkillTimer - Time.deltaTime, 0f);
        _lastPressBurstSkillTimer = Mathf.Max(_lastPressBurstSkillTimer - Time.deltaTime, 0f);


        #region Input 
        if ( GameInputManager.Instance.PlayerActions.Attack.WasPressedThisFrame() )
        {
            _lastPressAttackTimer = _data.attackInputBuffer;
        }
        if (GameInputManager.Instance.PlayerActions.ElementalSkill.WasPressedThisFrame())
        {
            _lastPressElementalSkillTimer = _data.attackInputBuffer;
        }

        if (GameInputManager.Instance.PlayerActions.BurstSkill.WasPressedThisFrame())
        {
            _lastPressBurstSkillTimer = _data.attackInputBuffer;
            _burstSkillPressTimer = 0f;
            _isBurstSkillHolding = false;
        }
        if (GameInputManager.Instance.PlayerActions.BurstSkill.IsPressed())
        {
            _burstSkillPressTimer += Time.deltaTime;
            if(_burstSkillPressTimer> _data.burstSkillPressedThreshhold && !_isBurstSkillHolding)
            {
                InfuseElementToWeapon();
            }
        }
       

        #endregion

        if (CanUseElementalSkill())
        {
            ElementalSkillAttack();
        }
        if ((CanNormalAttack()))
        {
            NormalAttack();
        }
        if (CanUseBurstSkill())
        {
            BurstSkillAttack();
        }
       
    }
  

    private bool CanNormalAttack()
    {
        float attackCoolDown = _data.baseAttackCooldown / _attackSpeedMultiplier;

        return _lastPressAttackTimer>0f && !IsAttackCooldown && !_playerMovement.IsDashing;
    }
    private bool CanUseElementalSkill()
    {
        return _lastPressElementalSkillTimer > 0f && !IsElementalSkillCooldown;
    }
    private bool CanUseBurstSkill()
    {
        return _lastPressBurstSkillTimer > 0f && _playerHealth.IsEnoughEnergyToUseBurstSkill() && !IsBurstSkillCooldown;
    }

    private void ElementalSkillAttack()
    {
        _elementalSkillTimer = _data.baseElementalSkillCooldown;
      
        Debug.Log("Elemental Skill");

        _playerHealth.GainEnergyFormElementalSKill();

    }
    private void BurstSkillAttack()
    {
        _burstSkillTimer = _data.baseBurstSkillCooldown;

        _playerHealth.UseEnergyForBurstSkill();
        Debug.Log("Burst Skill");
    }
    private void InfuseElementToWeapon()
    {
        _infusedElementToWeaponTimer = _data.infusedElementToWeaponDuration;
        _isBurstSkillHolding = true;
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

            _playerHealth.GainEnergyFromNormalAttack();

            #endregion

            // 2. Damge to enemy
            foreach (Collider2D hit in hitEnemies)
            {
                if(hit.TryGetComponent(out IDamageable damageable))
                {
                    
                   
                  
                    damageable.TakeDamage(new Damage
                    {
                        amount= damageAmount,
                        element= GetElementDamage(),
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

        ElementData elementDamage = GetElementDamage();
       
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
