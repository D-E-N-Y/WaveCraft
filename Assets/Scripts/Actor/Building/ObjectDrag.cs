using System;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    public Action stopDrag;
    private Vector3 offset;

    private void OnMouseDown() 
    {
        offset = transform.position - InteractionSystem.GetMouseWorldPosition();
    }

    private void OnMouseDrag() 
    {
        Vector3 pos = InteractionSystem.GetMouseWorldPosition() + offset;
        transform.position = BuildSystem.current.SnapCoordinateToGrid(pos);
    }

    private void OnMouseUp()
    {
        stopDrag?.Invoke();
    }
}
