using UnityEngine;

public class Raincloud : Ability
{
    [SerializeField] GameObject _cloud;
    [SerializeField] Transform _castPosition;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha4) && _allowedToCast ) {
            Cast();
        }
    }

    internal override void Behaviour(){
        GameObject currentCloud = Instantiate(_cloud, _castPosition.position, Quaternion.identity);
        Destroy(currentCloud, duration);
    }
}
