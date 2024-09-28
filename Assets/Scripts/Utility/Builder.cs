using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public KeyCode buildKey = KeyCode.E;
    public KeyCode upgradeKey = KeyCode.Q;
    public Camera cam;
    public Vector3 positionOffset;
    public LayerMask _towerLayer;
    public GameObject[] towers = new GameObject[10];

    [SerializeField] GameEvent buildModeChangedEvent;

    List<KeyCode> _keys = new List<KeyCode> { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };


    bool _inBuildMode;
    GameObject _selectedTower = null;
    public GameObject towerToBuild;
    GameObject _preview;
    Vector3 _targetPos;



    void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            GoldManager.AddGold(50000);
        }


        if (Input.GetKeyDown(buildKey))
        {
            ToggleBuildMode();
        }

        if (_inBuildMode && Input.GetKeyDown(KeyCode.Mouse0) && _selectedTower != null)
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
            {
                Vector3 targetPoint = hit.point;

                Tower towerScript = _selectedTower.GetComponent<Tower>();
                float neededSpace = towerScript.spaceoccupied;
                Collider[] nearbyTowers = Physics.OverlapSphere(targetPoint, neededSpace, _towerLayer);
                if (nearbyTowers.Length == 0 && towerScript.CanBeBuildHere(hit.collider.gameObject.layer))
                {
                    if (GoldManager.SpendGold(towerScript.cost))
                    {
                        Instantiate(_selectedTower, targetPoint + positionOffset, Quaternion.identity);
                    }
                    else { Debug.Log("Tower to expensive!"); }
                }
                else
                {
                    Debug.Log($"Cannot build here! There are {nearbyTowers.Length} towers nearby and tower {towerScript.CanBeBuildHere(hit.collider.gameObject.layer)} be build here");
                }

            }

        }

        if (Input.GetKeyDown(upgradeKey))
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _towerLayer))
            {
                Debug.Log($"Upgrading {hit}");
                hit.transform.gameObject.GetComponentInParent<Tower>().Upgrade();
            }
        }

        if (_inBuildMode)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && _selectedTower != null)
            {
                ToggleBuildMode();
            }

            for (int i = 0; i < _keys.Count; i++)
            {
                if (Input.GetKeyDown(_keys[i]))
                {
                    if (towers[i] == _selectedTower)
                    {
                        _selectedTower = null;
                        DeleteOldPreview(_preview);
                    }
                    else
                    {
                        _selectedTower = towers[i];
                        Debug.Log($"Selected tower: {_selectedTower.name}");
                        SetPreview();
                    }
                }
            }
        }

        if (_inBuildMode)
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 7));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
            {
                _targetPos = hit.point;
            }
        }


        PreviewTower();

        if ((!_inBuildMode && _preview != null) || (_selectedTower == null && _preview != null))
        {
            DeleteOldPreview(_preview);
        }

    }


    void PreviewTower()
    {
        if (_preview != null)
        {
            _preview.transform.position = _targetPos + positionOffset;
        }
    }

    void DeleteOldPreview(GameObject _prev)
    {
        Destroy(_prev); return;
    }

    void ToggleBuildMode()
    {
        _inBuildMode = !_inBuildMode;
        buildModeChangedEvent.Raise(_inBuildMode);
    }

    void SetPreview()
    {
        DeleteOldPreview(_preview);
        _preview = Instantiate(_selectedTower);
        _preview.GetComponentInChildren<CapsuleCollider>().enabled = false;
        _preview.GetComponent<Tower>().enabled = false;
    }
}
