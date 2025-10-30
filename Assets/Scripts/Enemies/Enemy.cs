using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float speed;

    private WaveSpawner waveSpawner;

    public float countdown = 5f;

    private void Start()
    {
        waveSpawner = GetComponentInParent<WaveSpawner>();
    }

    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);

        countdown -= Time.deltaTime;

        if (countdown <= 0)
        {
            waveSpawner.waves[waveSpawner.currentWaveIndex].enemiesLeft--;
        }
    }
}
