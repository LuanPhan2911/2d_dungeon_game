using NUnit.Framework.Internal;
using System.Collections.Generic;

using UnityEngine;


[System.Serializable]
public class ActiveEffect
{
    public StatusEffectSO effectSO;

    public int currentStacks;
    public float durationTimer;
    public float tickTimer;
    public float lastStackAddedTime;
    public float decayTimer;
    public bool isDamageActive; 
    public GameObject effectUIInstance; // Reference to the instantiated UI prefab

    public ActiveEffect(StatusEffectSO effectSO)
    {
        this.effectSO = effectSO;
        this.currentStacks = 0;
        this.lastStackAddedTime = -99f;
    }
}

public class StatusEffectManager : MonoBehaviour
{

    [SerializeField] private float _decayStackTime = 5f;
    [SerializeField] private GameObject _parentUIGameObject;

    // thời gian để giảm stack nếu không có stack mới được thêm vào
    private List<ActiveEffect> activeEffects = new List<ActiveEffect>();

    private List<ActiveEffect> effectsToRemove = new List<ActiveEffect>();

    private void Update()
    {
        effectsToRemove.Clear();
        foreach (var effect in activeEffects)
        {
            HandleEffectLogic(effect);
        }
        foreach (var effect in effectsToRemove)
        {
            activeEffects.Remove(effect);
        }
    }



    public void ApplyEffect(StatusEffectSO effectSO) { 
        
        ActiveEffect existingEffect = activeEffects.Find(effect => effect.effectSO == effectSO);

        if(existingEffect== null)
        {
            existingEffect= new ActiveEffect(effectSO);
            activeEffects.Add(existingEffect);

            existingEffect.effectUIInstance= Instantiate(effectSO.effectUIPrefab, _parentUIGameObject.transform);
        }

        float currentTime = Time.time;
       

        if(currentTime - existingEffect.lastStackAddedTime >= effectSO.stackCooldown)
        {
            if (existingEffect.currentStacks < effectSO.maxStacks)
            {
                existingEffect.currentStacks++;
                existingEffect.lastStackAddedTime = currentTime;
                existingEffect.decayTimer = _decayStackTime; // reset decay timer when a new stack is added

                UpdateStatusEffectUI(existingEffect);

            }

            if(existingEffect.currentStacks>= effectSO.maxStacks)
            {
                existingEffect.isDamageActive = true;
            }
        }

        if (existingEffect.isDamageActive)
        {
            existingEffect.durationTimer = effectSO.maxDuration; // reset duration timer when damage is active

        }


    }


    private void HandleEffectLogic(ActiveEffect effect)
    {

        

        if (effect.isDamageActive)
        {
            effect.durationTimer -= Time.deltaTime;

            effect.tickTimer -= Time.deltaTime;

            if (effect.tickTimer <= 0f)
            {
                // Apply damage here
                ExecuteEffectAction(effect.effectSO);
                effect.tickTimer = effect.effectSO.tickInterval;
            }

            if (effect.durationTimer <= 0f)
            {
                effectsToRemove.Add(effect);
                Destroy(effect.effectUIInstance);

            }
        }
        else
        {
            if (effect.currentStacks > 0)
            {
                effect.decayTimer -= Time.deltaTime;
                if (effect.decayTimer <= 0f)
                {
                    effect.currentStacks--;
                    effect.decayTimer = _decayStackTime;
                    UpdateStatusEffectUI(effect);



                    if (effect.currentStacks <= 0)
                    {
                        effectsToRemove.Add(effect);
                        Destroy(effect.effectUIInstance);
                    }

                }
            }
        }        
    }
    private void UpdateStatusEffectUI(ActiveEffect effect)
    {
        if (effect.effectUIInstance.TryGetComponent(out StatusEffectUI statusEffectUI))
        {
            statusEffectUI.SetFillAmount((float)effect.currentStacks / effect.effectSO.maxStacks);
        }
    }
    private void ExecuteEffectAction(StatusEffectSO effectSO)
    {
        // Implement the logic to apply the effect's action (e.g., damage, slow, etc.)
        // This is a placeholder for demonstration purposes.

        if (gameObject.TryGetComponent(out IDamageable damageable))
        {
            if (effectSO.type == EffectType.Poison || effectSO.type == EffectType.Burn)
            {
                Damage damage= new Damage
                {
                    amount = effectSO.valuePerTick,
                    elementSO = effectSO.elementSO,
                    isCrit = false,
                    type = DamageType.EffectDamage,
                    isFromEffect = true
                };
                damageable.TakeDamage(damage);
            }

        }
        
    }

    public bool HasEffect(EffectType type)
    {
        // Tìm trong danh sách xem có hiệu ứng nào trùng loại và có stack lớn hơn 0 không
        return activeEffects.Exists(e => e.effectSO.type == type && e.currentStacks > 0);
    }

    public bool IsEffectActive(EffectType type)
    {
        ActiveEffect effect = activeEffects.Find(e => e.effectSO.type == type);
        if (effect != null)
        {
            return effect.isDamageActive;
        }
        return false;
    }
    public float GetDamageBonusPercent(EffectType type)
    {
        ActiveEffect effect = activeEffects.Find(e => e.effectSO.type == type);
        if (effect != null)
        {
            return effect.effectSO.damageBonusPercent;
        }
        return 0f;
    }
}
