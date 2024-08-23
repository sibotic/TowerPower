using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float baseDamage, lifetime;
    public float bounces = 0, bounceDamageMultiplier = 1.5f;
    public float explosionRadius = 0, explosionDamage = 0;
    public LayerMask targetLayer;

    float _damage, _bulletSpeed, _startOfLife;
    bool _seeking;
    Transform _target = null;
    Rigidbody _rb = null;
    ProjectileWeapon _origin;

    private void Start()
    {
        _startOfLife = Time.time;
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer) == targetLayer.value) //1 << converts the layerMask index into a LayerMask (bitmap)
        {
            if (collision.gameObject.TryGetComponent<Creature>(out Creature creature))
            {
                (float, float) damageDealt = creature.TakeDamage(_damage);
                _origin.AddDamageDealt(damageDealt);
            }
        }

        if (explosionRadius > 0)
        {
            Explode();
        }
        
        if (bounces <= 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            bounces--;
            _damage *= bounceDamageMultiplier;
        }


    }

    void FixedUpdate()
    {
        if (_seeking)
        {
            if (_target != null)
            {
                MoveToTarget();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        if (_startOfLife + lifetime < Time.time)
        {
            Destroy(gameObject);
        }
    }

    public void ApplyDamageMultiplier(float multiplier)
    {
        //Should explosions be affected by damageMultiplier?
        _damage = baseDamage * multiplier;
    }

    public void Seek(Transform target, float bulledSpeed)
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _target = target;
        _bulletSpeed = bulledSpeed / 10;
        _seeking = true;
    }

    void MoveToTarget()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, _target.position, _bulletSpeed);
        _rb.MovePosition(pos);
        transform.LookAt(_target);
    }

    void Explode()
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, explosionRadius, targetLayer);


        foreach (Collider collision in collisions)
        {
            try
            {
                (float, float) damageDealt = collision.GetComponentInParent<Creature>().TakeDamage(explosionDamage);

                _origin.AddDamageDealt(damageDealt);
            }
            catch (System.Exception)
            {
                Debug.Log("No 'Creature' Component found on parent");
            }
        }
    }

    public void SetOrigin(ProjectileWeapon origin){
        _origin = origin;
    }
}
