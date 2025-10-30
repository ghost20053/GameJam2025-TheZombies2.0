using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restartlevel()
    {
        Time.timeScale = 1;
        GameManager.health = 4;
        SceneManager.LoadScene(0);
    }
}
