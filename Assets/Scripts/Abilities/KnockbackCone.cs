using UnityEngine;

public class KnockbackCone : Ability
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && _allowedToCast)
        {
            Cast();
            Trigger();
        }
    }

    public override void Trigger()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward * range / 2, range, targetLayer);
        foreach (Collider collider in colliders)
        {
            try
            {
                collider.GetComponentInParent<Rigidbody>().AddForce(transform.forward * amount, ForceMode.VelocityChange);

            }
            catch (System.Exception)
            {
                Debug.Log($"Unable to find RigitBody on parent of {collider.name}");
            }
        }
    }
}
