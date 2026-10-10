

using NUnit.Framework.Internal;
using System.Collections.Generic;
using UnityEngine;




public class DebuffEffectManager : MonoBehaviour
{
    [System.Serializable]
    public class ActiveEffect
    {
        public DebuffEffectSO effectSO;

        public int currentStacks;
        public float durationTimer;
        public float tickTimer;
        public float lastStackAddedTime;
        public float decayTimer;
        public bool isActive; // enough stack, trigger active effect
        public float valuePerTick;
        public GameObject effectUIInstance; // Reference to the instantiated UI prefab
      

        public ActiveEffect(DebuffEffectSO effectSO)
        {
            this.effectSO = effectSO;
            this.currentStacks = 0;
            this.lastStackAddedTime = -99f;
        }
    }

    public const float BURNING_DAMAGE_BONUS = 0.2f;

    [SerializeField] private float _decayStackTime = 3f;
    [SerializeField] private GameObject _effectUIGameObject;
    [SerializeField] private StatusEffectUI _effectUIPrefab;

    // thời gian để giảm stack nếu không có stack mới được thêm vào
    private List<ActiveEffect> _activeEffects = new List<ActiveEffect>();

    private List<ActiveEffect> _effectsToRemove = new List<ActiveEffect>();

    private void Update()
    {
        _effectsToRemove.Clear();
        foreach (var effect in _activeEffects)
        {
            HandleEffectLogic(effect);
        }
        foreach (var effect in _effectsToRemove)
        {
            _activeEffects.Remove(effect);
        }
    }



    public void ApplyEffect(DebuffEffectSO effectSO, float valuePerTick) { 
        
        ActiveEffect existingEffect = _activeEffects.Find(effect => effect.effectSO == effectSO);

        if(existingEffect== null)
        {
            existingEffect= new ActiveEffect(effectSO);
            _activeEffects.Add(existingEffect);
            existingEffect.effectUIInstance= Instantiate(_effectUIPrefab.gameObject, _effectUIGameObject.transform);
        }
        existingEffect.valuePerTick = valuePerTick;

       

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
                existingEffect.isActive = true;
            }
        }

        if (existingEffect.isActive)
        {
            existingEffect.durationTimer = effectSO.maxDuration; // reset duration timer when damage is active

        }


    }


    private void HandleEffectLogic(ActiveEffect effect)
    {

        

        if (effect.isActive)
        {
            effect.durationTimer -= Time.deltaTime;

            effect.tickTimer -= Time.deltaTime;

            if (effect.tickTimer <= 0f)
            {
               
                ExecuteEffectAction(effect);
                effect.tickTimer = effect.effectSO.tickInterval;
            }

            if (effect.durationTimer <= 0f)
            {
                effect.currentStacks = 0;

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
                }
            } 
        }

        if (effect.currentStacks <= 0)
        {
            _effectsToRemove.Add(effect);
            Destroy(effect.effectUIInstance);
        }
    }

   
    private void UpdateStatusEffectUI(ActiveEffect effect)
    {
        if(effect.effectUIInstance.TryGetComponent(out StatusEffectUI statusEffectUI))
        {
            statusEffectUI.SetImage(effect.effectSO.sprite);
            statusEffectUI.SetFillAmount((float)effect.currentStacks / effect.effectSO.maxStacks);
        }
      
    }
    private void ExecuteEffectAction(ActiveEffect effect)
    {
        // Implement the logic to apply the effect's action (e.g., damage, slow, etc.)
        // This is a placeholder for demonstration purposes.
        DebuffEffectSO effectSO = effect.effectSO;

        if(effectSO.type == DebuffEffectType.Poison || effectSO.type == DebuffEffectType.Burn)
        {
            if (gameObject.TryGetComponent(out IDamageable damageable))
            {
                Damage damage = new Damage
                {
                    amount = effect.valuePerTick,
                    elementSO = effectSO.elementSO,
                    canCrit = false,
                    isFromEffect = true,
                    
                };
                damageable.TakeDamage(damage);

            }
        }

       
        
    }

    public bool HasEffect(DebuffEffectType type)
    {
        // Tìm trong danh sách xem có hiệu ứng nào trùng loại và có stack lớn hơn 0 không
        return _activeEffects.Exists(e => e.effectSO.type == type && e.currentStacks > 0);
    }

    public bool IsEffectActive(DebuffEffectType type)
    {
        ActiveEffect effect = _activeEffects.Find(e => e.effectSO.type == type);
        if (effect != null)
        {
            return effect.isActive;
        }
        return false;
    }
    public void PurifyEffect (DebuffEffectType type)
    {
        if (!HasEffect(type)) return;

        ActiveEffect effect = _activeEffects.Find(e => e.effectSO.type == type);

        effect.currentStacks = 0;
    }
    
}
