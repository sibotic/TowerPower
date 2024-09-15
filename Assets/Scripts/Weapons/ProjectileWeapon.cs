using UnityEngine;
using TMPro;

enum ReloadType
{
    Manual,
    Automatic
}

public abstract class ProjectileWeapon : MonoBehaviour
{

    [Header("Projectile")]
    public GameObject bullet;
    public bool homingProjectiles;
    public float damageMultiplier = 1;
    public Transform[] attackpoints;

    [Header("Ammo")]
    [SerializeField] ReloadType reloadType;
    public float reloadTime;
    public int magazineSize;
    public int bulletsPerTap;
    public int ammoCapacity;
    public TMP_Text txtAmmo = null;

    [Header("Shooting")]
    public float timeBetweenShooting;
    public float  spread;
    public float  timeBetweenShots;
    public float  shootForce;


    internal int _bulletsLeft, _bulletsShot, _ammoLeft, _currentAttackpoint;
    internal bool _shooting, _readyToShoot, _reloading, _resetShotInvoked, _infiniteAmmo, _infiniteMagazine;

    ProjectileWeapon _scriptReference;
    (float theory, float actual) _damageDealt;



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
        _scriptReference = gameObject.GetComponent<ProjectileWeapon>();

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
        Projectile projectileScript = currentBullet.GetComponent<Projectile>();
        projectileScript.ApplyDamageMultiplier(damageMultiplier);
        projectileScript.SetOrigin(_scriptReference);

        if (homingProjectiles)
        {
            currentBullet.GetComponent<Projectile>().Seek(target.Item2, shootForce);
        }


        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * (homingProjectiles&&target.Item2!=null ? 0 : shootForce), ForceMode.Impulse);
        currentBullet.transform.forward = direction.normalized;


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

    internal abstract (Vector3, Transform) GetTarget();

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
        int reloadAmount = _ammoLeft - magazineSize - _bulletsLeft < 0 ? _ammoLeft : magazineSize - _bulletsLeft;
        _ammoLeft -= _infiniteAmmo ? 0 : reloadAmount;
        _bulletsLeft += reloadAmount;
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

    public void AddDamageDealt((float theory, float actual) values)
    {
        _damageDealt.theory += values.theory;
        _damageDealt.actual += values.actual;
    }

    public (float theory, float actual) GetDamageDealt(){
        return _damageDealt;
    }

    public bool RefillAmmo(int amountOfMagazines){
        if(_ammoLeft >= ammoCapacity){return false;}


        _ammoLeft += amountOfMagazines * magazineSize;
        if(_ammoLeft > ammoCapacity){_ammoLeft = ammoCapacity;}

        return true;
    }

}