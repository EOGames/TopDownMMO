using System;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public bool sendMovementData = false;
    NavMeshAgent agent;
    RaycastHit hit;
    Ray ray;
    [SerializeField] LayerMask groundLayer;
    Vector3 targetToReach;
    bool timeToMove = false;
    bool reachingLocation = false;
    float safeDistanceToTarget = 0.4f;
    private Player player;
 

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GetComponent<Player>();
    }

    void Update()
    {
        CheckForInput();
        MoveToTarget();
    }
    private void CheckForInput()
    {
        if (player.isLocalPlayer)
        {
            if (Input.GetMouseButtonUp(0))
            {
                timeToMove = false;
                reachingLocation = false;

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    targetToReach = hit.point;
                    timeToMove = true;
                }
                else
                {
                    print("No Target");
                }
            }
        }
    }

    public void MoveCloneToTarget(Vector3 destination)
    {
        targetToReach = destination;
        timeToMove = true;
    }
    void MoveToTarget()
    {
        if (timeToMove)
        {
            if (GetDistanceToTarget(targetToReach) > safeDistanceToTarget)
            {
                if (sendMovementData)
                {
                    SocketManager.Instance.GetMovementHandler().SendMovementData(targetToReach,player.playerId);
                    print("Movement Data Sent From Agent");
                }
                agent.isStopped = false;
                agent.SetDestination(targetToReach);
                timeToMove = false;
                reachingLocation = true;
                print("Time To Move");
            }
        }

        if (reachingLocation)
        {
            if (GetDistanceToTarget(targetToReach) <= safeDistanceToTarget)
            {
                reachingLocation = false;
                agent.isStopped = true;
                print("Destination Reached");
            }
        }
    }

    public float GetDistanceToTarget(Vector3 tar)
    {
        return Vector3.Distance(transform.position, tar);
    }
}
