using UnityEngine;

public class MeeleAttack : MonoBehaviour
{
    public float attackCooldown;
    public float attackDuration;
    public KeyCode meeleAttackButton = KeyCode.F;
    public GameObject meeleWeapon;
    [SerializeField] Transform attackPos;

    float _lastAttack;
    GameObject weapon;

    void Update() {
        if(Input.GetKey(meeleAttackButton) && _lastAttack + attackCooldown < Time.time){
            Attack();
        }
    }

    void Attack()
    {
        _lastAttack = Time.time;
        weapon = Instantiate(meeleWeapon, attackPos.position, Quaternion.identity, transform); 
        Invoke("StopAttack", attackDuration);
    }

    void StopAttack()
    {
        Destroy(weapon);
    }
}