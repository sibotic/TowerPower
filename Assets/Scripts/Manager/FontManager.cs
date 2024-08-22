using UnityEngine;
using TMPro;

public class FontManager : MonoBehaviour
{
    [SerializeField] TMP_Text _currentGold;


    private void Start()
    {
        GoldManager.goldChanged.AddListener(OnGoldChanged);
        OnGoldChanged();
    }

    void OnGoldChanged()
    {
        _currentGold.text = $"{GoldManager.GetCurrentGold()}$"; 
    }
}
