using Unity.VisualScripting;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    private Camera _camera;
    
    [SerializeField] private float normalSpeed;
    [SerializeField] private float fastSpeed;
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementTime;
    [SerializeField] private float rotationAmount;
    [SerializeField] private Vector3 zoomAmount;

    private Vector3 newPosition;
    private Quaternion newRotation;
    private Vector3 newZoom;

    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;
    private Vector3 rotateStartPosition;
    private Vector3 rotateCurrentPosition;

    private void Start() 
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;

        _camera = cameraTransform.gameObject.GetComponent<Camera>();
    }

    private void Update() 
    {
        // if (EventSystem.current.IsPointerOverGameObject()) 
        // {
        //     return;
        // }
        
        HandleMouseInput();
        HandleMovementInput();

        SetNearCamera();
        Control();
    }

    private float nearFactor = 0.06f;
    private void SetNearCamera()
    {
        _camera.nearClipPlane = newZoom.y * nearFactor;
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

    private void HandleMouseInput()
    {
        // move
        if(Input.GetMouseButtonDown((int)MouseButton.Right))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray, out entry))
                dragStartPosition = ray.GetPoint(entry);
        }

        if(Input.GetMouseButton((int)MouseButton.Right))
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
            newZoom += Input.mouseScrollDelta.y * zoomAmount;

        if(newZoom.z < -500f)
            newZoom = new Vector3(newZoom.x, 500f, -500f);

        if(newZoom.z > -10f)
            newZoom = new Vector3(newZoom.x, 10f, -10f);
    }

    private void HandleMovementInput()
    {
        // set speed move
        if(Input.GetKey(KeyCode.LeftShift)) 
            movementSpeed = fastSpeed;
        else 
            movementSpeed = normalSpeed;
        
        // move
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            newPosition += (transform.forward * movementSpeed);

        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            newPosition += (transform.forward * -movementSpeed);

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow))
            newPosition += (transform.right * -movementSpeed);

        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow))
            newPosition += (transform.right * movementSpeed);
        

        // rotation
        if(Input.GetKey(KeyCode.Q))
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);

        if(Input.GetKey(KeyCode.E))
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);

        
        // zoom
        if(Input.GetKey(KeyCode.R))
            newZoom += zoomAmount;
        
        if(Input.GetKey(KeyCode.F))
            newZoom -= zoomAmount;

        if(newZoom.z < -500f)
            newZoom = new Vector3(newZoom.x, 500f, -500f);

        if(newZoom.z > -10f)
            newZoom = new Vector3(newZoom.x, 10f, -10f);
    }
}
