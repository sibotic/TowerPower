using UnityEngine;

public static class CameraManager
{
    static Camera _activeCamera;

    public static Camera GetActiveCamera(){
        return _activeCamera;
    }

    public static void SetActiveCamera(Camera cam){
        _activeCamera = cam;
    }
}
