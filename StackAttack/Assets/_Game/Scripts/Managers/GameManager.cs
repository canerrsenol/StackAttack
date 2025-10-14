using System;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public GameState GameState;
    public event Action<GameState> OnGameStateChanged;
    public GlobalEventsSO globalEvents;

    [SerializeField] private GameObject victoryConfetti;
 
    public void CompleteLevel()
    {
        ChangeGameState(GameState.Win);
    }

    public void ChangeGameState(GameState newGameState)
    {
        if (GameState == newGameState)
            return;
        
        switch (newGameState)
        {
            case GameState.Initialized:
            victoryConfetti.SetActive(false);
                break;
            case GameState.Started:
                break;
            case GameState.Win:
            victoryConfetti.SetActive(true);
                break;
            case GameState.Lose:
                break;
        }
        if (GameState != newGameState)
            OnGameStateChanged?.Invoke(newGameState);
        GameState = newGameState;
    }
}

public enum GameState
{
    Initialized,
    Started,
    Win,
    Lose
}