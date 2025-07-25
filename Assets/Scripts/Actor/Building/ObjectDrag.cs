using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private LayerMask layerMask;
    private bool isConnect;

    private void Update()
    {
        layerMask = 1 << LayerMask.NameToLayer("Ground");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999, layerMask))
        {
            if (!isConnect)
            {
                transform.position = BuildSystem.current.SnapCoordinateToGrid(raycastHit.point);
            }
            else
            {
                float _distance = Vector3.Distance(transform.position, raycastHit.point);
                if (_distance > 5f)
                {
                    FalseConnect();
                    transform.position = BuildSystem.current.SnapCoordinateToGrid(raycastHit.point);
                }
            }
        }
    }

    public void TrueConnect() => isConnect = true;
    public void FalseConnect() => isConnect = false;
}
