using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    Canvas _canvas;
    Slider _slider;
    TMP_Text _txt;
    [SerializeField] Camera _cam;
    [SerializeField] Transform _target;
    [SerializeField] float _hideDelay = 2f;

    float _lastUpdate;

    private void Awake()
    {
        _cam = GameObject.FindWithTag("Player").GetComponentInChildren<Camera>();
        _canvas = GetComponentInChildren<Canvas>();
        _slider = GetComponentInChildren<Slider>();
        _txt = GetComponentInChildren<TMP_Text>();
        _canvas.enabled = false;
    }

    public void UpdateStatusBar(float currentValue, float maxValue)
    {
        _lastUpdate = Time.time;
        _canvas.enabled = true;
        _slider.value = currentValue / maxValue;
        _txt.text = $"{currentValue}";
    }

    private void Update()
    {
        if(_lastUpdate + _hideDelay < Time.time){
            _canvas.enabled = false;
        }else {
            _canvas.transform.rotation = _cam.transform.rotation;
        }
    }
}
