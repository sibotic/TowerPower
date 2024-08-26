using UnityEngine;


enum TargetType
{
    // First,
    // Last,
    Close,
    Far
    // Strong,
    // Weak,
    // Low, //HP
    // High //HP
}

public class Tower : ProjectileWeapon
{
    //maybe add range type so later there can be stuff with min ranges, attack the air or not etc
    [Header("Tower Stats")]
    public float range;
    public float turnSpeed;
    public float cost;
    public float spaceoccupied = 2;
    public float updateTarget = .5f;
    public LayerMask targetLayer;
    [SerializeField] TargetType _targetType = TargetType.Close;
    [SerializeField] LayerMask[] _buildableLayers;
    [SerializeField] GameObject upgrade;

    Transform _targetEnemy = null;
    Vector3 _lastTarget;

    private void Start()
    {
        _lastTarget = transform.position;
        InvokeRepeating("UpdateTargetEnemy", 0, updateTarget);
    }

    void UpdateTargetEnemy()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, range, targetLayer);
        float _bestDistance;


        switch (_targetType)
        {
            case TargetType.Close:
                _bestDistance = Mathf.Infinity;
                break;
            case TargetType.Far:
                _bestDistance = 0;
                break;
            default:
                _bestDistance = 0;
                break;
        }


        GameObject _bestEnemy = null;

        foreach (Collider enemy in targets)
        {
            float _distanceToEnemy = (enemy.transform.position - transform.position).sqrMagnitude;

            switch (_targetType)
            {
                case TargetType.Close:
                    if (_distanceToEnemy < _bestDistance)
                    {
                        _bestDistance = _distanceToEnemy;
                        _bestEnemy = enemy.gameObject;

                    }
                    break;
                case TargetType.Far:
                    if (_distanceToEnemy > _bestDistance)
                    {
                        _bestDistance = _distanceToEnemy;
                        _bestEnemy = enemy.gameObject;

                    }
                    break;
            }



        }

        if (_bestEnemy != null)
        {
            _targetEnemy = _bestEnemy.transform;
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

    public bool CanBeBuildHere(LayerMask layer)
    {
        foreach (var buildableLayer in _buildableLayers)
        {
            if (buildableLayer.value == (1 << layer.value))
            {
                return true;
            }
        }
        return false;
    }
}