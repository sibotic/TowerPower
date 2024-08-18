using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float baseDamage;
    public float bounces = 0;
    public float bounceDamageMultiplier = 1.5f;
    float _damage;

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.TryGetComponent<Creature>(out Creature creature)) {
            creature.TakeDamage(_damage);
        }

        if(bounces <= 0){
            Destroy(this.gameObject);
        }else{
            bounces--;
            _damage *= bounceDamageMultiplier;
        }
    }

    public void SetDamage(float multiplier){
        _damage = baseDamage * multiplier;
    }

}
