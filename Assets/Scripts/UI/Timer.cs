using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    private bool _isCounting = false;
    private float elapsedTime = 0f;


    private void Update()
    {
        if (_isCounting)
        {
            elapsedTime += Time.deltaTime;
            int minutes = (int)(elapsedTime / 60f);
            int seconds = (int)(elapsedTime % 60f);
            int milliseconds = (int)((elapsedTime * 100f) % 100f);
            timerText.text = $"{minutes:D2}:{seconds:D2}:{milliseconds:D2}";
        }
    }

    public void StartTiming()
    {
        _isCounting = true;
    }

    public void StopTiming()
    {
        _isCounting = false;
    }

    public float GetTime()
    {
        return elapsedTime;
    }
}
