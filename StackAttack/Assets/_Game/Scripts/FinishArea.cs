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

    private void OnZPositionChanged(float playerCurrentZ)
    {
        // Oyuncunun Z pozisyonuna göre ilerleme yüzdesini hesapla oyuncu 0 dan başlayıp FinishArea'nın Z pozisyonuna kadar ilerliyor
        float finishZ = transform.position.z;
        float progress = Mathf.InverseLerp(0f, finishZ, playerCurrentZ);
        fillImage.fillAmount = progress;
    }
}