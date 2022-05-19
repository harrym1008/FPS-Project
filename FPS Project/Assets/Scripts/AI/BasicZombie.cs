using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicZombie : MonoBehaviour
{
    public Vector2 damageRange;
    public Transform player;

    NavMeshAgent agent;
    Target target;
    DieToRagdoll dieToRagdoll;
    [SerializeField] Animator animator;

    public float attackDistance;

    float timeTillNavUpdate;
    float timeTillDistCheck;
    bool attacking;


    private void Start()
    {
        timeTillNavUpdate = RNG.Range(0, 2);
        timeTillDistCheck = RNG.Range(0, 0.5f);

        animator.SetFloat("Speed", RNG.Range(0.92f, 1.08f));

        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent = GetComponent<NavMeshAgent>();
        target = GetComponent<Target>();
        dieToRagdoll = GetComponent<DieToRagdoll>();
    }

    private void Update()
    {
        timeTillDistCheck -= Time.deltaTime;
        timeTillNavUpdate -= Time.deltaTime;


        if (target.dead)
        {
            dieToRagdoll.IHaveDied(new List<DismemberableLimbs>() { target.limbToDismember });
            return;
        }
        else if (attacking)
        {
            return;
        }


        if (timeTillNavUpdate < 0f)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            timeTillNavUpdate = RNG.Range(0.9f, 1.1f);
        }

        if (agent.remainingDistance < agent.stoppingDistance)
        {
            agent.SetDestination(player.position);
        }

        if (timeTillDistCheck < 0f)
        {
            timeTillDistCheck = 0.5f;

            if (Vector3.Distance(player.position, transform.position) < attackDistance)
            { 
                StartCoroutine(Attack());
            }
        }
    }

    private IEnumerator Attack()
    {
        attacking = true;
        agent.isStopped = true;
        animator.SetBool("Attacking", true);

        while (true)
        {
            PointTowardsPlayer();
            yield return new WaitForSeconds(0.24f);
            PointTowardsPlayer();

            if (Vector3.Distance(player.position, transform.position) < attackDistance)
            {
                player.GetComponent<Target>().TakeDamage(RNG.RangeBetweenVector2(damageRange));
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(0.4f);
            PointTowardsPlayer();

            if (Vector3.Distance(player.position, transform.position) < attackDistance)
            {
                player.GetComponent<Target>().TakeDamage(RNG.RangeBetweenVector2(damageRange));
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(0.36f);
        }

        animator.SetBool("Attacking", false);
        attacking = false;
        agent.isStopped = false;
    }

    void PointTowardsPlayer()
    {
        transform.LookAt(player);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
    }
}
