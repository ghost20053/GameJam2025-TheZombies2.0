using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PumpkinShooter : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("Patrol Settings")]
    public float patrolRadius = 10f;
    public float patrolWaitTime = 2f;
    private float waitTimer = 0f;

    [Header("Detection Settings")]
    public float hearingRange = 15f;   // how far it can hear player sounds
    public float attackRange = 20f;    // how close to attack player

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileForce = 20f;
    public float fireRate = 1f;
    private float nextFire = 0f;

    [Header("References")]
    private NavMeshAgent agent;
    private Transform player;
    private bool reactingToSound = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentHealth = maxHealth;
        ChooseNewPatrolPoint();
    }

    private void Update()
    {
        if (isDead) return;

        // If reacting to sound, AI will stop patrolling
        if (reactingToSound) return;

        // Patrol
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= patrolWaitTime)
            {
                ChooseNewPatrolPoint();
                waitTimer = 0f;
            }
        }

        // Check if player in hearing range
        if (Vector3.Distance(transform.position, player.position) <= hearingRange)
        {
            HearSound(player.position);
        }

        // Attack if in range
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            AttackPlayer();
        }
    }

    // Picks a random point to patrol to
    private void ChooseNewPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    // Called when the AI hears something
    public void HearSound(Vector3 soundPos)
    {
        if (Vector3.Distance(transform.position, soundPos) <= hearingRange)
        {
            StartCoroutine(ReactToSound(soundPos));
        }
    }

    private IEnumerator ReactToSound(Vector3 soundPos)
    {
        reactingToSound = true;
        agent.isStopped = true;

        // Turn towards sound
        Vector3 dir = (soundPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        float elapsed = 0f;

        while (elapsed < 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);
        agent.isStopped = false;
        reactingToSound = false;
    }

    private void AttackPlayer()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            if (projectilePrefab && projectileSpawnPoint)
            {
                GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
                Rigidbody rb = proj.GetComponent<Rigidbody>();

                if (rb)
                {
                    Vector3 dir = (player.position - projectileSpawnPoint.position).normalized;
                    rb.AddForce(dir * projectileForce, ForceMode.VelocityChange);
                }
            }
        }
    }



    // -------- Damage + Death ----------
    public void TakeDamage(int amount, Vector3 hitForce)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die(hitForce);
        }
    }

    private void Die(Vector3 hitForce)
    {
        isDead = true;
        agent.enabled = false;
        Destroy(gameObject);

        EnemyManager.Instance?.EnemyDied();
    }
}

