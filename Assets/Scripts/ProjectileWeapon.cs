using UnityEngine;
using TMPro;

enum ReloadType
{
    Manual,
    Automatic
}

public abstract class ProjectileWeapon : MonoBehaviour
{
    public GameObject bullet;

    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots, shootForce;
    public float damageMultiplier = 1;
    public int magazineSize, bulletsPerTap, ammoCapacity;
    public bool homingProjectiles;
    public Transform[] attackpoints;
    public TMP_Text txtAmmo = null;

    [SerializeField]
    ReloadType reloadType;
    internal int _bulletsLeft, _bulletsShot, _ammoLeft, _currentAttackpoint;
    internal bool _shooting, _readyToShoot, _reloading, _resetShotInvoked, _infiniteAmmo, _infiniteMagazine;


    void Awake()
    {
        if (magazineSize == 0)
        {
            _infiniteMagazine = true;
        }
        if (ammoCapacity == 0)
        {
            _infiniteAmmo = true;
        }


        _bulletsLeft = _infiniteMagazine ? 1 : magazineSize;
        _readyToShoot = true;
        _ammoLeft = ammoCapacity == 0 ? magazineSize : ammoCapacity;

        UpdateTxt();

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
        UpdateTxt();
    }

    public virtual void ManualReload() { }
    public void AutomaticReload()
    {
        //TODO
    }

    public abstract bool ShootingInput();

    void Shoot()
    {
        _readyToShoot = false;
        (Vector3, Transform) target = GetTarget();
        Vector3 directionWithoutSpread = target.Item1;
        Vector3 direction = ApplySpread(directionWithoutSpread);



        GameObject currentBullet = Instantiate(bullet, attackpoints[_currentAttackpoint].position, Quaternion.identity);
        currentBullet.GetComponent<Projectile>().ApplyDamageMultiplier(damageMultiplier);

        if (homingProjectiles)
        {
            currentBullet.GetComponent<Projectile>().Seek(target.Item2, shootForce);
        }

        currentBullet.transform.forward = direction.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * (homingProjectiles ? 0 : shootForce), ForceMode.Impulse);


        if (!_infiniteMagazine)
        {
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

    public abstract (Vector3, Transform) GetTarget();

    void ResetShot()
    {
        _readyToShoot = true;
        _resetShotInvoked = false;
    }

    internal void Reload()
    {
        _reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    void ReloadFinished()
    {
        int reloadAmount = _ammoLeft - magazineSize < 0 ? _ammoLeft : magazineSize;
        _ammoLeft -= _infiniteAmmo ? 0 : reloadAmount;
        _bulletsLeft = reloadAmount;
        _reloading = false;
    }

    Vector3 ApplySpread(Vector3 directionWithoutSpread)
    {
        float xSpread = Random.Range(-spread, spread) / 10;
        float ySpread = Random.Range(-spread, spread) / 10;

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(xSpread, ySpread, 0);
        return directionWithSpread;
    }

    internal Transform GetNextAttackPoint()
    {
        if (_currentAttackpoint + 1 < attackpoints.Length)
        {
            _currentAttackpoint++;
        }
        else
        {
            _currentAttackpoint = 0;
        }
        return attackpoints[_currentAttackpoint];
    }

    void UpdateTxt()
    {
        if (txtAmmo != null)
        {
            txtAmmo.text = _reloading ? "Reloading .." : $"{_bulletsLeft / bulletsPerTap}/{magazineSize / bulletsPerTap} ({_ammoLeft})";

        }
    }
}