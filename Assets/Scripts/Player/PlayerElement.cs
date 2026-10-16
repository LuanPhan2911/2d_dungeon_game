using System;
using UnityEngine;

public class PlayerElement : MonoBehaviour
{

    public static PlayerElement Instance { get; private set; }


    public PlayerAttack PAttack=> PlayerAttack.Instance;
    public PlayerMovement PMovement => PlayerMovement.Instance;
    public PlayerHealth PHealth => PlayerHealth.Instance;

    [Header("Burst Skill")]
    [SerializeField] private SteelBurstSlash _steelBurstSlashPrefab;

    [SerializeField] private GroundBurstTornado _groundBurstTornadoPrefab;
    [SerializeField] private float _infusedElementToWeaponDuration = 10f;


    [Serializable]
    public class PlayerElementData
    {
        public PlayerElementSO PlayerElementSO;
        [HideInInspector] public float burstTimer;
        [HideInInspector] public float burstEnergy;
    }

    [Header("Player Element Slot")]
    [SerializeField] PlayerElementData _firstPlayerElement;
    [SerializeField] PlayerElementData _secondPlayerElement;

    

    public PlayerElementData FirstPlayerElementData => _firstPlayerElement;
    public PlayerElementData SecondPlayerElementData => _secondPlayerElement;

    [Header("Element")]

    [SerializeField] private ElementSO _physicElementSO;
    [SerializeField] private ElementSO _steelElementSO;
    [SerializeField] private ElementSO _grassElementSO;
    [SerializeField] private ElementSO _waterElementSO;
    [SerializeField] private ElementSO _firstElementSO;
    [SerializeField] private ElementSO _groundElementSO;

    public ElementSO PhysicElementSO=> _physicElementSO;
    public ElementSO SteelElementSO=>_steelElementSO;
    public ElementSO GrassElementSO=>_grassElementSO;
    public ElementSO WaterElementSO=>_waterElementSO;
    public ElementSO FireElementSO=>_firstElementSO;
    public ElementSO GroundElementSO=>_groundElementSO;


    #region Active

    
    public bool HasActive => _activePlayerElement != null;
    public PlayerElementData ActivePlayerElement => _activePlayerElement;
    public ElementSO ActiveElementSO => ActivePlayerElement.PlayerElementSO.element;
    public PlayerElementSO ActivePlayerElementSO => ActivePlayerElement.PlayerElementSO;
    public float ActiveBurstTimer => ActivePlayerElement.burstTimer;
    public float ActiveBurstEnergy => ActivePlayerElement.burstEnergy;

    #endregion

    public event Action OnElementSwapped;

    public bool IsElementSwapCooldown => _elementSwapTimer > 0f;
    private float _elementSwapTimer;

    private PlayerElementData _activePlayerElement;
    private float _elementSwapCooldown=0.5f;



  


    #region Burst Skill

 

    public bool IsBurstCooldown => ActiveBurstTimer > 0f;
    public float BurstCooldownDuration => ActiveBurstTimer > 1f ?
                Mathf.Round(ActiveBurstTimer) : ActiveBurstTimer.OneDecimal();
    public float BurstCooldownRatio => ActiveBurstTimer / ActivePlayerElementSO.burstCooldown;
    public float BurstEnergyRatio => ActiveBurstEnergy / ActivePlayerElementSO.burstEnergy;
    public bool IsInfusedElementToWeapon => _infusedElementToWeaponTimer > 0f;


    private float _lastPressBurstSkillTimer;
    private float _infusedElementToWeaponTimer;

    #endregion





    private BuffEffectManager _buffEffectManager;
    private DebuffEffectManager _debuffEffectManager;


    private void Awake()
    {
        Instance = this;
        _buffEffectManager = GetComponent<BuffEffectManager>();
        _debuffEffectManager = GetComponent<DebuffEffectManager>();
       

    }

    private void Update()
    {
        _elementSwapTimer = Mathf.Max(0, _elementSwapTimer - Time.deltaTime);
     
        _infusedElementToWeaponTimer = Mathf.Max(_infusedElementToWeaponTimer - Time.deltaTime, 0f);

        if (!IsElementSwapCooldown)
        {
            if (GameInputManager.Instance.PlayerActions.First.WasPressedThisFrame())
            {
                SetActivePlayerElement(_firstPlayerElement);
            }
            if (GameInputManager.Instance.PlayerActions.Second.WasPressedThisFrame())
            {
                SetActivePlayerElement(_secondPlayerElement);
            }
           
        }



        if (HasActive)
        {
           
            _firstPlayerElement.burstTimer = Mathf.Max(_firstPlayerElement.burstTimer - Time.deltaTime, 0f);
            _secondPlayerElement.burstTimer = Mathf.Max(_secondPlayerElement.burstTimer - Time.deltaTime, 0f);



            _lastPressBurstSkillTimer = Mathf.Max(_lastPressBurstSkillTimer - Time.deltaTime, 0f);

            if (GameInputManager.Instance.PlayerActions.Burst.WasPressedThisFrame())
            {
                _lastPressBurstSkillTimer = ActivePlayerElementSO.skillInputBuffer;
               
            }
         
            if (CanUseBurst())
            {
                BurstAttack();
                InfuseElementToWeapon();
            }
        }

      

      

    }

    public bool HasBurst(PlayerElementData playerElementData)
    {
        if (playerElementData== null) return false;

        return playerElementData.burstEnergy >= playerElementData.PlayerElementSO.burstEnergy;
    }
    public ElementSO GetElementInfuseToWeapon()
    {
        
    
        if (!HasActive|| !IsInfusedElementToWeapon)
        {
            return PhysicElementSO;
        }
       
        return ActiveElementSO;
    }
    public void SetActivePlayerElement(PlayerElementData playerElementData)
    {
        if (ActivePlayerElement == playerElementData) return;

        _activePlayerElement = playerElementData;

        _elementSwapTimer = _elementSwapCooldown;
        StopInfuseElementToWeapon();
        OnElementSwapped?.Invoke();

    }

    public void StopInfuseElementToWeapon()
    {
        _infusedElementToWeaponTimer = 0f;
    }
   
    private bool CanUseBurst()
    {
        return _lastPressBurstSkillTimer > 0f && 
            ActivePlayerElement.burstEnergy >= ActivePlayerElementSO.burstEnergy &&
            !IsBurstCooldown;
    }
    private void BurstAttack()
    {

        _activePlayerElement.burstTimer = ActivePlayerElementSO.burstCooldown;
       _activePlayerElement.burstEnergy = 0f;
       

    
        Debug.Log("Burst Skill");

        #region Steel Burst Skill
        if (ActivePlayerElementSO is SteelPlayerElementSO steelPlayerElementSO)
        {
            float distance = steelPlayerElementSO.slashBurstDistance;
            float duration = steelPlayerElementSO.slashBurstDuration;
            SpawnSteelBurstSlash(steelPlayerElementSO.burstDamage, distance, duration);

            _debuffEffectManager.PurifyEffect(DebuffEffectType.Poison);
            

            ActiveBuffEffect grassResistanceEffect = new ActiveBuffEffect
            {
                durationTimer = steelPlayerElementSO.resistanceBonusDuration,
                type= BuffEffectType.ResistanceBonus,
                value= steelPlayerElementSO.bonusResistance
                

            };
            _buffEffectManager.ApplyEffect(grassResistanceEffect);

            ActiveBuffEffect counterAttackEffect = new ActiveBuffEffect
            {
                durationTimer = steelPlayerElementSO.counterAttackDuration,
                type= BuffEffectType.CounterAttack,
                value= steelPlayerElementSO.counterAttackRate
                

            };
            _buffEffectManager.ApplyEffect(counterAttackEffect);


            ActiveBuffEffect damageReductionEffect = new ActiveBuffEffect
            {
                durationTimer = steelPlayerElementSO.damageReductionDuration,
                type= BuffEffectType.DamageReduction,
                value= steelPlayerElementSO.damageReductionRate
                

            };

         
            _buffEffectManager.ApplyEffect(damageReductionEffect);
        }

        #endregion

        #region Water Burst Skill

        if(ActivePlayerElementSO is WaterPlayerElementSO waterPlayerElementSO)
        {
            // restore health instantly 
            float amount = waterPlayerElementSO.healRate * PHealth.GetMaxHealth();
            PHealth.Heal(amount);


            _debuffEffectManager.PurifyEffect(DebuffEffectType.Burn);

            ActiveBuffEffect fireResistanceEffect = new ActiveBuffEffect
            {
                durationTimer = waterPlayerElementSO.resistanceBonusDuration,
                type = BuffEffectType.ResistanceBonus,
                value = waterPlayerElementSO.bonusResistance


            };
            _buffEffectManager.ApplyEffect(fireResistanceEffect);


            PHealth.SetHealPerDuration(waterPlayerElementSO.healPerDuration);

            ActiveBuffEffect allResistances = new ActiveBuffEffect
            {
                durationTimer = waterPlayerElementSO.allResistancesDuration,
                type = BuffEffectType.AllResistances,
                value = waterPlayerElementSO.allResistanceRate


            };
            _buffEffectManager.ApplyEffect(allResistances);
        }

        #endregion



        #region Ground Burst Skill
        if(ActivePlayerElementSO is GroundPlayerElementSO groundPlayerElementSO)
        {
            float distance = groundPlayerElementSO.tornadoMoveDistance;
            float duration = groundPlayerElementSO.tornadoDuration;
            float damageAmount = groundPlayerElementSO.tornadoDamage;
            float damageInterval = groundPlayerElementSO.damageInterval;
            SpawnGroundTornado(damageAmount, damageInterval, distance, duration);



            ActiveBuffEffect moveSpeedBonusEffect = new ActiveBuffEffect
            {
                durationTimer = groundPlayerElementSO.moveSpeedDuration,
                value = groundPlayerElementSO.moveSpeedRate,
                type = BuffEffectType.MoveSpeedBonus,

            };

            _buffEffectManager.ApplyEffect(moveSpeedBonusEffect);

            ActiveBuffEffect attackSpeedBonusEffect = new ActiveBuffEffect
            {
                durationTimer = groundPlayerElementSO.attackSpeedDuration,
                value = groundPlayerElementSO.attackSpeedRate,
                type = BuffEffectType.AttackSpeedBonus,

            };

            _buffEffectManager.ApplyEffect(attackSpeedBonusEffect);

            ActiveBuffEffect debuffInscreasementEffect = new ActiveBuffEffect
            {
                durationTimer = groundPlayerElementSO.debuffIncreasementEffectDuration,
                value = groundPlayerElementSO.debuffIncreasementEffectRate,
                type = BuffEffectType.DebuffIncreasement,

            };

            _buffEffectManager.ApplyEffect(debuffInscreasementEffect);
        }


        #endregion
    }

    private void SpawnSteelBurstSlash(float damageAmount, float distance, float duration)
    {
        SteelBurstSlash slash = Instantiate(_steelBurstSlashPrefab, transform.position, Quaternion.identity);
        
        Damage damage = new Damage
        {
            Amount = damageAmount,
            ElementSO = ActiveElementSO,
            SourceAttackGameObject=gameObject
        };
        slash.Launch(PMovement.IsFacingRight,distance, duration );
        slash.SetDamage(damage);

    }
    private void SpawnGroundTornado(float damageAmount, float damageInterval,  float distance, float duration)
    {
        GroundBurstTornado tornado = 
            Instantiate(_groundBurstTornadoPrefab, transform.position, Quaternion.identity);


        tornado.Launch(PMovement.IsFacingRight, distance, duration);
        Damage damage = new Damage
        {
            Amount = damageAmount,
            ElementSO = ActiveElementSO,
            SourceAttackGameObject=gameObject
        };
        tornado.SetDamage(damage, damageInterval);
    }
    private void InfuseElementToWeapon()
    {
        _infusedElementToWeaponTimer = _infusedElementToWeaponDuration;
       
    }

    public float GetResistanceByElement(ElementSO element)
    {
        float value = 0f;

        if (HasActive)
        {
            float allResistances =  _buffEffectManager.HasEffect(BuffEffectType.AllResistances) ?
                     _buffEffectManager.GetValues(BuffEffectType.AllResistances): 0f;
            foreach (var item in ActivePlayerElement.PlayerElementSO.resistances)
            {
                if (item.element == element)
                {
                    value += item.resistance + allResistances;
                    break;
                }
            }
        }

        foreach (var item in _buffEffectManager.GetResiatanceBonusArray())
        {
            if (item.element == element)
            {
                value += item.resistance;
                break;
            }
        }
        return value;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        
    }

   
}
