using System;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    
    public static PlayerLevel Instance { get; private set; }

    [SerializeField] private PlayerLevelSO _dataSO;

    [SerializeField] private int _level = 1;
    [SerializeField] private int _breakthroughCount = 0;
    [SerializeField] private int _currentExp = 0;


    public int Level => _level;
    public int CurrentEXP => _currentExp;
    public int BreakthroughCount => _breakthroughCount;

    public float HP => _dataSO.baseHP + (_level - 1) * _dataSO.hpPerLevel + 
        _breakthroughCount * _dataSO.hpBreakthrough;
    public float ATK => _dataSO.baseATK + (_level - 1) * _dataSO.atkPerLevel + 
        _breakthroughCount * _dataSO.atkBreakthrough;
    public float DEF => _dataSO.baseDEF + (_level - 1) * _dataSO.defPerLevel + 
        _breakthroughCount * _dataSO.defBreakthrough;


    public int ExpToLevelUp
    {
        get
        {
            if(_level >= _dataSO.maxLevel)
            {
                return 0;
            }
            return _dataSO.baseExpToLevelUp * (1 << _breakthroughCount);
        }
    }
    public bool NeedsBreakthrough =>
        _level < _dataSO.maxLevel && _level % _dataSO.breakthroughLevelNumber == 0 &&
        _breakthroughCount < _level / _dataSO.breakthroughLevelNumber;


    public event Action OnExpChanged;
    public event Action OnStatsChanged;
    public event Action OnLevelChanged;
    public event Action OnBreakthrough;
    private void Awake()
    {
        Instance = this;
    }



    public void AddExp(int amount)
    {
        if (amount <= 0 || _level >= _dataSO.maxLevel) return;

        _currentExp += amount;
        OnExpChanged?.Invoke();
        ProcessLevelUp();

    }

    private void ProcessLevelUp()
    {
        while(_level < _dataSO.maxLevel)
        {
            if (NeedsBreakthrough) break;

            if (_currentExp < ExpToLevelUp) break;

            _currentExp -= ExpToLevelUp;
            _level++;
            OnLevelChanged?.Invoke();
            OnStatsChanged?.Invoke();
            Debug.Log($"Level up! Current level: {_level}");
        }
    }

    public bool TryBreakthrough()
    {
        if (!NeedsBreakthrough) return false;

        _breakthroughCount++;
        OnBreakthrough?.Invoke();
        OnStatsChanged?.Invoke();
        Debug.Log(
          $"Breakthrough successful! " +
          $"Level: {_level}, " +
          $"HP: {HP}, ATK: {ATK}, DEF: {DEF}, " +
          $"Next EXP: {ExpToLevelUp}"
      );

        ProcessLevelUp();
        return true;
    }
}
