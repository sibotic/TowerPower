using UnityEngine;

public class StatusEffect : ScriptableObject
{
    public string Name;
    public string Description;
    
    public float DOTAmount;
    public float TickSpeed = 1;
    public float Duration = 5;
    public int MaxStacks = 10;

    public ParticleSystem ParticleEffect;




    public virtual void HandleEffect(Health origin, StatusEffectData sed){}

}