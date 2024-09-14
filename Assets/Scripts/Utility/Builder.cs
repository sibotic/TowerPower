using UnityEngine;

public class Builder : MonoBehaviour
{
    public KeyCode buildKey = KeyCode.E;
    public Camera cam;
    public GameObject towerToBuild;
    public Vector3 positionOffset;
    public LayerMask _towerLayer;



    void Update()
    {
        if (Input.GetKeyDown(buildKey))
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPoint = hit.point;

                Tower towerScript = towerToBuild.GetComponent<Tower>();
                float neededSpace = towerScript.spaceoccupied;
                Collider[] nearbyTowers = Physics.OverlapSphere(targetPoint, neededSpace, _towerLayer);
                if (nearbyTowers.Length == 0 && towerScript.CanBeBuildHere(hit.collider.gameObject.layer))
                {
                    if (GoldManager.SpendGold(towerToBuild.GetComponent<Tower>().cost))
                    {
                        Instantiate(towerToBuild, targetPoint + positionOffset, Quaternion.identity);
                    }
                    else { Debug.Log("Tower to expensive!"); }
                }else{
                    Debug.Log($"Cannot build here! There are {nearbyTowers.Length} towers nearby and tower {towerScript.CanBeBuildHere(hit.collider.gameObject.layer)} be build here");
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
