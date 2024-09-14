using UnityEngine;
using UnityEngine.Events;

public abstract class Health : MonoBehaviour
{

    public float maxHealth = 10f;
    float _health;
    public UnityEvent<float, float> HealthChanged;


    void Awake()
    {
        _health = maxHealth;
    }

    public (float theoryDamage, float actualDamage) TakeDamage(float amount)
    {
        _health -= amount;
        HealthChanged.Invoke(_health, maxHealth);

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
}
