using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour, IEffectable
{

    public float maxHealth = 10f;
    float _health;
    [SerializeField] FloatingStatusBar _healthBar;

    Dictionary<StatusEffect, StatusEffectData> _statusEffects = new Dictionary<StatusEffect, StatusEffectData>();

    void Awake()
    {
        _health = maxHealth;
    }

    protected void Update()
    {
        if (_statusEffects.Count > 0) HandleEffects();
    }

    public (float theoryDamage, float actualDamage) TakeDamage(float amount)
    {
        _health -= amount;
        _healthBar?.UpdateStatusBar(_health, maxHealth);
        if (_health <= 0)
        {
            _Die();
        }
        //if resistances are implemented, this can be used to track dealt damage
        return (amount, amount);
    }

    void _Die()
    {
        Invoke("_Despawn", 0f);
    }

    void _Despawn()
    {
        Destroy(this.gameObject);
    }

    public float GetCurrentHealth()
    {
        return _health;
    }

    public void AddEffect(StatusEffect _statusEffect, int amountOfStacks = 1)
    {
        if (_statusEffects.ContainsKey(_statusEffect))
        {
            _statusEffects[_statusEffect].stackSize = _statusEffects[_statusEffect].stackSize + amountOfStacks > _statusEffect.MaxStacks ? _statusEffect.MaxStacks : _statusEffects[_statusEffect].stackSize + amountOfStacks;
            _statusEffects[_statusEffect].currentDuration = 0f;
        }
        else
        {
            _statusEffects.Add(_statusEffect, new StatusEffectData());
            _statusEffects[_statusEffect].particleEffect = Instantiate(_statusEffect.ParticleEffect, transform);
        }
    }

    public void RemoveEffect(StatusEffect statusEffect)
    {
        if (_statusEffects.ContainsKey(statusEffect))
        {
            ParticleSystem particles = _statusEffects[statusEffect].particleEffect;
            particles.Stop();
            Destroy(particles, 2); 
            _statusEffects.Remove(statusEffect);
        }
    }

    List<StatusEffect> effectsToRemove = new List<StatusEffect>();
    public void MarkEffectForRemoval(StatusEffect statusEffect)
    {
        effectsToRemove.Add(statusEffect);
    }

    public void HandleEffects()
    {
        foreach (KeyValuePair<StatusEffect, StatusEffectData> kvp in _statusEffects)
        {
            if (kvp.Key == null || kvp.Value == null)
            {
                return;
            }
            kvp.Key.HandleEffect(this, kvp.Value);
        }

        foreach (StatusEffect effect in effectsToRemove)
        {
            RemoveEffect(effect);
        }
        effectsToRemove.Clear();
    }

}
