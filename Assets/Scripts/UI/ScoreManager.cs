using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreBody;
    public int score;
    public static ScoreManager instance { get; private set; }

    void Start()
    {
        if (instance != null && instance != this) Destroy(this);
        instance = this;
    }

    public void UpdateScore(int amount)
    {
        score += amount;
        scoreBody.text = score.ToString("D6");
    }
}
