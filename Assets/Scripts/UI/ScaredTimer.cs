using TMPro;
using UnityEngine;

public class ScaredTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    private float _scaredTime = 10f;
    private float _timer = 0f;

    void Update()
    {
        _timer -= Time.deltaTime;
        int miliseconds = (int)(_timer * 100) % 100;
        int seconds = (int)_timer;

        timerText.text = $"{seconds:D2}:{miliseconds:D2}";
    }

    public void ResetTimer()
    {
        _timer = _scaredTime;
    }
}
