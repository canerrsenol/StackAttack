using Lean.Touch;
using UnityEngine;

public class TapToStart : MonoBehaviour
{
    private GameManager gameManager;

    void Awake()
    {
        gameManager = GameManager.Instance;
    }

    void OnEnable()
    {
        gameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable()
    {
        gameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.Initialized)
        {
            LeanTouch.OnFingerTap += OnFingerTap;
        }
    }

    private void OnFingerTap(LeanFinger finger)
    {
        gameManager.ChangeGameState(GameState.Started);
        LeanTouch.OnFingerTap -= OnFingerTap;
    }
}
