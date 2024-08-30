using UnityEngine;

public class CloudWeapon : ProjectileWeapon
{
    [SerializeField] Transform _target;



    public override (Vector3, Transform) GetTarget()
    {
        Vector3 direction = _target.position - GetNextAttackPoint().position;
        return (direction, null);
    }

    public override bool ShootingInput()
    {
        return true;
    }

}
