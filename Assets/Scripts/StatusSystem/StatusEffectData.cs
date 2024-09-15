using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    public string Name;
    public string Description;
    
    public float DOTAmount;
    public float TickSpeed = 1;
    public float Duration = 5;
    public int MaxStacks = 10;

    public ParticleSystem particles;

}