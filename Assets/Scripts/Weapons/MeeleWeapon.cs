using UnityEngine;

public class MeeleWeapon : MonoBehaviour
{
    public float damage;
    public LayerMask targetLayer;

    void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer) == targetLayer.value)
        {
            try
            {
                other.gameObject.GetComponentInParent<Creature>().TakeDamage(damage);
            }
            catch (System.Exception)
            {
                Debug.Log("No creature component found to deal damage to!");
            }
        }
    }

}
