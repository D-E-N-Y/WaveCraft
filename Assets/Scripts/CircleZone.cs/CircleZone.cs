using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleZone : MonoBehaviour
{
    [SerializeField] private int segments = 64;
    [SerializeField] Color lineColor;

    private LineRenderer line;

    private class Side
    {
        public List<Vector3> points;
    }

    // [SerializeField] private List<SCircleZone> zones;

    // void Start()
    // {
    //     Initialize();
    //     // DrawLine(zones);
    // }

    // void Update()
    // {
    //     DrawLine(zones);
    // }

    public void Initialize()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startWidth = line.endWidth = 0.5f;
        line.startColor = line.endColor = lineColor;
    }

    public void DrawLine(List<SCircleZone> zones)
    {
        line.useWorldSpace = false;
        line.loop = true;
        line.positionCount = segments;

        List<Side> sides = new List<Side>();
        List<Vector3> points = new List<Vector3>();

        int countSides = 0;
        sides.Add(new Side());
        sides[countSides].points = new List<Vector3>();

        foreach (SCircleZone zone in zones)
        {
            for (int i = 0; i < segments; i++)
            {
                float angle = 2 * Mathf.PI * i / segments;

                Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * zone._radius;
                Vector3 point = zone._transform.position + offset;

                if (IsIntoOtherZone(point, zones, zone))
                {
                    sides[countSides].points.Add(point);
                }
                else
                {
                    countSides++;
                    sides.Add(new Side());
                    sides[countSides].points = new List<Vector3>();
                }
            }
        }

        sides = sides.Where(x => x.points.Count > 0).ToList();

        Side side = sides[0];
        for (int i = 0; i < sides.Count; i++)
        {
            List<Side> neiborthSides = new List<Side>();

            foreach (Side _side in sides)
            {
                if (side == _side) continue;
                neiborthSides.Add(_side);
            }

            Side neiborthSide = neiborthSides
                .OrderBy(x => Vector3.Distance(x.points.First(), side.points.Last()))
                .FirstOrDefault();

            points.AddRange(side.points);
            side = neiborthSide;
        }

        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
    }

    private bool IsIntoOtherZone(Vector3 point, List<SCircleZone> zones, SCircleZone current)
    {
        foreach (SCircleZone zone in zones)
        {
            if (zone == current) continue;

            if (Vector3.Distance(point, zone._transform.position) < zone._radius)
            {
                return false;
            }
        }
        return true;
    }
}
