using UnityEngine;

public class Creature : Health
{
    public float moveSpeed = .5f;
    public float attackRange = 5; //for later use

    [SerializeField] Transform _target;
    float _distanceToTarget, _currentTargetMaxDistance;
    bool _playerInRange;
    int _currentWaypointIndex = 0;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _target = Waypoints.GetWaypoint(_currentWaypointIndex);
        _distanceToTarget = Vector3.Distance(_target.position, transform.position);
        _currentTargetMaxDistance = _distanceToTarget;
    }

    void Update()
    {
        _distanceToTarget = (_target.position - transform.position).sqrMagnitude;
    }

    void FixedUpdate()
    {
        if (_distanceToTarget < 0.3f || _playerInRange)
        {
            UpdateTarget();
        }
        else
        {
            MoveCreature();
        }
    }

    void MoveCreature()
    {
        Vector3 pos = Vector3.MoveTowards(this.transform.position, _target.position, moveSpeed / 10);
        rb.MovePosition(pos);
        transform.LookAt(_target);
    }

    void UpdateTarget()
    {
        //some logic to attack player
        Transform nextWaypoint = Waypoints.GetWaypoint(_currentWaypointIndex + 1);
        if (nextWaypoint == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _currentWaypointIndex++;
            _target = nextWaypoint;
            _currentTargetMaxDistance = (_target.position - transform.position).sqrMagnitude;
        }
    }

    public float GetProgress()
    {
        float progress;
        float ratio = _distanceToTarget / _currentTargetMaxDistance;
        if (ratio > 1)
        {
            int wholeNumber = (int)ratio;
            progress = _currentWaypointIndex - (ratio - wholeNumber);
        }
        else
        {
            progress = _currentWaypointIndex + (1 - ratio);
        }
        return progress;
    }
}
