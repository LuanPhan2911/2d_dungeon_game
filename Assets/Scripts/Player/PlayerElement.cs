using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerElementType
{
    public PlayerElementSO DataSO;
    public PlayerBurst Burst;
    public bool IsActive;
    public bool IsElementSlot;

}
[Serializable]
public class PlayerElementSlot
{
    public PlayerElementType PlayerElementType;
    public float BurstTimer;
    public float BurstEnergy;
    public bool IsOnField;
}


public class PlayerElement : MonoBehaviour
{
    public static PlayerElement Instance { get; private set; }

    [SerializeField] private ElementSO _physicElementSO;

    [Header("Elements")]
    [SerializeField] private PlayerElementType[] _playerElementTypeArray;

    [Header("Player Element Slot")]
    private List<PlayerElementSlot> _playerElementSlotList= new List<PlayerElementSlot>();
    const int FIRST_PLAYER_ELEMENT_SLOT = 0;
    const int SECOND_PLAYER_ELEMENT_SLOT = 1;
    const int THIRD_PLAYER_ELEMENT_SLOT = 2;

    const int MAX_PLAYER_ELEMENT_SLOT = 3;

    [Header("Burst Skill")]
    [SerializeField] private float _infusedElementToWeaponDuration = 10f;
    [SerializeField] private float _burstInputBuffer = 0.2f;

    public bool HasPlayerElementSlot => _playerElementSlotList.Count > 0;
    public bool IsElementSwapCooldown => _elementSwapTimer > 0f;
    public bool IsInfusedElementToWeapon => _infusedElementToWeaponTimer > 0f;
    public PlayerElementType[] PlayerElementTypeArray => _playerElementTypeArray;
    public PlayerElementSlot[] PlayerElementSlotArray => _playerElementSlotList.ToArray();

    public bool IsMaxPlayerElementSlot => _playerElementSlotList.Count >= MAX_PLAYER_ELEMENT_SLOT;

    private float _lastPressBurstSkillTimer;
    private float _infusedElementToWeaponTimer;
    private float _elementSwapTimer;
    private float _elementSwapCooldown = 0.5f;
    public event Action OnPlayerElementSlotChanged;
    public event Action OnFieldPlayerElementSlotChanged;
 
    


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        PlayerParry.OnParrySuccess += PlayerParry_OnParrySuccess;
    }

    private void PlayerParry_OnParrySuccess()
    {
        GainEnergyFromParrySuccess();
    }

    private void Update()
    {
        _elementSwapTimer = Mathf.Max(0, _elementSwapTimer - Time.deltaTime);
     
        _infusedElementToWeaponTimer = Mathf.Max(_infusedElementToWeaponTimer - Time.deltaTime, 0f);

        if (!IsElementSwapCooldown)
        {
            if (GameInputManager.Instance.PlayerActions.First.WasPressedThisFrame())
            {
                SetOnFieldPlayerElementSlot(FIRST_PLAYER_ELEMENT_SLOT);
            }
            if (GameInputManager.Instance.PlayerActions.Second.WasPressedThisFrame())
            {
                SetOnFieldPlayerElementSlot(SECOND_PLAYER_ELEMENT_SLOT);
            }
            if (GameInputManager.Instance.PlayerActions.Third.WasPressedThisFrame())
            {
                SetOnFieldPlayerElementSlot(THIRD_PLAYER_ELEMENT_SLOT);
            }
           
        }
        if (HasPlayerElementSlot)
        {
           
            for(int i=0; i< _playerElementSlotList.Count; i++)
            {
                _playerElementSlotList[i].BurstTimer =
                    Mathf.Max(_playerElementSlotList[i].BurstTimer - Time.deltaTime, 0f);
            }
          
            _lastPressBurstSkillTimer = Mathf.Max(_lastPressBurstSkillTimer - Time.deltaTime, 0f);

            if (GameInputManager.Instance.PlayerActions.Burst.WasPressedThisFrame())
            {
                _lastPressBurstSkillTimer = _burstInputBuffer;
               
            }
         
            if (CanUseBurst())
            {
                BurstAttack();
                InfuseElementToWeapon();
            }
        }
    }

    

   
    public void AddPlayerElementSlot(PlayerElementType playerElementType)
    {
        PlayerElementSlot slot = new PlayerElementSlot
        {
            PlayerElementType = playerElementType,
        };
        _playerElementSlotList.Add(slot);
        SetFirstIndexOnFieldPlayerElementSlot();
        OnPlayerElementSlotChanged?.Invoke();
    }
    public void RemovePlayerElementSlot(PlayerElementType playerElementType)
    {
        PlayerElementSlot slot = _playerElementSlotList.Find(el => el.PlayerElementType == playerElementType);
        if(slot!= null)
        {
            _playerElementSlotList.Remove(slot);
            SetFirstIndexOnFieldPlayerElementSlot();
            OnPlayerElementSlotChanged?.Invoke();
        }
    }
    private void SetFirstIndexOnFieldPlayerElementSlot()
    {
        for(int i=0; i< _playerElementSlotList.Count; i++)
        {
            _playerElementSlotList[i].IsOnField = i == 0;
        }
    }

    public void ActivatePlayerElementType(PlayerElementSO dataSO)
    {
        foreach (PlayerElementType playerElementType in _playerElementTypeArray)
        {
            if (playerElementType.DataSO == dataSO)
            {
                playerElementType.IsActive = true;
                break;
            }
        }
    }
    private void SetOnFieldPlayerElementSlot(int slotIndex)
    {
        if (_playerElementSlotList.Count >= slotIndex) return;
        if (_playerElementSlotList[slotIndex].IsOnField) return;

        for (int i = 0; i < _playerElementSlotList.Count; i++)
        {
            _playerElementSlotList[i].IsOnField = slotIndex == i;
        }
        OnFieldPlayerElementSlotChanged?.Invoke();
        _elementSwapTimer = _elementSwapCooldown;
        StopInfusingToWeapon();
    }
    public PlayerElementSlot GetOnFieldPlayerElementSlot()
    {
        return _playerElementSlotList.Find(el => el.IsOnField);
    }
    public PlayerElementSlot[] GetOffFieldPlayerElementSlotArray()
    {
        return _playerElementSlotList.FindAll(el => !el.IsOnField).ToArray();
    }
    public ElementSO GetElementInfusedToWeapon()
    {
        PlayerElementSlot onFieldPlayerElementSlot = GetOnFieldPlayerElementSlot();
        if (!HasPlayerElementSlot|| !IsInfusedElementToWeapon|| onFieldPlayerElementSlot== null)
        {
            return _physicElementSO;
        }
       
        return onFieldPlayerElementSlot.PlayerElementType.DataSO.elementSO;
    }
  

    private void StopInfusingToWeapon()
    {
        _infusedElementToWeaponTimer = 0f;
    }
   
    private bool CanUseBurst()
    {
        PlayerElementSlot onFieldPlayerElementSlot = GetOnFieldPlayerElementSlot();

        if (!HasPlayerElementSlot || onFieldPlayerElementSlot == null) return false;

        
        return _lastPressBurstSkillTimer > 0f && onFieldPlayerElementSlot.BurstTimer <= 0f &&
             onFieldPlayerElementSlot.BurstEnergy >= onFieldPlayerElementSlot.PlayerElementType.DataSO.burstEnergy 
            ;
    }
    private void BurstAttack()
    {
        PlayerElementSlot onFieldPlayerElementSlot = GetOnFieldPlayerElementSlot();
        if(onFieldPlayerElementSlot!= null)
        {
            onFieldPlayerElementSlot.BurstTimer = onFieldPlayerElementSlot.PlayerElementType.DataSO.burstCooldown;
            onFieldPlayerElementSlot.BurstEnergy = 0f;

            Debug.Log("Burst Skill");

            onFieldPlayerElementSlot.PlayerElementType.Burst.TriggerBurst();
        }
    }

    
   
    private void InfuseElementToWeapon()
    {
        _infusedElementToWeaponTimer = _infusedElementToWeaponDuration;
       
    }
    private void GainEnergyFromParrySuccess()
    {

        float onFieldAmount = 10;


        PlayerElementSlot onFieldPlayerElementSlot = GetOnFieldPlayerElementSlot();
        if(onFieldPlayerElementSlot!= null)
        {
            onFieldPlayerElementSlot.BurstEnergy = Mathf.Min(onFieldPlayerElementSlot.BurstEnergy + onFieldAmount,
                onFieldPlayerElementSlot.PlayerElementType.DataSO.burstEnergy);
        }
        float offFieldAmount = onFieldAmount / 2;

        PlayerElementSlot[] offFieldPlayerElementSlotArray = GetOffFieldPlayerElementSlotArray();
        for(int i=0; i< offFieldPlayerElementSlotArray.Length; i++)
        {
            offFieldPlayerElementSlotArray[i].BurstEnergy=
                Mathf.Min(offFieldPlayerElementSlotArray[i].BurstEnergy + offFieldAmount,
                offFieldPlayerElementSlotArray[i].PlayerElementType.DataSO.burstEnergy);
        } 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        
    }

   
}
