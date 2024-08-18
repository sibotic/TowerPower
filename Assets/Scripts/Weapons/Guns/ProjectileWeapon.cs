using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    public GameObject bullet;

    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots, shootForce;
    public float damageMultiplier = 1;
    public int magazineSize, bulletsPerTap, ammoCapacity;
    public bool allowButtonHold;
    public Camera cam;
    public Transform attackpoint;

    int _bulletsLeft, _bulletsShot, _ammoLeft;
    bool _shooting, _readyToShoot, _reloading, _resetShotInvoked;


    void Awake()
    {
        _bulletsLeft = magazineSize;
        _readyToShoot = true;
        _ammoLeft = ammoCapacity;
    }

    void Update()
    {

        //Reeloading
        if (Input.GetKeyDown(KeyCode.R) && !_reloading && _bulletsLeft != magazineSize || _bulletsLeft == 0 && _ammoLeft > 0 && !_reloading)
        {
            Reload();
        }

        //Shooting
        if (allowButtonHold) { _shooting = Input.GetKey(KeyCode.Mouse0); }
        else { _shooting = Input.GetKeyDown(KeyCode.Mouse0); }

        if (_readyToShoot && _shooting && !_reloading && _bulletsLeft > 0)
        {
            _bulletsShot = 0;
            Shoot();
        }

    }

    void Shoot()
    {
        _readyToShoot = false;

        Vector3 direction = CalculateTarget();

        GameObject currentBullet = Instantiate(bullet, attackpoint.position, Quaternion.identity);
        currentBullet.GetComponent<Projectile>().SetDamage(damageMultiplier);

        currentBullet.transform.forward = direction.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);


        _bulletsLeft--;
        _bulletsShot++;

        if (!_resetShotInvoked)
        {
            Invoke("ResetShot", timeBetweenShooting);
            _resetShotInvoked = true;
        }

        if (_bulletsShot < bulletsPerTap && _bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    Vector3 CalculateTarget()
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

    void ResetShot()
    {
        _readyToShoot = true;
        _resetShotInvoked = false;
    }

    void Reload()
    {
        Debug.Log("Reloadin. . .");
        _reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    void ReloadFinished()
    {
        int reloadAmount = _ammoLeft - magazineSize < 0 ? _ammoLeft : magazineSize;
        _ammoLeft -= reloadAmount;
        _bulletsLeft = reloadAmount;
        _reloading = false;
        Debug.Log("Reload Finished!");
    }
}
