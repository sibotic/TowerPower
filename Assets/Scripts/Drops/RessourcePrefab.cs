using UnityEngine;

public class RessourcePrefab : MonoBehaviour
{
    [SerializeField] float _moveToPlayerSpeed = 100;
    bool _isFollowing;
    Transform _playerTransform;
    Vector3 _velocity = Vector3.zero;

    private void Start()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isFollowing = true;
            InvokeRepeating(nameof(IncreaseFollowSpeed), .25f, .25f);
        }
    }

    private void Update()
    {
        if (_isFollowing)
        {
            Follow();
        }
    }

    internal void Follow()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _playerTransform.position + Vector3.up / 2, ref _velocity, _moveToPlayerSpeed * Time.deltaTime);
    }

    void IncreaseFollowSpeed(){
        _moveToPlayerSpeed *= 0.9f;
    }

}
