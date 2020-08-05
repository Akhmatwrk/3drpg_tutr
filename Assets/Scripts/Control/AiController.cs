using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Control;
using RPG.Resources;

public class AiController : MonoBehaviour
{
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspitionTime = 3f;
    [SerializeField] float timeForPatrolStop = 2f;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointTorlerance = 1f;

    // Cache
    private GameObject targetToAttack = null;
    private Fighter fighter = null;
    private Health health = null;
    private Mover mover = null;


    // Memory
    Vector3 guardLocation;
    float lastTimeSeenPlayer = Mathf.Infinity;
    int currentWaypointIndex = 0;
    float lastTimeWaypointStop = Mathf.Infinity;


    private void Start()
    {
        targetToAttack = GameObject.FindWithTag("Player");
        fighter = GetComponent<Fighter>();
        health = GetComponent<Health>();
        mover = GetComponent<Mover>();

        guardLocation = transform.position;
    }

    private void Update()
    {
        if (health.IsDead()) return;

        if (isTargetInDistance() && fighter.CanAttack(targetToAttack))
        {
            fighter.Attack(targetToAttack);
            lastTimeSeenPlayer = 0f;
        }
        else if (isSuspitionTime())
        {
            SuspicionBehaviour();
        }
        else
        {
            GuardBehaviour();
        }

        UpdateTimers();

    }

    private void UpdateTimers()
    {
        lastTimeSeenPlayer += Time.deltaTime;
        lastTimeWaypointStop += Time.deltaTime;
    }

    private void GuardBehaviour()
    {
        Vector3 nextPosition = guardLocation;

        if (patrolPath != null)
        {
            if (Waypoint())
            {
                CycleWaypoint();
                lastTimeWaypointStop = 0f;
            }

            nextPosition = GetCurrentWaypoint();
        }

        if (lastTimeWaypointStop >= timeForPatrolStop)
        {
            mover.StartMoveAction(nextPosition);
        }
    }


    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        //print("Current Waypoint is " + currentWaypointIndex);
    }

    private bool Waypoint()
    {
        float distancToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distancToWaypoint <= waypointTorlerance;
    }

    private void SuspicionBehaviour()
    {
        GetComponent<ActionScheduler>().CancelCurrentAction();
    }

    private bool isSuspitionTime()
    {
        return (lastTimeSeenPlayer < suspitionTime);
    }

    private bool isTargetInDistance()
    {
        float distanceToTarget = Vector3.Distance(targetToAttack.transform.position, transform.position);
        return distanceToTarget < chaseDistance;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
