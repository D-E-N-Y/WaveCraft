using System;
using System.Collections;
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

    public IEnumerator MoveTo(Vector3 target, E_MoveTo _object)
    {
        isMoving = true;
        
        while (true)
        {
            switch(_object)
            {
                case E_MoveTo.PlacedObject:
                    MoveToPlacedObject(target);
                    break;
                
                case E_MoveTo.Object:
                    MoveToObject(target);
                    break;
            }

            yield return new WaitForSeconds(0.5f);

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
    
    private Vector3 GetBestNavMeshPoint(Vector3 agentPosition, Vector3 targetPosition)
    {
        Vector3 midPoint = (agentPosition + targetPosition) / 2;
        NavMeshHit hit;

        if(NavMesh.SamplePosition(midPoint, out hit, 5f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        if(NavMesh.SamplePosition(agentPosition, out hit, 5f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return targetPosition;
    }

    private void MoveToPlacedObject(Vector3 target) 
    {
        Vector3 bestPoint = GetBestNavMeshPoint(transform.position, target);
        
        if (Vector3.Distance(agent.destination, bestPoint) > 1f)
        {
            agent.SetDestination(bestPoint);
        }
    }

    private void MoveToObject(Vector3 target)
    {
        agent.SetDestination(target);
    }
}