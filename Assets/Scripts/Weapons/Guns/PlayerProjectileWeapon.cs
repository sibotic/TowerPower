using UnityEngine;

public class PlayerProjectileWeapon : ProjectileWeapon
{
    [Header("Player Weapon")]
    public Camera cam;
    public bool allowButtonHold;
    [SerializeField] KeyCode fireKey = KeyCode.Mouse0;
    [SerializeField] KeyCode reloadKey = KeyCode.R;

    public override (Vector3, Transform) GetTarget()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Transform targetToSeek = null;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
            if(homingProjectiles && hit.collider.gameObject.layer == 12){
                targetToSeek = hit.transform;
            }
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        Vector3 direction = targetPoint - GetNextAttackPoint().position;


        return (direction, targetToSeek);
    }

    public override bool ShootingInput()
    {
        if (allowButtonHold) { return Input.GetKey(fireKey); }
        else { return Input.GetKeyDown(fireKey); }
    }

    public override void ManualReload()
    {
        if (Input.GetKeyDown(reloadKey) && !_reloading && _bulletsLeft < magazineSize && !_infiniteMagazine && _ammoLeft > 0)// || _shooting && _bulletsLeft <= 0)
        {
            Reload();
        }
    }
}