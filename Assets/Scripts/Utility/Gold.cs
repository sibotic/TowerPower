using UnityEngine;

public class Gold : RessourcePrefab
{
    [SerializeField] float _amount;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GoldManager.AddGold(_amount);

            Destroy(this.gameObject);
        }
    }
}
