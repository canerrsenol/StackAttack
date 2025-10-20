using UnityEngine;

public class FinishArea : MonoBehaviour
{
    [SerializeField] private GlobalEventsSO globalEventsSO;
    private GameManager gameManager;

    void Awake()
    {
        gameManager = GameManager.Instance;
    }

    void OnEnable()
    {
        globalEventsSO.PlayerEvents.zPositionChanged += OnZPositionChanged;
        gameManager.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDisable()
    {
        globalEventsSO.PlayerEvents.zPositionChanged -= OnZPositionChanged;
        gameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Initialized)
        {
            globalEventsSO.UIEvents.LevelProgressChanged?.Invoke(0f);
        }
    }

    private void OnZPositionChanged(float playerCurrentZ)
    {
        float finishZ = transform.position.z;
        float progress = Mathf.InverseLerp(0f, finishZ, playerCurrentZ);
        globalEventsSO.UIEvents.LevelProgressChanged?.Invoke(progress);

        if(progress >= 1f)
        {
            gameManager.ChangeGameState(GameState.Win);
        }
    }
}