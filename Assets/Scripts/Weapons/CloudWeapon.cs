using UnityEngine;

public class CloudWeapon : ProjectileWeapon
{
    [SerializeField] Transform _target;



    internal override (Vector3, Transform) GetTarget()
    {
        Vector3 direction = _target.position - GetNextAttackPoint().position;
        return (direction, null);
    }

    public override bool ShootingInput()
    {
        return true;
    }

}
