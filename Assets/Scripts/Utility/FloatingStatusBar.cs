using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingStatusBar : MonoBehaviour
{
    Canvas _canvas;
    Slider _slider;
    TMP_Text _txt;
    Camera _targetCamera;
    [SerializeField] Transform _targetPosition;
    [SerializeField] float _hideDelay = 2f; 
    [SerializeField] ColorVariable _background;
    [SerializeField] ColorVariable _foreground;

    float _lastUpdate;

    private void OnEnable() {
        CameraManager.OnCameraChanged += OnCameraChange;
    }
    private void OnDisable() {
        CameraManager.OnCameraChanged -= OnCameraChange;
    }

    private void Awake()
    {
        _canvas = GetComponentInChildren<Canvas>();
        _slider = GetComponentInChildren<Slider>();
        _txt = GetComponentInChildren<TMP_Text>();

        Image[] images = _slider.GetComponentsInChildren<Image>();
        images[0].color = _background.Value;
        images[1].color = _foreground.Value;
        _canvas.enabled = false;

        if(_targetPosition != null){
            transform.position = _targetPosition.position;
        }
        OnCameraChange();
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
            _canvas.transform.rotation = _targetCamera.transform.rotation;
        }
    }

    void OnCameraChange(){
        _targetCamera = CameraManager.GetActiveCamera();
    }
}
