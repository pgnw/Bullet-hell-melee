using TMPro;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{

    GameObject MainMenu;
    [SerializeField] GameObject MainGame;
    [SerializeField] GameObject GameCanvas;
    [SerializeField] GameObject PrevHighScore;
    [SerializeField] GameObject HighestScore;
    public void Awake()
    {
        MainMenu = gameObject;
        PrevHighScore.GetComponent<TextMeshProUGUI>().text = ($"Previous time alive was: {HighScoreScript.OldTimeAlive}");
        /// HighestScore.GetComponent<TextMeshProUGUI>().text = ($"Highest time alive: {((HighestScore > PrevHighScore) ? HighestScore : PrevHighScore)})
        // fix ^^^^^^^
    }


    public void StartGame()
    {
        MainMenu.SetActive(false);

        GameCanvas.SetActive(true);
        MainGame.SetActive(true);


    }







}