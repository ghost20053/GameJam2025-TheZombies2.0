using TMPro;
using UnityEngine;

public class GameUIManager
{
    public static GameUIManager Instance { get; private set; }

    [Header("Top Bar UI")]
    public TextMeshProUGUI enemyCounterText;
    // timer script now lives in same TopBar panel
    public Timer timer;

    public void ToggleUI(bool visible)
    {
        if (timer != null)
        {
            timer.ToggleUI(visible);
        }

    }
}