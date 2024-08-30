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
        }
    }

    private void Update()
    {
        if (_isFollowing)
        {
            Follow();
        }
    }

    void Follow()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _playerTransform.position + Vector3.up, ref _velocity, _moveToPlayerSpeed * Time.deltaTime);
    }

}
