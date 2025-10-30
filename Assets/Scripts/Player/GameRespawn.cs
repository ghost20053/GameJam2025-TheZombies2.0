using UnityEngine;

public class GameRespawn : MonoBehaviour
{
    public float threshold;

    public Vector3 spawnPoint;

    private void Update()
    {
        if(transform.position.y < threshold)
        {
            transform.position = spawnPoint;
            GameManager.health -= 1;
        }
    }
}
