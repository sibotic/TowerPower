using UnityEngine;

public class Tower : ProjectileWeapon
{
    //maybe add range type so later there can be stuff with min ranges, attack the air or not etc
    [Header("Tower Stats")]
    public float range;
    public float turnSpeed;
    public float cost;
    public float spaceoccupied = 2;
    public LayerMask targetLayer;
    [SerializeField] float _updateTargetInterval = .5f;
    [SerializeField] TargetType _targetType = TargetType.First;
    [SerializeField] LayerMask[] _buildableLayers;
    [SerializeField] GameObject upgrade;

    [Header("Runtime Sets")]
    [SerializeField] TowerRuntimeSet _activeTowers;

    Transform _targetEnemy = null;
    Vector3 _lastTarget;

    private void OnEnable() {
        _activeTowers.Add(this);
    }

    private void OnDisable() {
        _activeTowers.Remove(this);
    }

    private void Start()
    {
        _lastTarget = transform.position;
        InvokeRepeating("UpdateTargetEnemy", 0, _updateTargetInterval);
    }

    void UpdateTargetEnemy()
    {
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, range, targetLayer);
        float _comparingValue;

        switch (_targetType)
        {
            case TargetType.Close:
                _comparingValue = Mathf.Infinity;
                break;
            case TargetType.Far:
                _comparingValue = 0;
                break;
            case TargetType.First:
                _comparingValue = 0;
                break;
            case TargetType.Last:
                _comparingValue = Mathf.Infinity;
                break;
            case TargetType.Slow:
                _comparingValue = Mathf.Infinity;
                break;
            case TargetType.Fast:
                _comparingValue = 0;
                break;
            case TargetType.Low:
                _comparingValue = Mathf.Infinity;
                break;
            case TargetType.High:
                _comparingValue = 0;
                break;
            default:
                _comparingValue = 0;
                break;
        }


        GameObject _bestEnemy = null;

        foreach (Collider enemy in potentialTargets)
        {
            float _distanceToEnemy;
            float _progressOfEnemy;
            float _healthOfEnemy;
            float _speedOfEnemy;
            try
            {
            switch (_targetType)
            {
                case TargetType.Close:
                    _distanceToEnemy = (enemy.transform.position - transform.position).sqrMagnitude;
                    if (_distanceToEnemy < _comparingValue)
                    {
                        _comparingValue = _distanceToEnemy;
                        _bestEnemy = enemy.gameObject;

                    }
                    break;
                case TargetType.Far:
                    _distanceToEnemy = (enemy.transform.position - transform.position).sqrMagnitude;
                    if (_distanceToEnemy > _comparingValue)
                    {
                        _comparingValue = _distanceToEnemy;
                        _bestEnemy = enemy.gameObject;

                    }
                    break;
                case TargetType.First:
                    _progressOfEnemy = enemy.gameObject.GetComponentInParent<Creature>().GetProgress();
                    if (_progressOfEnemy > _comparingValue)
                    {
                        _comparingValue = _progressOfEnemy;
                        _bestEnemy = enemy.gameObject;

                    }
                    break;
                case TargetType.Last:
                    _progressOfEnemy = enemy.gameObject.GetComponentInParent<Creature>().GetProgress();
                    if (_progressOfEnemy < _comparingValue)
                    {
                        _comparingValue = _progressOfEnemy;
                        _bestEnemy = enemy.gameObject;

                    }
                    break;
                case TargetType.Slow:
                    _speedOfEnemy = enemy.gameObject.GetComponentInParent<Creature>().moveSpeed;
                    if (_speedOfEnemy < _comparingValue)
                    {
                        _comparingValue = _speedOfEnemy;
                        _bestEnemy = enemy.gameObject;

                    }
                    break;
                case TargetType.Fast:
                    _speedOfEnemy = enemy.gameObject.GetComponentInParent<Creature>().moveSpeed;
                    if (_speedOfEnemy > _comparingValue)
                    {
                        _comparingValue = _speedOfEnemy;
                        _bestEnemy = enemy.gameObject;

                    }
                    break;
                case TargetType.Low:
                    _healthOfEnemy = enemy.gameObject.GetComponentInParent<Creature>().GetCurrentHealth();
                    if (_healthOfEnemy < _comparingValue)
                    {
                        _comparingValue = _healthOfEnemy;
                        _bestEnemy = enemy.gameObject;

                    }
                    break;
                case TargetType.High:
                    _healthOfEnemy = enemy.gameObject.GetComponentInParent<Creature>().GetCurrentHealth();
                    if (_healthOfEnemy > _comparingValue)
                    {
                        _comparingValue = _healthOfEnemy;
                        _bestEnemy = enemy.gameObject;

                    }
                    break;
            }

            }
            catch (System.Exception)
            {
                Debug.Log("Issue updating Target!" );
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

    internal override (Vector3, Transform) GetTarget()
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