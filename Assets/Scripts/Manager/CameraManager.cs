using UnityEngine;

public static class CameraManager
{
    static Camera _activeCamera;

    public delegate void CameraChanged(); 
    public static event CameraChanged OnCameraChanged;

    public static Camera GetActiveCamera(){
        return _activeCamera;
    }

    public static void SetActiveCamera(Camera cam){
        _activeCamera = cam;
        OnCameraChanged?.Invoke();
    }
}
