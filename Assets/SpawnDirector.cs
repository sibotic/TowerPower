using UnityEngine;

public class SpawnDirector : MonoBehaviour
{
    public GameObject[] creatures;

    float _lastSpawn, _lastBreak, _spawnInterval = 1f, _breakInterval = 10f, _breakDuration = 5f;

    private void Start()
    {
        _lastSpawn = Time.time;
        _lastBreak = Time.time;
    }

    void Update()
    {
        if (_lastBreak + _breakInterval < Time.time)
        {

            if (_breakInterval > 1)
            {
                _breakInterval -= 0.2f;
                if(_breakDuration > 1){
                    _breakDuration -= .1f;
                }
            }
            if (_spawnInterval > .5f)
            {
                _spawnInterval -= 0.1f;
            }

            _lastSpawn = Time.time + _breakDuration;
            _lastBreak = Time.time + _breakDuration;
        }


        if (_lastSpawn + _spawnInterval < Time.time)
        {
            SpawnEnemie();
        }


    }

    void SpawnEnemie()
    {
        int randIndex = Random.Range(0, creatures.Length);
        Instantiate(creatures[randIndex], transform.position, Quaternion.identity);
        _lastSpawn = Time.time;

    }
}
