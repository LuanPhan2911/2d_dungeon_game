using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private PlayerHealthData _data;

    public static PlayerHealth Instance { get; private set;  }

    public event Action<float> OnHealthChanged;
    public event Action<float> OnEnergyChanged;


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


        OnHealthChanged?.Invoke(_health);
        OnEnergyChanged?.Invoke(_energy);
    }


    public void ChangeHealth(float amount)
    {
        _health = Mathf.Clamp(_health + amount, 0f, GetMaxHealth());

        if(_health <= 0f)
        {
            Debug.Log("player die");
        }

        float ratio = _health / GetMaxHealth();


        OnHealthChanged?.Invoke(ratio);
    }

    public void ChangeEnergy(float amount)
    {
        _energy = Mathf.Clamp(_energy + amount, 0f, GetMaxEnergy());

        

        float ratio = _health / GetMaxEnergy();


        OnEnergyChanged?.Invoke(ratio);
    }

    // Update is called once per frame
  
}
