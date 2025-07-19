using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentHour = 9;
    public TextMeshProUGUI timeDisplay;
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
        mrClock.ThrowIngredients(newDish);
        roundTimer.StartTimer(10f); // e.g., 10 seconds for each round
    }

    public void EndRound(bool success)
    {
        roundTimer.StopTimer();
        currentHour += success ? 1 : -1;
        UpdateTimeDisplay();

        if (currentHour >= 17) WinGame();
        else if (currentHour < 9) LoseGame();
        else Invoke(nameof(StartNewRound), 1.5f);
    }

    void UpdateTimeDisplay()
    {
        timeDisplay.text = currentHour + ":00";
    }

    void WinGame() { /* Show win screen */ }
    void LoseGame() { /* Show lose screen */ }
}
