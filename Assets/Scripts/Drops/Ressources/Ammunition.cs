using UnityEngine;

public class Ammunition : RessourcePrefab
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ProjectileWeapon weapon = other.gameObject.GetComponentInChildren<ProjectileWeapon>();
            weapon.RefillAmmo(5);
            Destroy(this.gameObject);


        }
    }
}
