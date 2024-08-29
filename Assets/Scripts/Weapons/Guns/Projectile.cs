using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("General")]
    public LayerMask targetLayer;

    float _startOfLife;
    Transform _target = null;
    Rigidbody _rb = null;
    ProjectileWeapon _origin;

    [Header("Bullet")]
    public float baseDamage;
    public float lifetime;
    public float bounces = 0;
    public float bounceDamageMultiplier = 1.5f;

    float _damage, _projectileSpeed;

    [Header("Hoaming")]
    public float hoamingRadius = 5f;

    float _searchCooldown = .5f;
    float _lastSearched = 0f;

    bool _seeking, _searchForTarget;

    [Header("Explosion")]
    public float explosionRadius = 0;
    public float explosionDamage = 0;


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
                if (_origin != null)
                {
                    _origin.AddDamageDealt(damageDealt);

                }

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
        if (_seeking && !_searchForTarget)
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

        if (_searchForTarget && _lastSearched < Time.time)
        {
            SearchForTarget();
            _lastSearched = Time.time + _searchCooldown;
        }
    }

    public void ApplyDamageMultiplier(float multiplier)
    {
        _damage = baseDamage * multiplier;
        explosionDamage = explosionDamage * (multiplier / 2);
    }

    public void Seek(Transform target, float bulledSpeed)
    {
        if (target != null)
        {
            _target = target;
        }
        else
        {
            _searchForTarget = true;
        }


        _rb = gameObject.GetComponent<Rigidbody>();
        _projectileSpeed = bulledSpeed / 5;
        _seeking = true;
    }

    void MoveToTarget()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, _target.position, _projectileSpeed);
        _rb.MovePosition(pos);
        transform.LookAt(_target);
    }

    void SearchForTarget()
    {
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, hoamingRadius, targetLayer);
        float _closestDistance = Mathf.Infinity;
        Transform newTarget = null;

        foreach (Collider target in targetsInRange)
        {
            float distanceToTarget = (target.transform.position - transform.position).sqrMagnitude;
            if (distanceToTarget < _closestDistance) { _closestDistance = distanceToTarget; }
            newTarget = target.transform;
        }

        if (newTarget != null)
        {
            _target = newTarget;
            _rb.velocity = Vector3.zero;
            _searchForTarget = false;
        }
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

    public void SetOrigin(ProjectileWeapon origin)
    {
        _origin = origin;
    }

    public void SetTargetLayer(LayerMask newTargetLayer)
    {
        targetLayer = newTargetLayer;
    }

}
