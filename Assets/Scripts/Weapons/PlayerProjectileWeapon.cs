using UnityEngine;

public class PlayerProjectileWeapon : ProjectileWeapon
{
    public Camera cam;
    public bool allowButtonHold;
    [SerializeField]
    KeyCode fireKey = KeyCode.Mouse0;
    KeyCode reloadKey = KeyCode.R;

    public override Vector3 GetTarget()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        Vector3 directionWithoutSpread = targetPoint - attackpoint.position;

        float xSpread = Random.Range(-spread, spread);
        float ySpread = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(xSpread, ySpread, 0);

        return directionWithSpread;
    }

    public override bool ShootingInput()
    {
        if (allowButtonHold) { return Input.GetKey(fireKey); }
        else { return Input.GetKeyDown(fireKey); }
    }

    public override void ManualReload()
    {
        if (Input.GetKeyDown(reloadKey) && !_reloading && _bulletsLeft != magazineSize && !_infiniteAmmo)// || _shooting && _bulletsLeft <= 0)
        {
            Reload();
        }
    }
}