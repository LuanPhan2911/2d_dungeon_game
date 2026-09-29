using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    


    public static PlayerSkill Instance { get ; private set; }
    public PlayerElementData Data => PlayerElement.Instance.CurrentPlayerElement;

    #region Elemental Skill
    private float _elementalSkillTimer;
    public bool IsElementalSkillCooldown => _elementalSkillTimer > 0f;
    public float ElementalSkillDuration => _elementalSkillTimer > 1f ?
        Mathf.Round(_elementalSkillTimer) : Mathf.Floor(_elementalSkillTimer * 10f) / 10f;
    public float ElementalSkillRatio => _elementalSkillTimer / Data.baseElementalSkillCooldown;

    #endregion


    #region Burst Skill

    private float _burstSkillTimer;
    private float _infusedElementToWeaponTimer;
    public bool IsBurstSkillCooldown => _burstSkillTimer > 0f;
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


    private void Update()
    {
        if (!Data) return;

        _elementalSkillTimer = Mathf.Max(_elementalSkillTimer - Time.deltaTime, 0f);
        _burstSkillTimer = Mathf.Max(_burstSkillTimer - Time.deltaTime, 0f);
        _infusedElementToWeaponTimer = Mathf.Max(_infusedElementToWeaponTimer - Time.deltaTime, 0f);


        _lastPressElementalSkillTimer = Mathf.Max(_lastPressElementalSkillTimer - Time.deltaTime, 0f);
        _lastPressBurstSkillTimer = Mathf.Max(_lastPressBurstSkillTimer - Time.deltaTime, 0f);



        if (GameInputManager.Instance.PlayerActions.ElementalSkill.WasPressedThisFrame())
        {
            _lastPressElementalSkillTimer = Data.skillInputBuffer;
        }

        if (GameInputManager.Instance.PlayerActions.BurstSkill.WasPressedThisFrame())
        {
            _lastPressBurstSkillTimer = Data.skillInputBuffer;
            _burstSkillPressTimer = 0f;
            _isBurstSkillHolding = false;
        }
        if (GameInputManager.Instance.PlayerActions.BurstSkill.IsPressed())
        {
            _burstSkillPressTimer += Time.deltaTime;
            if (_burstSkillPressTimer > Data.burstSkillPressedThreshhold && !_isBurstSkillHolding)
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
        return _lastPressBurstSkillTimer > 0f && PlayerHealth.Instance.IsEnoughEnergyToUseBurstSkill() && 
            !IsBurstSkillCooldown;
    }

    private void ElementalSkillAttack()
    {
        _elementalSkillTimer = Data.baseElementalSkillCooldown;

        Debug.Log("Elemental Skill");

        PlayerHealth.Instance.GainEnergyFormElementalSKill();

    }
    private void BurstSkillAttack()
    {
      

        PlayerHealth.Instance.UseEnergyForBurstSkill();
        Debug.Log("Burst Skill");
    }
    private void InfuseElementToWeapon()
    {
        _infusedElementToWeaponTimer = Data.infusedElementToWeaponDuration;
        _isBurstSkillHolding = true;
    }
}
