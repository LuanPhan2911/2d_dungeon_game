
using UnityEngine;

public class PlayerWaterBurst : PlayerBurst
{
    [SerializeField] private WaterPlayerElementSO _dataSO;
    public override void TriggerBurst()
    {
        // restore health instantly 
        float amount = _dataSO.healRate * PlayerHealth.Instance.GetMaxHealth();
        PlayerHealth.Instance.Heal(amount);


        DebuffEffectManager.PurifyEffect(DebuffEffectType.Burn);

        ActiveBuffEffect fireResistanceEffect = new ActiveBuffEffect
        {
            durationTimer = _dataSO.resistanceBonusDuration,
            type = BuffEffectType.ResistanceBonus,
            value = _dataSO.bonusResistance


        };
        BuffEffectManager.ApplyEffect(fireResistanceEffect);


        PlayerHealth.Instance.SetHealPerDuration(_dataSO.healPerDuration);

        ActiveBuffEffect allResistances = new ActiveBuffEffect
        {
            durationTimer = _dataSO.allResistancesDuration,
            type = BuffEffectType.AllResistances,
            value = _dataSO.allResistanceRate


        };
        BuffEffectManager.ApplyEffect(allResistances);
    }

    
}
