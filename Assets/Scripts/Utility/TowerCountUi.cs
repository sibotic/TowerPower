using TMPro;
using UnityEngine;

public class TowerCountUi : MonoBehaviour
{
    [SerializeField] RuntimeSet<Tower> set;
    TMP_Text text;
    private void Start() {
        text = GetComponent<TMP_Text>();
        text.text = $"Towers placed: {set.Items.Count}";
    }

    public void CountChanged(){
        text.text = $"Towers placed: {set.Items.Count}";
    }
}

