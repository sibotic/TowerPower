using UnityEngine;

enum ReloadType {
    Manual,
    Automatic
}

public abstract class ProjectileWeapon : MonoBehaviour
{
    public GameObject bullet;

    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots, shootForce;
    public float damageMultiplier = 1;
    public int magazineSize, bulletsPerTap, ammoCapacity;
    public Transform attackpoint;

    [SerializeField]
    ReloadType reloadType;
    internal int _bulletsLeft, _bulletsShot, _ammoLeft;
    internal bool _shooting, _readyToShoot, _reloading, _resetShotInvoked, _infiniteAmmo;


    void Awake()
    {
        if (ammoCapacity == 0){
            _infiniteAmmo = true;
        }
        _bulletsLeft = magazineSize;
        _readyToShoot = true;
        _ammoLeft = ammoCapacity;

    } 

    void Update()
    {
        switch (reloadType)
        {
            case ReloadType.Manual:
                ManualReload();
                break;
            case ReloadType.Automatic:
                AutomaticReload();
                break;
            default:
                break;
        }


        //Shooting
        _shooting = ShootingInput();

        if (_readyToShoot && _shooting && !_reloading && _bulletsLeft > 0)
        {
            _bulletsShot = 0;
            Shoot();
        }

    }

    public virtual void ManualReload(){}
    public void AutomaticReload(){
        //TODO

    }

    public abstract bool ShootingInput();

    void Shoot()
    {
        _readyToShoot = false;

        Vector3 direction = GetTarget();

        GameObject currentBullet = Instantiate(bullet, attackpoint.position, Quaternion.identity);
        currentBullet.GetComponent<Projectile>().SetDamage(damageMultiplier);

        currentBullet.transform.forward = direction.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);


        if(!_infiniteAmmo){
            _bulletsLeft--;
        }

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

    public abstract Vector3 GetTarget();

    void ResetShot()
    {
        _readyToShoot = true;
        _resetShotInvoked = false;
    }

    internal void Reload()
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