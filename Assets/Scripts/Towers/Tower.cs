using UnityEngine;

public class Tower : ProjectileWeapon
{
    public float range, turnSpeed, cost;
    public string enemyTag = "Enemy"; //might allow to have an enemy that makes towers shoot the player
    public LayerMask enemyLayer;
    public float spaceoccupied = 2;
    [SerializeField] GameObject upgrade;
    //maybe add range type so later there can be stuff with min ranges, attack the air or not etc
    Transform _targetEnemy = null;
    Vector3 _lastTarget;

    private void Start()
    {
        _lastTarget = transform.position;
        InvokeRepeating("UpdateTargetEnemy", 0, .5F);
    }

    // private void Update()
    // {
    //     Debug.Log(_shooting);
    //     if (_targetEnemy == null) { return; }
    // }

    void UpdateTargetEnemy()
    {
        //change to use OverlapSpheres to get all enemies inside the sphgere, not in the entire game
        //can also implement stuff like first or last enemy with this later on

        // GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        Collider[] enemies = Physics.OverlapSphere(transform.position, range, enemyLayer);
        float _shortestDistance = Mathf.Infinity;
        GameObject _nearestEnemy = null;
        foreach (Collider enemy in enemies)
        {
            float _distanceToEnemy = Vector3.Distance(enemy.transform.position, this.transform.position);
            if (_distanceToEnemy < _shortestDistance)
            {
                _shortestDistance = _distanceToEnemy;
                _nearestEnemy = enemy.gameObject;
            }
        }

        if (_nearestEnemy != null && _shortestDistance <= range)
        {
            _targetEnemy = _nearestEnemy.transform;
        }
        else
        {
            _targetEnemy = null;
        }
    }

    public override (Vector3, Transform) GetTarget()
    {
        Vector3 target;
        try
        {
            target = (_targetEnemy.position - GetNextAttackPoint().position).normalized;
            _lastTarget = target;
        }
        catch (System.Exception)
        {
            target = _lastTarget;
        }

        return (target, homingProjectiles ? _targetEnemy : null);
    }

    public override bool ShootingInput()
    {
        return _targetEnemy == null ? false : true;
    }

    public override void ManualReload()
    {
        if (_bulletsLeft <= 0 && !_reloading)
        {
            Reload();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, .2f);
        Gizmos.DrawSphere(transform.position, range);
    }

    public void Upgrade()
    {
        if (upgrade == null)
        {
            return;
        }

        if (GoldManager.SpendGold(upgrade.GetComponent<Tower>().cost))
        {
            Instantiate(upgrade, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

    }
}