using UnityEngine;

[System.Serializable]
public class SCircleZone
{
    public Transform _transform;
    public float _radius;

    public SCircleZone(Transform _transform, float _radius)
    {
        this._transform = _transform;
        this._radius = _radius;
    }
}