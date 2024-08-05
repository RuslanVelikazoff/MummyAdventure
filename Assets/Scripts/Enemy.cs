using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    [SerializeField]private NavMeshAgent agent;
    float timePassed;
    [SerializeField] float hearRange = 4f;
    [SerializeField] public List<Transform> randomDestination;
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [Header("Detection")]
    public float radius;
    [Range(0, 360)]
    [SerializeField] float angle;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    [SerializeField] bool canSeePlayer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorControl();
        if (Vector3.Distance(player.transform.position, transform.position) <= hearRange)
        {
            agent.SetDestination(player.transform.position);
            Vector3 dir = player.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.localEulerAngles = lookRotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0f, transform.localEulerAngles.y, 0f);
        }
        else
        {           
            if(agent.velocity == Vector3.zero)
            {
                int rand = Random.Range(0, randomDestination.Count);
                agent.SetDestination(randomDestination[Random.Range(0, rand)].position);
            }
        }
        if (canSeePlayer)
        {
            agent.SetDestination(player.transform.position);
            Vector3 dir = player.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.localEulerAngles = lookRotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0f, transform.localEulerAngles.y, 0f);
        }
    }

    private void AnimatorControl()
    {
        if (agent.velocity != Vector3.zero)
        {
            animator.SetBool("IsWalk", true);
        }
        else
        {
            animator.SetBool("IsWalk", false);
        }
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hearRange);
        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, angle / 2);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + viewAngle01 * radius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + viewAngle02 * radius);
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
