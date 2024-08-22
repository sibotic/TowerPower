using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float baseDamage, lifetime;
    public float bounces = 0;
    public float bounceDamageMultiplier = 1.5f;
    float _damage, _bulletSpeed, _startOfLife;
    bool _seeking;
    Transform _target = null;
    Rigidbody _rb = null;

    private void Start() {
        _startOfLife = Time.time;
    }

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

    private void FixedUpdate() {
        if(_seeking){
            if(_target != null){
                MoveToTarget();
            }else{
                Destroy(this.gameObject);
            }
        }

        if(_startOfLife + lifetime < Time.time){
            Destroy(gameObject);
        }
    }

    public void ApplyDamageMultiplier(float multiplier){
        _damage = baseDamage * multiplier;
    }

    public void Seek(Transform target, float bulledSpeed){
        _rb = gameObject.GetComponent<Rigidbody>();
        _target = target;
        _bulletSpeed = bulledSpeed / 10;
        _seeking = true;
    }

    void MoveToTarget(){
        Vector3 pos = Vector3.MoveTowards(transform.position, _target.position, _bulletSpeed );
        _rb.MovePosition(pos);
        transform.LookAt(_target);
    }

}
