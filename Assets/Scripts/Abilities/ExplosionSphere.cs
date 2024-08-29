using UnityEngine;

public class ExplosionSphere : Ability
{
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha2) && _allowedToCast ) {
            Cast();
        }
    }

    internal override void Behaviour(){
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, targetLayer);
        foreach(Collider collider in colliders) {
            try
            {
                collider.GetComponentInParent<Creature>().TakeDamage(amount);
            }
            catch (System.Exception)
            {
                Debug.Log($"No Creature Component found in {collider.name}");
            }
        }
    }
}
