using UnityEngine;

public class Waypoints : MonoBehaviour
{

    static Transform[] _waypoints;

    void Awake()
    {
        _waypoints = new Transform[transform.childCount];

        for (int i = 0; i < _waypoints.Length; i++)
        {
            _waypoints[i] = transform.GetChild(i);
        }
    }

    public static Transform GetWaypoint(int index)
    {
        try
        {
            return _waypoints[index];

        }
        catch (System.Exception)
        {

            return null;
        }
    }

}
