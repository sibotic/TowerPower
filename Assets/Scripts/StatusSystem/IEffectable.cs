public interface IEffectable {
    public void AddEffect(StatusEffectData _data, int amountOfStacks = 1);
    public void RemoveEffect(StatusEffectData _data);
    public void HandleEffects();
}