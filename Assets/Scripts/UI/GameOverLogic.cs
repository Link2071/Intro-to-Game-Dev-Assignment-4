using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverLogic : MonoBehaviour
{
    [SerializeField] private GameObject gameOverDisplay;
    [SerializeField] private Timer timer;
    [SerializeField] private TMP_Text score;

    public void GameOver()
    {
        gameOverDisplay.SetActive(true);
        PlayerController.instance.SetMovementState(false);
        GhostManager.instance.SetGhostsCanMove(false);
        Invoke("ReturnToMenu", 3);

        int highScore = PlayerPrefs.GetInt("HighScore", -1);
        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        int currentScore = int.Parse(score.text);
        float currentTime = timer.GetTime();

        if (currentScore > highScore || (currentScore == highScore && currentTime < bestTime))
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.SetFloat("BestTime", currentTime);
            PlayerPrefs.Save();
        }

        timer.StopTiming();
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
