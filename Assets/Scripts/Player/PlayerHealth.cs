using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private PlayerHealthData _data;

    public static PlayerHealth Instance { get; private set;  }

    public event Action<float> OnHealthChanged;
    public event Action<float> OnEnergyChanged;


    public float HealthRatio => _health / GetMaxHealth();
    public float EnergyRatio => _energy / GetMaxEnergy();


    private void Awake()
    {
        Instance = this;
    }

    private float _health;
    private float _energy;


    private float GetMaxHealth()
    {
        return _data.baseHealth;
    }
    private float GetMaxEnergy()
    {
        return _data.baseEnergy;
    }

    void Start()
    {
        _health = GetMaxHealth();
        _energy = 0f;


        OnHealthChanged?.Invoke(HealthRatio);
        OnEnergyChanged?.Invoke(EnergyRatio) ;
    }


    private void ChangeHealth(float amount)
    {
        _health = Mathf.Clamp(_health + amount, 0f, GetMaxHealth());

        if(_health <= 0f)
        {
            Debug.Log("player die");
        }

      


        OnHealthChanged?.Invoke(HealthRatio);
    }

    private void ChangeEnergy(float amount)
    {
        _energy = Mathf.Clamp(_energy + amount, 0f, GetMaxEnergy());
        OnEnergyChanged?.Invoke(EnergyRatio);
    }

    public void GainEnergyFromNormalAttack()
    {
        ChangeEnergy(_data.gainingEnergyFromNormalAttack);
    }


  
}
