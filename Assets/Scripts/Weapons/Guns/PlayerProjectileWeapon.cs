using UnityEngine;

public class PlayerProjectileWeapon : ProjectileWeapon
{
    [Header("Player Weapon")]
    public Camera cam;
    public bool allowButtonHold;
    [SerializeField] KeyCode fireKey = KeyCode.Mouse0;
    [SerializeField] KeyCode reloadKey = KeyCode.R;

    internal override (Vector3, Transform) GetTarget()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Transform targetToSeek = null;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
        {
            targetPoint = hit.point;
            if (homingProjectiles && hit.collider.gameObject.layer == 12)
            {
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
        if ((Input.GetKeyDown(reloadKey) && !_reloading && _bulletsLeft < magazineSize && !_infiniteMagazine && _ammoLeft > 0)
        || (_bulletsLeft <= 0 && _ammoLeft > 0 && !_reloading))
        {
            Reload();
        }
    }

    public void ToggleActive(){
        enabled = !enabled;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = new Color(1, 1, 0, .2f);

    //     Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    //     RaycastHit hit;
    //     if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
    //     {
    //         Gizmos.DrawSphere(hit.point, .5f);
    //     }

    //     Gizmos.DrawRay(transform.position, ray.direction * 50);

    // }
}