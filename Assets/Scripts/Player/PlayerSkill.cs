using UnityEngine;
using UnityEngine.Rendering;

public class PlayerSkill : MonoBehaviour
{
    


    public static PlayerSkill Instance { get ; private set; }
    public PlayerElementData ActivePlayerElement => PlayerElement.Instance.ActivePlayerElement;
    public int ActivePlayerElementIndex => PlayerElement.Instance.ActivePlayerElementIndex;

    #region Elemental Skill
    private float _elementalSkillTimer;
    public bool IsElementalSkillCooldown => _elementalSkillTimer > 0f;
    public float ElementalSkillDuration => _elementalSkillTimer > 1f ?
        Mathf.Round(_elementalSkillTimer) : _elementalSkillTimer.OneDecimal();
    public float ElementalSkillRatio => _elementalSkillTimer / ActivePlayerElement.baseElementalSkillCooldown;

    #endregion


    #region Burst Skill

    private float ActiveBurstSkillTimer => _burstSkillTimers[ActivePlayerElementIndex];
    private float[] _burstSkillTimers;


    
    private float _infusedElementToWeaponTimer;
    public bool IsBurstSkillCooldown => ActiveBurstSkillTimer > 0f;
    public float BurstSkillDuration => ActiveBurstSkillTimer > 1f ?
                Mathf.Round(ActiveBurstSkillTimer) : ActiveBurstSkillTimer.OneDecimal();
    public float BurstSkillRatio => ActiveBurstSkillTimer / ActivePlayerElement.burstCooldown;
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
        _burstSkillTimers = new float[PlayerElement.Instance.PlayerElementArray.Length];

    }



    private void Update()
    {
        if (!PlayerElement.Instance.HasActivePlayerElement) return;

        _elementalSkillTimer = Mathf.Max(_elementalSkillTimer - Time.deltaTime, 0f);
        _infusedElementToWeaponTimer = Mathf.Max(_infusedElementToWeaponTimer - Time.deltaTime, 0f);

        for(int i=0; i< _burstSkillTimers.Length; i++)
        {
            _burstSkillTimers[i] = Mathf.Max(_burstSkillTimers[i] - Time.deltaTime, 0f);
        }


        _lastPressElementalSkillTimer = Mathf.Max(_lastPressElementalSkillTimer - Time.deltaTime, 0f);
        _lastPressBurstSkillTimer = Mathf.Max(_lastPressBurstSkillTimer - Time.deltaTime, 0f);



        if (GameInputManager.Instance.PlayerActions.ElementalSkill.WasPressedThisFrame())
        {
            _lastPressElementalSkillTimer = ActivePlayerElement.skillInputBuffer;
        }

        if (GameInputManager.Instance.PlayerActions.BurstSkill.WasPressedThisFrame())
        {
            _lastPressBurstSkillTimer = ActivePlayerElement.skillInputBuffer;
            _burstSkillPressTimer = 0f;
            _isBurstSkillHolding = false;
        }
        if (GameInputManager.Instance.PlayerActions.BurstSkill.IsPressed())
        {
            _burstSkillPressTimer += Time.deltaTime;
            if (_burstSkillPressTimer > ActivePlayerElement.burstSkillPressedThreshhold && !_isBurstSkillHolding)
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
        return _lastPressBurstSkillTimer > 0f && PlayerHealth.Instance.CurrentEnergy>= ActivePlayerElement.burstEnergy && 
            !IsBurstSkillCooldown;
    }

    private void ElementalSkillAttack()
    {
        _elementalSkillTimer = ActivePlayerElement.baseElementalSkillCooldown;

        Debug.Log("Elemental Skill");

        PlayerHealth.Instance.GainEnergyFormElementalSKill();

    }
    private void BurstSkillAttack()
    {

        _burstSkillTimers[ActivePlayerElementIndex] = ActivePlayerElement.burstCooldown;
        PlayerHealth.Instance.UseEnergyForBurstSkill();
        Debug.Log("Burst Skill");
    }
    private void InfuseElementToWeapon()
    {
        _infusedElementToWeaponTimer = ActivePlayerElement.infusedElementToWeaponDuration;
        _isBurstSkillHolding = true;
    }
}
