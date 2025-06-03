using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float viewRadius = 10f;
    [Range(0, 360)]
    public float viewAngle = 120f;
    public Transform player;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    private NavMeshAgent agent;
    private bool playerInSight = false;
    [SerializeField] private Animator animator;

    private int health = 2;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (IsPlayerInView())
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > attackRange)
            {
                animator.SetBool("AttackRange", false);
                // Walking towards player
                agent.SetDestination(player.position);

                animator.SetBool("IsWalking", true);
                // Optional: cancel attack state if you use a bool instead of trigger
            }
            else
            {
                // In attack range
                agent.ResetPath();
                animator.SetBool("IsWalking", false);
                TryAttackPlayer();
            }
        }
        else
        {
            // Idle when player not in view
            agent.ResetPath();
            animator.SetBool("AttackRange", false);
            animator.SetBool("IsWalking", false);
        }
    }


    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }
    bool IsPlayerInView()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
        {
            if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask))
            {
                Debug.Log("Zombie following player!");
                return true;
            }
        }

        return false;
    }
    void TryAttackPlayer()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
                Debug.Log("Zombie attacked the player!");
                lastAttackTime = Time.time;
                animator.SetBool("AttackRange", true);
            }
        }
    }
    void Die()
    {
        animator.SetTrigger("Die");

        Debug.Log("Zombie died!");

        agent.ResetPath();         
        agent.enabled = false;     

        this.enabled = false;

        Destroy(gameObject, 3f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);
    }

    Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
