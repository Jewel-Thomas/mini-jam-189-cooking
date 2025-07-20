using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentHour = 10; // Start at 10 AM
    public TextMeshProUGUI timeDisplay;
    public float roundDuration = 10f;
    public Timer roundTimer;
    public DishManager dishManager;
    public MrClock mrClock;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartNewRound();
    }


    public void StartNewRound()
    {
        List<string> newDish = DishDatabase.GetRandomDish();
        dishManager.SetNewDish(newDish);
        roundTimer.StartTimer(roundDuration, OnRoundTimerEnd);
    }

    // Called when the round timer runs out
    private void OnRoundTimerEnd()
    {
        EndRound(false); // Failed to assemble dish in time
    }

    public void EndRound(bool success)
    {
        roundTimer.StopTimer();
        currentHour += success ? 1 : -1;
        UpdateTimeDisplay();

        if (currentHour >= 19) WinGame();
        else if (currentHour < 10) LoseGame();
        else Invoke(nameof(StartNewRound), 1.5f);
    }

    void UpdateTimeDisplay()
    {
        timeDisplay.text = currentHour + ":00";
    }

    void WinGame() { /* Show win screen */ }
    void LoseGame() { /* Show lose screen */ }

    // Call this from DishManager when dish is assembled successfully
    public void OnDishAssembled()
    {
        EndRound(true);
    }
}
