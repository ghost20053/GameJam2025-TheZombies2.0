using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

public class ProjectileEnemy : MonoBehaviour
{
    public NavMeshAgent projectileEnemy;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Attacking
    public float timeBetweenAttacks;
    public bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;


    [SerializeField] public float moveSpeed;

    private WaveSpawner waveSpawner;

    public float countdown = 5f;

    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
        projectileEnemy = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        waveSpawner = GetComponentInParent<WaveSpawner>();

        ChasePlayer();
    }

    void Update()
    {
        //check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

        //transform.Translate(transform.position * moveSpeed * Time.deltaTime);

        countdown -= Time.deltaTime;

        if (countdown <= 0)
        {
            waveSpawner.waves[waveSpawner.currentWaveIndex].enemiesLeft--;
        }

    }

    private void ChasePlayer()
    {
        projectileEnemy.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesnt move

        //projectileEnemy.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
