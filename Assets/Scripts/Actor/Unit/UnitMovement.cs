using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Object
    }

    public void Initialize()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    private Vector3 GetNearbyPosition(List<Transform> target)
    {
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
                
                case E_MoveTo.Object:
                    MoveToObject(GetNearbyPosition(target));
                    break;
            }

            // while(agent.pathPending || !agent.hasPath)
            // {
            //     yield return null;
            // }

            yield return new WaitForSeconds(0.1f);

            if(agent.remainingDistance <= 0.1f || agent.velocity.sqrMagnitude <= 0f)
            {
                break;
            }
        }
        agent.ResetPath();

        isMoving = false;
        // OnMoveComplete?.Invoke();
    }

    public void StopMove()
    {
        // StopAllCoroutines();
        agent.ResetPath();
        isMoving = false;
    }
    
    private Vector3 MiddlePoint(Vector3 vector_1, Vector3 vector_2)
    {
        return (vector_1 + vector_2) / 2;
    }

    private Vector3 GetBestNavMeshPoint(Vector3 agentPosition, Vector3 targetPosition)
    {
        Vector3 midPoint = MiddlePoint(agentPosition, targetPosition);
        NavMeshHit hit;

        if(NavMesh.SamplePosition(midPoint, out hit, 5f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        // if(NavMesh.SamplePosition(agentPosition, out hit, 5f, NavMesh.AllAreas))
        // {
        //     return hit.position;
        // }
        return targetPosition;
    }

    private void MoveToPlacedObject(Vector3 target) 
    {
        Vector3 bestPoint = GetBestNavMeshPoint(transform.position, target);
        
        // agent.SetDestination(bestPoint);
        
        if (Vector3.Distance(agent.destination, bestPoint) > 0.5f)
        {
            agent.SetDestination(bestPoint);
        }
    }

    private void MoveToObject(Vector3 target)
    {
        agent.SetDestination(target);
    }
}