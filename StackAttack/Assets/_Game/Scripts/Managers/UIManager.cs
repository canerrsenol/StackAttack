using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GlobalEventsSO globalEventsSO;
    [SerializeField] private GameObject levelEndBackground;
    [SerializeField] private GameObject startPanel, winPanel, losePanel, inGamePanel;
    [SerializeField] private TextMeshProUGUI levelText;
    private GameManager _gameManager;

    private void OnEnable()
    {
        _gameManager = GameManager.Instance;
        _gameManager.OnGameStateChanged += GameStateChanged;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStateChanged -= GameStateChanged;
    }

    private void GameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Initialized:
                levelText.text = "Level " + (SaveLoad.I.playerProgress.currentLevel + 1).ToString();
                startPanel.SetActive(true);
                winPanel.SetActive(false);
                losePanel.SetActive(false);
                levelEndBackground.SetActive(false);
                break;
            case GameState.Started:
                startPanel.SetActive(false);
                inGamePanel.SetActive(true);
                break;
            case GameState.Win:
                DOVirtual.DelayedCall(1f, () =>
                {
                    winPanel.SetActive(true);
                    levelEndBackground.SetActive(true);
                });
                break;
            case GameState.Lose:
                DOVirtual.DelayedCall(.1f, () =>
                {
                    losePanel.SetActive(true);
                    levelEndBackground.SetActive(true);
                });
                break;
        }
    }
}