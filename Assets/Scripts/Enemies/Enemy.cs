using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent enemy;
    public GameObject Player;

    [SerializeField] public float speed;

    private WaveSpawner waveSpawner;

    public float countdown = 5f;

    private void Start()
    {
        waveSpawner = GetComponentInParent<WaveSpawner>();
    }

    void Update()
    { 

        enemy.SetDestination(Player.transform.position * speed * Time.deltaTime);

        countdown -= Time.deltaTime;

        if (countdown <= 0)
        {
            waveSpawner.waves[waveSpawner.currentWaveIndex].enemiesLeft--;
        }
    }
}
