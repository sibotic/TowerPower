using TMPro;
using UnityEngine;

public class EnemyCountUi : MonoBehaviour
{
    [SerializeField] RuntimeSet<Creature> set;
    TMP_Text text;
    private void Start() {
        text = GetComponent<TMP_Text>();
        text.text = $"Enemies left: {set.Items.Count}";
    }

    public void CountChanged(){
        text.text = $"Enemies left: {set.Items.Count}";
    }
}
