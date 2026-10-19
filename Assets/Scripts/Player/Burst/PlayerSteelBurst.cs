using UnityEngine;

public class PlayerSteelBurst : PlayerBurst
{
    [SerializeField] private SteelPlayerElementSO _dataSO;
    [SerializeField] private SteelBurstSlash _steelBurstSlashPrefab;


   

    private void SpawnSteelBurstSlash(float damageAmount, float distance, float duration)
    {
        Vector3 position = PlayerMovement.Instance.transform.position;
        SteelBurstSlash slash = Instantiate(_steelBurstSlashPrefab, position, Quaternion.identity);

        Damage damage = new Damage
        {
            Amount = damageAmount,
            ElementSO = _dataSO.element,
            SourceAttackGameObject = gameObject
        };
        slash.Launch(PlayerMovement.Instance.IsFacingRight, distance, duration);
        slash.SetDamage(damage);

    }

    public override void TriggerBurst()
    {
        float distance = _dataSO.slashBurstDistance;
        float duration = _dataSO.slashBurstDuration;
        SpawnSteelBurstSlash(_dataSO.burstDamage, distance, duration);

        DebuffEffectManager.PurifyEffect(DebuffEffectType.Poison);


        ActiveBuffEffect grassResistanceEffect = new ActiveBuffEffect
        {
            durationTimer = _dataSO.resistanceBonusDuration,
            type = BuffEffectType.ResistanceBonus,
            value = _dataSO.bonusResistance


        };
        BuffEffectManager.ApplyEffect(grassResistanceEffect);

        ActiveBuffEffect counterAttackEffect = new ActiveBuffEffect
        {
            durationTimer = _dataSO.counterAttackDuration,
            type = BuffEffectType.CounterAttack,
            value = _dataSO.counterAttackRate


        };
        BuffEffectManager.ApplyEffect(counterAttackEffect);


        ActiveBuffEffect damageReductionEffect = new ActiveBuffEffect
        {
            durationTimer = _dataSO.damageReductionDuration,
            type = BuffEffectType.DamageReduction,
            value = _dataSO.damageReductionRate


        };


        BuffEffectManager.ApplyEffect(damageReductionEffect);
    }


}
