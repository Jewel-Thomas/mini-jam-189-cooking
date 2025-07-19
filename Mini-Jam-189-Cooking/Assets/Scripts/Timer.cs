using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeLeft;
    public bool isRunning = false;
    public TextMeshProUGUI timerText;

    void Update()
    {
        if (!isRunning) return;

        timeLeft -= Time.deltaTime;
        timerText.text = Mathf.Ceil(timeLeft).ToString();

        if (timeLeft <= 0)
        {
            isRunning = false;
            GameManager.Instance.EndRound(false); // Round failed
        }
    }

    public void StartTimer(float duration)
    {
        timeLeft = duration;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
