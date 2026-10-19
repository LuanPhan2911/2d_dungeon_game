using UnityEngine;

public class PlayerGroundBurst : PlayerBurst
{

    [SerializeField] private GroundPlayerElementSO _dataSO;
    [SerializeField] private GroundBurstTornado _groundBurstTornadoPrefab;

    public override void TriggerBurst()
    {
        float distance = _dataSO.tornadoMoveDistance;
        float duration = _dataSO.tornadoDuration;
        float damageAmount = _dataSO.tornadoDamage;
        float damageInterval = _dataSO.damageInterval;
        SpawnGroundTornado(damageAmount, damageInterval, distance, duration);



        ActiveBuffEffect moveSpeedBonusEffect = new ActiveBuffEffect
        {
            durationTimer = _dataSO.moveSpeedDuration,
            value = _dataSO.moveSpeedRate,
            type = BuffEffectType.MoveSpeedBonus,

        };

        BuffEffectManager.ApplyEffect(moveSpeedBonusEffect);

        ActiveBuffEffect attackSpeedBonusEffect = new ActiveBuffEffect
        {
            durationTimer = _dataSO.attackSpeedDuration,
            value = _dataSO.attackSpeedRate,
            type = BuffEffectType.AttackSpeedBonus,

        };

        BuffEffectManager.ApplyEffect(attackSpeedBonusEffect);

        ActiveBuffEffect debuffInscreasementEffect = new ActiveBuffEffect
        {
            durationTimer = _dataSO.debuffIncreasementEffectDuration,
            value = _dataSO.debuffIncreasementEffectRate,
            type = BuffEffectType.DebuffIncreasement,

        };

        BuffEffectManager.ApplyEffect(debuffInscreasementEffect);
    }

    private void SpawnGroundTornado(float damageAmount, float damageInterval, float distance, float duration)
    {
        Vector3 position = PlayerMovement.Instance.transform.position;
        GroundBurstTornado tornado =
            Instantiate(_groundBurstTornadoPrefab, position, Quaternion.identity);


        tornado.Launch(PlayerMovement.Instance.IsFacingRight, distance, duration);
        Damage damage = new Damage
        {
            Amount = damageAmount,
            ElementSO = _dataSO.element,
            SourceAttackGameObject = gameObject
        };
        tornado.SetDamage(damage, damageInterval);
    }
}
