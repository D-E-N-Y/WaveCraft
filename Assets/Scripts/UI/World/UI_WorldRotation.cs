using UnityEngine;

public class UI_WorldRotation : MonoBehaviour
{
    private Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        transform.rotation = _camera.transform.rotation;
    }
}
