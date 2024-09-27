using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffect/Fire")]
public class FireStatusEffect : StatusEffect
{
    public override void HandleEffect(Health origin, StatusEffectData sed)
    {
        sed.currentDuration += Time.deltaTime;

        if (sed.currentDuration >= Duration || sed.stackSize <= 0) { origin.MarkEffectForRemoval(this); return; }

        if (sed.nextTick <= Time.time)
        {
            origin.TakeDamage(DOTAmount * sed.stackSize);
            Decay(sed);
            sed.nextTick = Time.time + TickSpeed;
        }
    }

    void Decay(StatusEffectData sed)
    {
        sed.stackSize -= 1;
    }

}

