using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ActiveBuffEffect
{
    public float durationTimer;
    public BuffEffectType type;
    public dynamic value;
   
}
public enum BuffEffectType
{
    DamageBonus,
    CounterAttack,
    DamageReduction,

    ResistanceBonus,
  
}

public class BuffEffectManager : MonoBehaviour
{
    
   
    

    private List<ActiveBuffEffect> _activeEffects = new List<ActiveBuffEffect>();

    private List<ActiveBuffEffect> _effectsToRemove = new List<ActiveBuffEffect>();

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

    private void HandleEffectLogic(ActiveBuffEffect effect)
    {
        effect.durationTimer -= Time.deltaTime;
        if (effect.durationTimer <= 0f)
        {
            _effectsToRemove.Add(effect);
           
        }
    }

    public void ApplyEffect(ActiveBuffEffect effect)
    {
       

        _activeEffects.Add(effect);

    }

    public bool HasEffect(BuffEffectType type)
    {
        return _activeEffects.Exists(e => e.type == type);
    }

    public float GetValues(BuffEffectType type)
    {
        float value = 0f;
        foreach(ActiveBuffEffect effect in _activeEffects)
        {
            if(effect.type== type && effect.value is float floatValue)
            {
                value += floatValue;
            }
        }
        return value;
    }
    public ElementalResistance[] GetResiatanceBonusArray()
    {
        List<ElementalResistance> resistances = new List<ElementalResistance>();
        foreach(ActiveBuffEffect effect in _activeEffects) 
        {
            if(effect.type== BuffEffectType.ResistanceBonus && effect.value is ElementalResistance resistance)
            {
                resistances.Add(resistance);
            }
        }
        return resistances.ToArray();
    }


}
