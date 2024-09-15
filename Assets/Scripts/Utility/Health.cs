using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour, IEffectable
{

    public float maxHealth = 10f;
    float _health;
    [SerializeField] FloatingStatusBar _healthBar;

    Dictionary<StatusEffectData, int> _statusEffects = new Dictionary<StatusEffectData, int>();

    void Awake()
    {
        _health = maxHealth;
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

    public void AddEffect(StatusEffectData _data, int amountOfStacks = 1)
    {
        if (_statusEffects.ContainsKey(_data))
        {
           _statusEffects[_data] = _statusEffects[_data] + amountOfStacks > _data.MaxStacks ? _data.MaxStacks : _statusEffects[_data] + amountOfStacks;
        }
        else
        {
            _statusEffects.Add(_data, amountOfStacks);
        }
    }

    public void RemoveEffect(StatusEffectData _data)
    {
        if (_statusEffects.ContainsKey(_data))
        {
            _statusEffects.Remove(_data);
        }
    }

    public void HandleEffects()
    {
        foreach (KeyValuePair<StatusEffectData, int> kvp in _statusEffects){
            Debug.Log("Name: " + kvp.Key.name + " Stacks: " + kvp.Value);
        }
    }

    protected void Update(){
        if (_statusEffects.Count > 0) HandleEffects();
    }
}
