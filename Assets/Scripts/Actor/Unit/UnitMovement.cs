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

    private Vector3 GetNearbyPosition(List<Transform> target)
    {
        if(target == null)
        {
            return transform.position;
        }
        
        return target.OrderBy(x => Vector3.Distance(x.position, transform.position)).First().position;
    }

    public IEnumerator MoveTo(List<Transform> target, E_MoveTo _object)
    {
        isMoving = true;

        while (true)
        {
            switch(_object)
            {
                case E_MoveTo.PlacedObject:
                    MoveToPlacedObject(GetNearbyPosition(target));
                    break;

                case E_MoveTo.NatureObject:
                    MoveToNatureObject(GetNearbyPosition(target));
                    break;
            }

            // MoveToObject(GetNearbyPosition(target));

            while(agent.pathPending || !agent.hasPath)
            {
                yield return null;
            }

            yield return null;

            if (agent.remainingDistance <= 0.1f || agent.velocity.sqrMagnitude <= 0f)
            {
                break;
            }
        }
        agent.ResetPath();

        isMoving = false;

        yield return RotateToObject(target.First());
        // OnMoveComplete?.Invoke();
    }

    private IEnumerator RotateToObject(Transform target)
    {
        transform.LookAt(target);

        yield return null;
    }
    
    private Vector3 MiddlePoint(Vector3 vector_1, Vector3 vector_2)
    {
        return (vector_1 + vector_2) / 2;
    }

    private Vector3 GetBestNavMeshPoint(Vector3 agentPosition, Vector3 targetPosition)
    {
        Vector3 direction = (agentPosition - targetPosition).normalized;
        float maxDistance = Vector3.Distance(targetPosition, agentPosition);

        Vector3 bestPoint = targetPosition;

        for (float dist = maxDistance; dist > 0; dist -= 0.1f)
        {
            bestPoint = targetPosition + direction * (maxDistance - dist);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(bestPoint, out hit, 0.1f, NavMesh.AllAreas))
            {
                bestPoint = hit.position;
                break;
            }
        }

        return bestPoint;
    }


    private void MoveToNatureObject(Vector3 target)
    {
        Vector3 bestPoint = GetBestNavMeshPoint(transform.position, target);

        // agent.SetDestination(bestPoint);

        if (Vector3.Distance(agent.destination, bestPoint) > 0.5f)
        {
            agent.SetDestination(bestPoint);
        }
    }

    private void MoveToPlacedObject(Vector3 target)
    {
        agent.SetDestination(target);
    }
}