using UnityEngine;

public class HighScoreScript : MonoBehaviour
{

    public static HighScoreScript Instance;
    public static float TimeAlive;
    public static float OldTimeAlive;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        TimeAlive = 0f;
        OldTimeAlive = 0f;
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }




}
