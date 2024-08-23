using System;
using Unity.VisualScripting;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public KeyCode interact = KeyCode.E;
    public Camera cam;
    public GameObject towerToBuild;
    public Vector3 positionOffset;
    public LayerMask _towerLayer;



    void Update()
    {
        if (Input.GetKeyDown(interact))
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPoint = hit.point;

                float neededSpace = towerToBuild.GetComponent<Tower>().spaceoccupied;
                Collider[] nearbyTowers = Physics.OverlapSphere(targetPoint, neededSpace, _towerLayer);
                if (nearbyTowers.Length == 0)
                {
                    if (GoldManager.SpendGold(towerToBuild.GetComponent<Tower>().cost))
                    {
                        Instantiate(towerToBuild, targetPoint + positionOffset, Quaternion.identity);
                    }
                    else { Debug.Log("Tower to expensive!"); }
                }

            }

        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _towerLayer))
            {
                hit.transform.gameObject.GetComponentInParent<Tower>().Upgrade();
            }
        }
    }
}
