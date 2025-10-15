using UnityEngine;
using UnityEngine.UI;

public class FinishArea : MonoBehaviour
{
    [SerializeField] private GlobalEventsSO globalEventsSO;
    [SerializeField] private Image fillImage;

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
            fillImage.fillAmount = 0f;
        }
    }

    private void OnZPositionChanged(float progress)
    {
        fillImage.fillAmount = Mathf.Clamp01(progress);
    }
}