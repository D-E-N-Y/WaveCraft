using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    private Camera _camera;
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementTime;
    [SerializeField] private float rotationAmount;
    [SerializeField] private float zoomAmount;
    
    private Vector3 newPosition;
    private Quaternion newRotation;
    private Vector3 newZoom;

    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;
    private Vector3 rotateStartPosition;
    private Vector3 rotateCurrentPosition;

    private UniversalRenderPipelineAsset urpAsset;

    private void Start() 
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;

        _camera = cameraTransform.gameObject.GetComponent<Camera>();

        urpAsset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
    }

    void OnDisable()
    {
        urpAsset.shadowDistance = 800f;
    }

    private void Update() 
    {
        HandleMouseInput();
        HandleMovementInput();
        
        SetNearCamera();
        SetShadowDistance();
        Control();
    }

    private float nearFactor = 0.06f;
    private void SetNearCamera()
    {
        _camera.nearClipPlane = (int)cameraTransform.localPosition.y * nearFactor;
    }
    
    private readonly List<(float zoomThreshold, float shadowDistance)> shadowTable = new()
    {
        (50f, 100f),            // if zoom < 50 then set shadow distance 100
        (100f, 200f),
        (200f, 350f),
        (300f, 550f),
        (400f, 700f),
        (float.MaxValue, 800f)  // if zoom >= 400 then set shadow distance 800
    };

    private void SetShadowDistance()
    {
        float currentZoom = cameraTransform.localPosition.y;

        foreach (var (threshold, distance) in shadowTable)
        {
            if (currentZoom < threshold)
            {
                if (urpAsset.shadowDistance != distance)
                    urpAsset.shadowDistance = distance;

                break;
            }
        }
    }

    private void Control()
    {
        // move
        transform.position = Vector3.Lerp(transform. position, newPosition, Time.deltaTime * movementTime);

        // rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);

        // zoom
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }

    private bool isDrag = false;
    private void HandleMouseInput()
    {
        if (EventSystem.current.IsPointerOverGameObject()) 
        {
            isDrag = false;
            return;
        }
        
        // move
        if(Input.GetMouseButtonDown((int)MouseButton.Right))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray, out entry))
                dragStartPosition = ray.GetPoint(entry);

            isDrag = true;
        }

        if(Input.GetMouseButton((int)MouseButton.Right) && isDrag)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray, out entry))
            { 
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }

        if(Input.GetMouseButtonUp((int)MouseButton.Right))
        {
            isDrag = false;
        }


        // rotation
        if(Input.GetMouseButtonDown((int)MouseButton.Middle))
            rotateStartPosition = Input.mousePosition;
        
        if(Input.GetMouseButton((int)MouseButton.Middle))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }


        // zoom
        if(Input.mouseScrollDelta.y != 0)
            newZoom += Input.mouseScrollDelta.y * (cameraTransform.localPosition * -zoomAmount);

        if(newZoom.z < -500f)
            newZoom = new Vector3(newZoom.x, 500f, -500f);

        if(newZoom.z > -10f)
            newZoom = new Vector3(newZoom.x, 10f, -10f);
    }

    private void HandleMovementInput()
    {
        // move
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            newPosition += (transform.forward * (movementSpeed + 0.005f * cameraTransform.localPosition.y));

        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            newPosition += (transform.forward * -(movementSpeed + 0.005f * cameraTransform.localPosition.y));

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow))
            newPosition += (transform.right * -(movementSpeed + 0.005f * cameraTransform.localPosition.y));

        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow))
            newPosition += (transform.right * (movementSpeed + 0.005f * cameraTransform.localPosition.y));
        

        // rotation
        if(Input.GetKey(KeyCode.Q))
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);

        if(Input.GetKey(KeyCode.E))
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);

        
        // zoom
        if(Input.GetKey(KeyCode.Z))
            newZoom += (cameraTransform.localPosition * -zoomAmount / 4);
        
        if(Input.GetKey(KeyCode.X))
            newZoom -= (cameraTransform.localPosition * -zoomAmount / 4);

        if(newZoom.z < -500f)
            newZoom = new Vector3(newZoom.x, 500f, -500f);

        if(newZoom.z > -10f)
            newZoom = new Vector3(newZoom.x, 10f, -10f);
    }

    public void FocusToObject(Actor actor)
    {
        newPosition = actor.GetPosition()[0].position;
        newZoom = new Vector3(newZoom.x, 100f, -100f);
    }
}
