using System.Collections.Generic;
using UnityEngine;

public class RessourceBag : MonoBehaviour
{
    public GameObject ressourcePrefab;
    [Range(0f, 25)]
    public float lootExplosionForce = 3;
    public float dropRateMultiplier = 1;

    float _dropOffset = 1;

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
            GameObject drop = Instantiate(ressourcePrefab, GetRandomOffset(spawnOrigin), Quaternion.identity);
            GameObject itemModel = Instantiate(item.model, drop.transform.position, Quaternion.identity);
            itemModel.transform.parent = drop.transform;
            Vector3 direction = new Vector3(Random.Range(0f, .4f), 1, Random.Range(0f, .4f));
            drop.GetComponent<SphereCollider>().radius = item.pickupRadius;

        }
    }

    private void OnDestroy()
    {
        DropLoot(transform.position);
    }

    Vector3 GetRandomOffset(Vector3 original) {
        float xSpread = Random.Range(-_dropOffset, _dropOffset);
        float zSpread = Random.Range(-_dropOffset, _dropOffset);

        return original + new Vector3(xSpread, 0 , zSpread);
    }

}
