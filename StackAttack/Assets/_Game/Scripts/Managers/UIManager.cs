using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GlobalEventsSO globalEventsSO;
    [SerializeField] private GameObject _levelEndBackground;
    [SerializeField] private GameObject _winPanel, _losePanel, _inGamePanel;
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
                _winPanel.SetActive(false);
                _losePanel.SetActive(false);
                _levelEndBackground.SetActive(false);
                break;
            case GameState.Started:
                break;
            case GameState.Win:
                DOVirtual.DelayedCall(1f, () =>
                {
                    _winPanel.SetActive(true);
                    _levelEndBackground.SetActive(true);
                });
                break;
            case GameState.Lose:
                DOVirtual.DelayedCall(.1f, () =>
                {
                    _losePanel.SetActive(true);
                    _levelEndBackground.SetActive(true);
                });
                break;
        }
    }
}