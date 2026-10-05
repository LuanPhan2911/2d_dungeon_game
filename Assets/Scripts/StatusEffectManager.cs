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

    public ActiveEffect(StatusEffectSO effectSO)
    {
        this.effectSO = effectSO;
        this.currentStacks = 0;
        this.lastStackAddedTime = -99f;
    }
}

public class StatusEffectManager : MonoBehaviour
{

    [SerializeField] private float _stackDecayTime = 5f; 
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
        }

        float currentTime = Time.time;
       

        if(currentTime - existingEffect.lastStackAddedTime >= effectSO.stackCooldown)
        {
            if (existingEffect.currentStacks < effectSO.maxStacks)
            {
                existingEffect.currentStacks++;
                existingEffect.lastStackAddedTime = currentTime;
                existingEffect.decayTimer = _stackDecayTime; // reset decay timer when a new stack is added

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
                effect.currentStacks = 0;
                effect.isDamageActive = false;

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
                    effect.decayTimer = _stackDecayTime;

                    if (effect.currentStacks <= 0)
                    {
                        effectsToRemove.Add(effect);
                    }

                }
            }
        }                                                              

       
    }
    private void ExecuteEffectAction(StatusEffectSO effectSO)
    {
        // Implement the logic to apply the effect's action (e.g., damage, slow, etc.)
        // This is a placeholder for demonstration purposes.

        if (gameObject.TryGetComponent(out IDamageable damageable))
        {
            if (effectSO.type == EffectType.Poison)
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
}
