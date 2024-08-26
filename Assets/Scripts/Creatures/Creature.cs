using UnityEngine;

public class Creature : Health
{
    public float moveSpeed = .5f;
    public float attackRange = 5; //for later use

    [SerializeField] Transform _target;
    float _distanceToTarget, _currentTargetMaxDistance;
    bool _playerInRange;
    int _currentWaypointIndex;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _currentWaypointIndex = 0;
        _target = Waypoints.GetWaypoint(_currentWaypointIndex);
        _distanceToTarget = Vector3.Distance(_target.position, transform.position);

    }

    void Update()
    {
        //use quadratic distance instead of real one
        _distanceToTarget = Vector3.Distance(_target.position, transform.position);
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

    void UpdateTarget() //TODO maybe this is called to often and needs a cooldown? / bool
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
            _currentTargetMaxDistance = Vector3.Distance(_target.position, transform.position);
        }
    }

    public float GetProgress()
    {
        if (_distanceToTarget / _currentTargetMaxDistance > 1)
        {
            int wholeNumber = (int)(_distanceToTarget / _currentTargetMaxDistance);
            return _currentWaypointIndex - (_distanceToTarget / _currentTargetMaxDistance - wholeNumber);
        }
        else
        {
            return _currentWaypointIndex + _distanceToTarget / _currentTargetMaxDistance;
        }
    }
}
