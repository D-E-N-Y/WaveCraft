using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleZone : MonoBehaviour
{
    [SerializeField] private int segments = 200;
    [SerializeField] Color lineColor;
    [SerializeField, Range(0.1f, 5f)] float lineWidth;

    [SerializeField] Material material;

    private class Side
    {
        public List<Vector3> points;
    }

    [SerializeField] private List<SCircleZone> zones;
    [SerializeField] private bool isUpdate;

    // void Start()
    // {
    //     Initialize();
    // }

    // void Update()
    // {
    //     if (isUpdate)
    //     {
    //         DrawLines(zones);
    //     }
    // }

    public void Initialize()
    {
        transform.localPosition = new Vector3(0f, lineWidth / 2, 0f);
    }

    public void DrawLines(List<SCircleZone> zones)
    {
        List<Side> sides = new List<Side>();

        MainLines(zones, sides);
        ExtraLines(sides);
    }

    private void MainLines(List<SCircleZone> zones, List<Side> sides)
    {
        List<Vector3> points = new List<Vector3>();

        sides.Add(new Side());
        sides.Last().points = new List<Vector3>(); 

        foreach (SCircleZone zone in zones)
        {
            for (int i = 0; i < segments; i++)
            {
                float angle = 2 * Mathf.PI * i / segments;

                Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * zone._radius;
                Vector3 point = zone._transform.position + offset;

                if (IsIntoOtherZone(point, zones, zone))
                {
                    sides.Last().points.Add(point);
                }
                else
                {
                    sides.Add(new Side());
                    sides.Last().points = new List<Vector3>();
                }
            }

            sides.Add(new Side());
            sides.Last().points = new List<Vector3>();
        }

        sides = sides.Where(x => x.points.Count > 0).ToList();

        List<LineRenderer> mainLines = GetComponentsInChildren<LineRenderer>(true).ToList();
        mainLines.ForEach(x => x.gameObject.SetActive(false));

        int residue = Math.Max(sides.Count - mainLines.Count, 0);

        for (int i = 0; i < residue; i++)
        {
            CreateLine(mainLines);
        }

        for (int i = 0; i < sides.Count; i++)
        {
            InitializeLine(mainLines[i], sides[i].points);
        }
    }

    private void ExtraLines(List<Side> sides)
    {
        List<LineRenderer> extraLines = GetComponentsInChildren<LineRenderer>(true)
            .Where(x => !x.gameObject.activeSelf)
            .ToList();

        sides = sides.Where(x => x.points.Count > 0).ToList();

        for (int i = 0; i < sides.Count; i++)
        {
            if (i == extraLines.Count)
            {
                CreateLine(extraLines);
            }

            List<Vector3> points = new List<Vector3>();
            points.Add(sides[i].points.Last());

            Vector3 neiborPoint = sides
                .Where(x => sides[i].points.Last() != x.points.First())
                .OrderBy(x => Vector3.Distance(sides[i].points.Last(), x.points.First()))
                .Select(x => x.points.First())
                .FirstOrDefault();

            points.Add(neiborPoint);

            InitializeLine(extraLines[i], points);
        }
    }

    private void InitializeLine(LineRenderer line, List<Vector3> points)
    {
        line.useWorldSpace = false;
        line.material = material;
        line.startWidth = line.endWidth = lineWidth;
        line.startColor = line.endColor = lineColor;

        List<Vector3> localPoints = points
            .Select(p => transform.InverseTransformPoint(p) + new Vector3(0f, MathF.Abs(transform.InverseTransformPoint(p).y), 0))
            .ToList();

        line.positionCount = localPoints.Count;
        line.SetPositions(localPoints.ToArray());

        line.gameObject.SetActive(true);
        line.transform.localPosition = Vector3.zero;

        line.material.SetFloat("_segmentLength", GetLengthLine(localPoints));
        line.material.SetFloat("_segmentCount", segments);
        line.material.SetColor("_Color", lineColor);
    }

    private float GetLengthLine(List<Vector3> points)
    {
        float distance = Vector3.Distance(points[0], points[1]);
        return distance * points.Count;
    }

    private void CreateLine(List<LineRenderer> lines)
    {
        GameObject obj = new GameObject("Line");
        obj.transform.SetParent(transform);
        obj.SetActive(false);

        LineRenderer lr = obj.AddComponent<LineRenderer>();
        lines.Add(lr);
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