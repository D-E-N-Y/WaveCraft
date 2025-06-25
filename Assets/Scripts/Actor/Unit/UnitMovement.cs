using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    // public event Action OnMoveComplete;

    private NavMeshAgent agent;
    public bool isMoving { get; private set; }
    public bool isCanMove { get; private set; }

    public enum E_MoveTo
    {
        PlacedObject,
        NatureObject
    }

    public void Initialize()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    public void StopMove() => agent.isStopped = true;
    public void StartMove() => agent.isStopped = false;

    public float DistanceToGoal(List<Transform> target) => Vector3.Distance(GetNearbyPosition(target), transform.position);

    private Vector3 GetNearbyPosition(List<Transform> target)
    {
        if (target == null)
        {
            return transform.position;
        }

        return target.OrderBy(x => Vector3.Distance(x.position, transform.position)).First().position;
    }

    private Vector3 GetNearbyPosition(List<Vector3> target)
    {
        if (target == null)
        {
            return transform.position;
        }

        return target.OrderBy(x => Vector3.Distance(x, transform.position)).First();
    }

    public IEnumerator MoveTo(IPosition iPosition, E_MoveTo _object)
    {
        isMoving = true;

        List<Vector3> avaliablePoints = new List<Vector3>();
        switch (_object)
        {
            case E_MoveTo.NatureObject:
                avaliablePoints = GetBestNavMeshPoints(transform.position, iPosition);
                break;
        }

        while (true)
        {
            switch (_object)
            {
                case E_MoveTo.PlacedObject:
                    MoveToPlacedObject(iPosition);
                    break;

                case E_MoveTo.NatureObject:
                    MoveToNatureObject(avaliablePoints);
                    break;
            }

            while (!(agent.hasPath && !agent.pathPending && agent.remainingDistance > 0f))
            {
                yield return null;

                if (IsFinish())
                {
                    break;
                }
            }

            yield return null;

            if (IsFinish())
            {
                break;
            }
        }
        agent.ResetPath();

        isMoving = false;

        yield return RotateToObject(iPosition.GetActor().transform);
    }

    private bool IsFinish()
    {
        return !agent.pathPending
            && agent.remainingDistance <= agent.stoppingDistance
            && (!agent.hasPath || agent.velocity.sqrMagnitude < 0.01f);
    }

    private IEnumerator RotateToObject(Transform target)
    {
        transform.LookAt(target);
        yield return null;
    }

    private List<Vector3> GetBestNavMeshPoints(Vector3 agentPosition, IPosition iPosition)
    {
        Vector3 targetPosition = GetNearbyPosition(iPosition.GetPosition());
        float maxDistance = Vector3.Distance(targetPosition, agentPosition);

        float step = 0.5f;
        float radius = 0.5f;

        List<Vector3> _avaliablePoints = new List<Vector3>();

        for (float dist = radius; dist <= maxDistance; dist += step)
        {
            List<Vector3> testPoints = new List<Vector3>();
            float angleStep = 15f; // градусы

            int countPointsInOtherObject = 0;
            for (float angle = 0; angle < 360f; angle += angleStep)
            {
                float rad = angle * Mathf.Deg2Rad;
                float x = targetPosition.x + dist * Mathf.Cos(rad);
                float z = targetPosition.z + dist * Mathf.Sin(rad);
                float y = targetPosition.y;

                Vector3 _point = new Vector3(x, y, z);

                testPoints.Add(_point);

                Collider[] colliders = Physics.OverlapSphere(_point, 0.01f);
                foreach (Collider hit in colliders)
                {
                    if (hit.gameObject.TryGetComponent<Actor>(out Actor _actor) && _actor != iPosition.GetActor())
                    {
                        countPointsInOtherObject++;
                    }
                }
            }

            if (testPoints.Count * 0.9 <= countPointsInOtherObject)
            {
                Debug.Log($"{radius / step} | {testPoints.Count * 0.9}/{testPoints.Count} {countPointsInOtherObject} | Обьект окружен препядствиями!");
                isCanMove = false;
                return null;
            }

            foreach (var point in testPoints)
            {
                if (NavMesh.SamplePosition(point, out NavMeshHit hit, 1f, NavMesh.AllAreas))
                {
                    NavMeshPath path = new NavMeshPath();
                    if (agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                    {
                        _avaliablePoints.Add(hit.position);
                    }
                }
            }

            if (_avaliablePoints.Count > 30)
            {
                isCanMove = true;
                return _avaliablePoints;
            }
        }

        isCanMove = false;
        return null;
    }

    private void MoveToNatureObject(List<Vector3> avaliablePoints)
    {
        if (avaliablePoints == null)
        {
            agent.SetDestination(transform.position);
        }
        else
        {
            Vector3 bestPoint = GetNearbyPosition(avaliablePoints);
            agent.SetDestination(bestPoint);
        }
    }

    private void MoveToPlacedObject(IPosition iPosition)
    {
        Vector3 bestPoint = GetNearbyPosition(iPosition.GetPosition());
        agent.SetDestination(bestPoint);
    }

    public void MoveToPosition(Vector3 target)
    {
        agent.SetDestination(target);
    }
}