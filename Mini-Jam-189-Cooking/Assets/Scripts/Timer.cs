using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    // Set the timer value and update the display immediately
    public void SetTime(float duration)
    {
        timeLeft = duration;
        timerText.text = "RoundTime : " + Mathf.Ceil(timeLeft).ToString();
    }
    public float timeLeft;
    public bool isRunning = false;
    public TextMeshProUGUI timerText;
    private System.Action onTimerEnd;

    void Update()
    {
        if (!isRunning) return;

        timeLeft -= Time.deltaTime;
        timerText.text = "RoundTime : " + Mathf.Ceil(timeLeft).ToString();

        if (timeLeft <= 0)
        {
            isRunning = false;
            onTimerEnd?.Invoke();
        }
    }

    public void StartTimer(float duration, System.Action onEnd = null)
    {
        timeLeft = duration;
        isRunning = true;
        onTimerEnd = onEnd;
    }

    public void StopTimer()
    {
        isRunning = false;
        onTimerEnd = null;
    }
}
