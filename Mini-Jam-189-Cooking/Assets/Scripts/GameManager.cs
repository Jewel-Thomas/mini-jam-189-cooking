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
    public GameObject winPanel;
    public GameObject losePanel;

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
        // Reset ingredients and timer UI for new round
        List<string> newDish = DishDatabase.GetRandomDish();
        dishManager.SetNewDish(newDish);
        roundTimer.SetTime(roundDuration);
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
        timeDisplay.text = "Day Hour : " + currentHour + ":00";
    }

    void WinGame()
    {
        Time.timeScale = 0; // Pause the game
        winPanel.SetActive(true); // Show win panel
    }
    void LoseGame()
    { 
        Time.timeScale = 0; // Pause the game
        losePanel.SetActive(true); // Show lose panel    
    }

    public void RetartGame()
    {
        Time.timeScale = 1; // Resume game time
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        currentHour = 10; // Reset to 10 AM
        UpdateTimeDisplay();
        StartNewRound();
    }    

    // Call this from DishManager when dish is assembled successfully
    public void OnDishAssembled()
    {
        EndRound(true);
    }
}
