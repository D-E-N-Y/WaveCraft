using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;

    private void OnMouseDown() 
    {
        offset = transform.position - BuildSystem.GetMouseWorldPosition();
    }

    private void OnMouseDrag() 
    {
        Vector3 pos = BuildSystem.GetMouseWorldPosition() + offset;
        transform.position = BuildSystem.current.SnapCoordinateToGrid(pos);
    }
}
