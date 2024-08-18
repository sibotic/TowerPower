using UnityEngine;

public class Creature : Health
{
    public float moveSpeed = .5f;
    public float attackRange = 5; //for later use
    public Transform[] waypoints;

    Transform _target;
    float _distanceToTarget;
    bool _playerInRange;
    int _currentWaypoint = 0;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _target = waypoints[_currentWaypoint];
    }

    void Update()
    {
        _distanceToTarget = Vector3.Distance(_target.position, transform.position);
    }

    void FixedUpdate()
    {
        if(_distanceToTarget < 0.4f || _playerInRange){
            UpdateTarget();
        }else{
            MoveCreature();
        }
    }

    void MoveCreature()
    {
        Vector3 pos = Vector3.MoveTowards(this.transform.position, _target.position, moveSpeed / 10);
        rb.MovePosition(pos);
        transform.LookAt(_target);
    }

    void UpdateTarget(){
        //some logic to attack player
        if(_currentWaypoint + 1 >= waypoints.Length){
            Destroy(this.gameObject);
        }else{
            _currentWaypoint++;
            _target = waypoints[_currentWaypoint];
        }

    }

}
