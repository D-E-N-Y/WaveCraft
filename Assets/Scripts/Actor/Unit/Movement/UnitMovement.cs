using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public bool isMoving { get; private set; }
    public bool isCanMove { get; private set; }

    private Coroutine updateAvoidancePriority;
    private float timeUpdateAvoidancePriority = 5.0f;

    public void Initialize()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        updateAvoidancePriority = StartCoroutine(nameof(UpdateAvoidancePriority));
    }

    private IEnumerator UpdateAvoidancePriority()
    {
        agent.avoidancePriority = Random.Range(10, 90);

        while (true)
        {
            if (agent.velocity.magnitude < 0.1f)
            {
                agent.avoidancePriority = Random.Range(10, 90);
            }

            yield return new WaitForSeconds(timeUpdateAvoidancePriority + Random.Range(0f, 1f));
        }
    }

    public void StopMove() => agent.isStopped = true;
    public void StartMove() => agent.isStopped = false;

    public float DistanceToGoal(List<Transform> target) => Vector3.Distance(GetNearbyPosition(target), transform.position);

    private Vector3 GetNearbyAvaliablePosition(List<Transform> target)
    {
        if (target == null)
        {
            return transform.position;
        }

        List<Vector3> _avaliablePoints = new List<Vector3>();
        foreach (Transform point in target)
        {
            if (NavMesh.SamplePosition(point.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    _avaliablePoints.Add(hit.position);
                }
            }
        }

        if (_avaliablePoints.Count > 0)
        {
            return _avaliablePoints.OrderBy(x => Vector3.Distance(x, transform.position)).First();
        }
        else
        {
            return new Vector3(-9999, -9999, -9999);
        }
    }

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

        debugPointsNotMove = new List<Vector3>();
        debugPointsMove = new List<Vector3>();
        debugPoints = new List<Vector3>();

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
                debugPoints.Add(_point);

                Collider[] colliders = Physics.OverlapSphere(_point, 0.01f);

                bool isObstacle = false;
                bool isSelf = false;

                foreach (Collider hit in colliders)
                {
                    if (hit.gameObject.TryGetComponent<Actor>(out Actor _actor))
                    {
                        if (_actor != iPosition.GetActor() && !isObstacle)
                        {
                            isObstacle = true;
                        }
                        else if (_actor == iPosition.GetActor() && !isSelf)
                        {
                            isSelf = true;
                        }
                    }

                    if (isSelf && isObstacle) break;
                }

                if (isObstacle && !isSelf)
                {
                    debugPointsNotMove.Add(_point);
                    countPointsInOtherObject++;
                }
            }

            if (testPoints.Count * 0.8f <= countPointsInOtherObject)
            {
                Debug.Log($"{radius / step} | {testPoints.Count * 0.8f}/{countPointsInOtherObject} {countPointsInOtherObject} | Обьект окружен препядствиями!");
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
                        debugPointsMove.Add(hit.position);
                    }
                }
            }

            if (_avaliablePoints.Count > 5)
            {
                isCanMove = true;
                return _avaliablePoints;
            }
        }

        isCanMove = false;
        return null;
    }





    private List<Vector3> debugPointsNotMove;
    private List<Vector3> debugPointsMove;
    private List<Vector3> debugPoints;
    public bool showDebugNavMeshPoints = true;

    private void OnDrawGizmosSelected()
    {
        if (!(!showDebugNavMeshPoints || debugPointsMove == null))
        {
            Gizmos.color = Color.green;
            foreach (var point in debugPointsMove)
            {
                Gizmos.DrawSphere(point, 0.1f);
            }
        }

        if (!(!showDebugNavMeshPoints || debugPointsNotMove == null))
        {
            Gizmos.color = Color.red;
            foreach (var point in debugPointsNotMove)
            {
                Gizmos.DrawSphere(point, 0.1f);
            }
        }

        if (!(!showDebugNavMeshPoints || debugPoints == null))
        {
            Gizmos.color = Color.yellow;
            foreach (var point in debugPoints)
            {
                Gizmos.DrawSphere(point, 0.05f);
            }
        }
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
        Vector3 bestPoint = GetNearbyAvaliablePosition(iPosition.GetPosition());
        MoveToPosition(bestPoint);
    }

    public void MoveToPosition(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(target, path) && path.status == NavMeshPathStatus.PathComplete && target != null)
        {
            agent.SetDestination(target);
            isCanMove = true;
        }
        else
        {
            agent.SetDestination(transform.position);
            isCanMove = false;
        }
    }

    void OnDisable()
    {
        if (updateAvoidancePriority != null)
        {
            StopCoroutine(updateAvoidancePriority);
        }
    }
}