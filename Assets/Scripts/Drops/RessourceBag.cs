using System.Collections.Generic;
using UnityEngine;

public class RessourceBag : MonoBehaviour
{
    public GameObject ressourcePrefab;
    [Range(0f, 25)]
    public float lootExplosionForce = 3;
    public float dropRateMultiplier = 1;

    public List<Ressource> dropTable = new List<Ressource>();

    List<Ressource> GetRessourcesToDrop()
    {
        List<Ressource> items = new List<Ressource>();
        foreach (Ressource item in dropTable)
        {
            int randNumb = Random.Range(1, 101);
            if (randNumb <= item.dropchance * dropRateMultiplier)
            {
                items.Add(item);
            }
        }
        return items;
    }

    public void DropLoot(Vector3 spawnOrigin)
    {
        List<Ressource> itemDrops = GetRessourcesToDrop();
        if (itemDrops.Count == 0) { return; }

        foreach (Ressource item in itemDrops)
        {
            GameObject drop = Instantiate(ressourcePrefab, spawnOrigin, Quaternion.identity);
            GameObject itemModel = Instantiate(item.model, spawnOrigin, Quaternion.identity);
            itemModel.transform.parent = drop.transform;
            Vector3 direction = new Vector3(Random.Range(0f, .4f), 1, Random.Range(0f, .4f));
            drop.GetComponent<SphereCollider>().radius = item.pickupRadius;

        }
    }

    private void OnDestroy()
    {
        DropLoot(transform.position);
    }

}
