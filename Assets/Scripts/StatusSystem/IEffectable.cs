public interface IEffectable {
    public void AddEffect(StatusEffect _statusEffect, int amountOfStacks = 1);
    public void RemoveEffect(StatusEffect _statusEffect);
    public void HandleEffects();
}