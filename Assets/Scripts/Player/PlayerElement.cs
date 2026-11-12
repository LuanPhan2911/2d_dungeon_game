using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElement : MonoBehaviour
{
    public static PlayerElement Instance { get; private set; }

    [SerializeField] private ElementSO _physicElementSO;
    [Header("Burst Skill")]
    [SerializeField] private float _infusedElementToWeaponDuration = 10f;
   
    public bool IsInfusedElementToWeapon => _infusedElementToWeaponTimer > 0f;
    private float _infusedElementToWeaponTimer;

    private void Awake()
    {
        Instance = this;
    }
 

    private void Update()
    {
       
        _infusedElementToWeaponTimer = Mathf.Max(_infusedElementToWeaponTimer - Time.deltaTime, 0f);

   
    }
    public ElementSO GetElementInfusedToWeapon()
    {

        return _physicElementSO;
    }
  

    private void StopInfusingToWeapon()
    {
        _infusedElementToWeaponTimer = 0f;
    }
     
    private void InfuseElementToWeapon()
    {
        _infusedElementToWeaponTimer = _infusedElementToWeaponDuration;
       
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        
    }

   
}
