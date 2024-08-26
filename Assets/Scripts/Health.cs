using UnityEngine;

public abstract class Health : MonoBehaviour
{

    public float maxHealth = 10f;
    public float goldDropped = 50f;
    float _health;
    FloatingHealthBar _healthBar;


    void Awake()
    {
        _health = maxHealth;
        _healthBar = GetComponent<FloatingHealthBar>();
    }

    public (float theoryDamage, float actualDamage) TakeDamage(float amount){
        _health -= amount;
        
        //_healthBar.UpdateStatusBar(_health, maxHealth);

        if (_health <= 0){
            _Die();
        }
        //if resistances are implemented, this can be used to track dealt damage
        return (amount, amount);
    }

    void _Die(){
        GoldManager.AddGold(goldDropped);
        Invoke("_Despawn", 0f);
    }

    void _Despawn(){
        Destroy(this.gameObject);
    }
}
