using UnityEngine;

public abstract class Health : MonoBehaviour
{

    public float maxHealth = 10f;
    float _health;


    void Awake()
    {
        _health = maxHealth;
    }

    public bool TakeDamage(float amount){
        Debug.Log($"Taking {amount} damage");
        _health -= amount;

        if (_health <= 0){
            _Die();
        }

        //in case some invurnable stuff is introduced, false could be returned
        return true;
    }

    void _Die(){
        Invoke("_Despawn", 0f);
    }

    void _Despawn(){
        Destroy(this.gameObject);
    }
}
