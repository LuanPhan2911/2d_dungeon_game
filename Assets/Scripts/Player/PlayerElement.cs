using System;
using UnityEngine;

public class PlayerElement : MonoBehaviour
{

    public static PlayerElement Instance { get; private set; }


    public PlayerAttack PAttack=> PlayerAttack.Instance;

    const int NONE_PLAYER_ELEMENT_INDEX = -1;
    const int FIRST_PLAYER_ELEMENT_INDEX = 0;
    const int SECOND_PLAYER_ELEMENT_INDEX = 1;
    const int THURD_PLAYER_ELEMENT_INDEX = 2;

    [Header("Element")]
    [SerializeField] private float _elementSwapCooldown = 0.5f;
    [SerializeField] private float _elementalSkillCooldown = 5f;
    [SerializeField] private float _elementalSkillDamage = 20f;

    [SerializeField] private float _infusedElementToWeaponDuration = 8.5f;
    [SerializeField] private float _burstSkillPressedThreshhold = 0.5f;
    [SerializeField] private ElementSO _physicElementType;


    [Header("Elemental Skill")]
    [SerializeField] private Vector2 _elementalSkillSize;
    [SerializeField] private Transform _elementalSkillPoint;

    [Serializable]
    public class PlayerElementData
    {
        public PlayerElementSO PlayerElementSO;
       [HideInInspector] public float burstSkillTimer;
    }

    public PlayerElementData[] ElementArray;


    public event Action OnElementArrayChanged;

    #region Active
    public int ActiveIndex => _activeIndex;
    
    public bool HasActive => _activeIndex != NONE_PLAYER_ELEMENT_INDEX;
    public PlayerElementData ActiveElement => ElementArray[_activeIndex];

    public ElementSO ActiveElementType => ActiveElement.PlayerElementSO.element;
    public PlayerElementSO ActiveElementSO => ActiveElement.PlayerElementSO;
    private float ActiveBurstSkillTimer => ActiveElement.burstSkillTimer;

    #endregion

    public event Action OnElementSwapped;

    public bool IsElementSwapCooldown => _elementSwapTimer > 0f;
    private float _elementSwapTimer;
    private int _activeIndex = NONE_PLAYER_ELEMENT_INDEX;



    #region Elemental Skill
    private float _elementalSkillTimer;
    public bool IsElementalSkillCooldown => _elementalSkillTimer > 0f;
    public float ElementalSkillDuration => _elementalSkillTimer > 1f ?
        Mathf.Round(_elementalSkillTimer) : _elementalSkillTimer.OneDecimal();
    public float ElementalSkillRatio => _elementalSkillTimer / _elementalSkillCooldown;

    #endregion


    #region Burst Skill

 

    private float _infusedElementToWeaponTimer;
    public bool IsBurstSkillCooldown => ActiveBurstSkillTimer > 0f;
    public float BurstSkillDuration => ActiveBurstSkillTimer > 1f ?
                Mathf.Round(ActiveBurstSkillTimer) : ActiveBurstSkillTimer.OneDecimal();
    public float BurstSkillRatio => ActiveBurstSkillTimer / ActiveElementSO.burstCooldown;
    public bool IsInfusedElementToWeapon => _infusedElementToWeaponTimer > 0f;

    private float _burstSkillPressTimer;
    private bool _isBurstSkillHolding;
    #endregion


    private float _lastPressElementalSkillTimer;
    private float _lastPressBurstSkillTimer;




    private void Awake()
    {
        Instance = this;
       

    }
    private void Start()
    {
      
     
    }
    private void Update()
    {
        _elementSwapTimer = Mathf.Max(0, _elementSwapTimer - Time.deltaTime);
        _elementalSkillTimer = Mathf.Max(_elementalSkillTimer - Time.deltaTime, 0f);
        _infusedElementToWeaponTimer = Mathf.Max(_infusedElementToWeaponTimer - Time.deltaTime, 0f);

        if (!IsElementSwapCooldown)
        {
            if (GameInputManager.Instance.PlayerActions.SwapElement1.WasPressedThisFrame())
            {
                SetActivePlayerElement(FIRST_PLAYER_ELEMENT_INDEX);
            }
            if (GameInputManager.Instance.PlayerActions.SwapElement2.WasPressedThisFrame())
            {
                SetActivePlayerElement(SECOND_PLAYER_ELEMENT_INDEX);
            }
            if (GameInputManager.Instance.PlayerActions.SwapElement3.WasPressedThisFrame())
            {
                SetActivePlayerElement(THURD_PLAYER_ELEMENT_INDEX);
            }
        }



        if (HasActive)
        {
            for (int i = 0; i < ElementArray.Length; i++)
            {
                float timer = ElementArray[i].burstSkillTimer;
                ElementArray[i].burstSkillTimer =Mathf.Max(timer - Time.deltaTime, 0f);
            }


            _lastPressElementalSkillTimer = Mathf.Max(_lastPressElementalSkillTimer - Time.deltaTime, 0f);
            _lastPressBurstSkillTimer = Mathf.Max(_lastPressBurstSkillTimer - Time.deltaTime, 0f);



            if (GameInputManager.Instance.PlayerActions.ElementalSkill.WasPressedThisFrame())
            {
                _lastPressElementalSkillTimer = ActiveElementSO.skillInputBuffer;
            }

            if (GameInputManager.Instance.PlayerActions.BurstSkill.WasPressedThisFrame())
            {
                _lastPressBurstSkillTimer = ActiveElementSO.skillInputBuffer;
                _burstSkillPressTimer = 0f;
                _isBurstSkillHolding = false;
            }
            if (GameInputManager.Instance.PlayerActions.BurstSkill.IsPressed())
            {
                _burstSkillPressTimer += Time.deltaTime;
                if (_burstSkillPressTimer > _burstSkillPressedThreshhold && !_isBurstSkillHolding)
                {
                    InfuseElementToWeapon();
                }
            }

            if (CanUseElementalSkill())
            {
                ElementalSkillAttack();
            }
            if (CanUseBurstSkill())
            {
                BurstSkillAttack();
            }
        }

      

      

    }
    public ElementSO GetElementTypeDamage()
    {
        
    
        if (!HasActive|| !IsInfusedElementToWeapon)
        {
            return _physicElementType;
        }
       
        return ActiveElementType;
    }
    public void SetActivePlayerElement(int index)
    {
        if (index < 0 || index >= ElementArray.Length) return;

        if (index == _activeIndex) return;


        _activeIndex = index;
        _elementSwapTimer = _elementSwapCooldown;
        StopInfuseElementToWeapon();
        OnElementSwapped?.Invoke();

    }

    public void StopInfuseElementToWeapon()
    {
        _infusedElementToWeaponTimer = 0f;
    }
    private bool CanUseElementalSkill()
    {
        return _lastPressElementalSkillTimer > 0f && !IsElementalSkillCooldown;
    }
    private bool CanUseBurstSkill()
    {
        return _lastPressBurstSkillTimer > 0f && PlayerHealth.Instance.CurrentEnergy >= ActiveElementSO.burstEnergy &&
            !IsBurstSkillCooldown;
    }

    private void ElementalSkillAttack()
    {
        _elementalSkillTimer = _elementalSkillCooldown;
        Debug.Log("Elemental Skill");
        if (PAttack.ScanAndDamage(_elementalSkillPoint.position, _elementalSkillSize, _elementalSkillDamage)){
            PlayerHealth.Instance.GainEnergyFormElementalSKill();
        }
       

    }
    private void BurstSkillAttack()
    {

        ElementArray[ActiveIndex].burstSkillTimer = ActiveElementSO.burstCooldown;
        PlayerHealth.Instance.ConsumeEnergy(ActiveElementSO.burstEnergy);
        Debug.Log("Burst Skill");
    }
    private void InfuseElementToWeapon()
    {
        _infusedElementToWeaponTimer = _infusedElementToWeaponDuration;
        _isBurstSkillHolding = true;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(_elementalSkillPoint.position, _elementalSkillSize);
    }
}
