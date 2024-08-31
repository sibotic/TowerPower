using UnityEngine;

[CreateAssetMenu(fileName = "new Ressource", menuName = "Ressource")]
public class Ressource : ScriptableObject
{
    public new string name;
    public string description;
    public GameObject model;
    public float dropchance = 20f;
    public float pickupRadius = 10f;
}
