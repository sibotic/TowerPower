using UnityEngine;

public class Fireball : Ability
{
    [SerializeField] GameObject _projectile;
    [SerializeField] Transform _castPosition;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha3) && _allowedToCast ) {
            Cast();
        }
    }

    internal override void Behaviour(){
        GameObject currentProjectile = Instantiate(_projectile, _castPosition.position, Quaternion.identity);
        Projectile projectileScript = currentProjectile.GetComponent<Projectile>();
        projectileScript.ApplyDamageMultiplier(amount);
        projectileScript.explosionRadius = range;
        

        currentProjectile.GetComponent<Rigidbody>().AddForce(transform.forward.normalized, ForceMode.Impulse);

    }
}
