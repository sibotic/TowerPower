using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FloatingStatusBar : MonoBehaviour
{
    Canvas _canvas;
    Slider _slider;
    TMP_Text _txt;
    Camera _cam;
    [SerializeField] Transform _targetPosition;
    [SerializeField] float _hideDelay = 2f;
    [SerializeField] Color _barColor = Color.cyan;

    float _lastUpdate;

    private void Awake()
    {
        _canvas = GetComponentInChildren<Canvas>();
        _slider = GetComponentInChildren<Slider>();
        _txt = GetComponentInChildren<TMP_Text>();
        _canvas.enabled = false;

        _cam = CameraManager.GetActiveCamera();

        if(_targetPosition != null){
            transform.position = _targetPosition.position;
        }
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
