using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{

    public float maxHealth = 10f;
    float _health;


    void Awake()
    {
        _health = maxHealth;
    }

    public bool TakeDamage(float amount){
        _health -= amount;

        if (_health <= 0){
            _Die();
        }

        //in case some invurnable stuff is introduced, false could be returned
        return true;
    }

    void _Die(){
        Invoke("_Despawn", .2f);
    }

    void _Despawn(){
        Destroy(this.gameObject);
    }
}
