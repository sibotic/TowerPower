using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffect/Poison")]
public class PoisonStatusEffect : StatusEffect
{
    public override void HandleEffect(Health origin, StatusEffectData sed)
    {
        sed.currentDuration += Time.deltaTime;

        if (sed.currentDuration >= Duration) { Decay(sed); }
        if (sed.stackSize <= 0) { origin.MarkEffectForRemoval(this); return; }

        if (sed.nextTick <= Time.time)
        {
            origin.TakeDamage(DOTAmount * sed.stackSize);
            sed.nextTick = Time.time + TickSpeed;
        }
    }

    void Decay(StatusEffectData sed)
    {
        sed.stackSize = sed.stackSize / 2;
        sed.currentDuration = 0f;
    }

}

