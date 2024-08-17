using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{

    [SerializeField]
    GameObject bullet;

    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots, shootForce;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    public Camera cam;
    public Transform attackpoint;

    int bulletsLeft, bulletsShot;
    bool shooting, readyToShoot, reloading;


    void Awake() {
        bulletsLeft = magazineSize;
        readyToShoot = true; 
    }

    void Update(){
        if (allowButtonHold) {shooting = Input.GetKey(KeyCode.Mouse0);}
        else{shooting = Input.GetKeyDown(KeyCode.Mouse0); }

        if(readyToShoot && shooting && !reloading && bulletsLeft > 0) {
            bulletsShot = 0;

            Shoot();
        }
    }

    void Shoot(){
        readyToShoot = false;

        bulletsLeft--;
        bulletsShot++;

        Vector3 direction = CalculateTarget();

        GameObject currentBullet = Instantiate(bullet, attackpoint.position, Quaternion.identity);
        currentBullet.transform.forward = direction.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);

        Invoke("ResetShot", 1);

    }

    Vector3 CalculateTarget(){
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit)){
            targetPoint = hit.point;
        }else {
            targetPoint = ray.GetPoint(75);
        }

        Vector3 directionWithoutSpread = targetPoint - attackpoint.position;

        float xSpread = Random.Range(-spread, spread);
        float ySpread = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(xSpread, ySpread, 0);

        return directionWithSpread;
    }

    void ResetShot(){
        readyToShoot = true;
    }
}
