using TMPro;
using UnityEngine;

public class LoadHighScore : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text bestTimeText;

    void Awake()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        float bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
        int minutes = (int)(bestTime / 60);
        int seconds = (int)(bestTime % 60);
        int milliseconds = (int)((bestTime * 100) % 100);
        highScoreText.text = $"High Score: {highScore}";
        bestTimeText.text = $"Best Time: {minutes:D2}:{seconds:D2}:{milliseconds:D2}";

    }
}
