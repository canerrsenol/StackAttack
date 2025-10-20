using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelEnd : MonoBehaviour
{
    [SerializeField] private GlobalEventsSO globalEventsSO;
    [SerializeField] private Image levelEndFillImage;

    void OnEnable()
    {
        globalEventsSO.UIEvents.LevelProgressChanged += OnLevelProgressChanged;
    }

    private void OnLevelProgressChanged(float fillAmount)
    {
        levelEndFillImage.fillAmount = fillAmount;
    }

    void OnDisable()
    {
        globalEventsSO.UIEvents.LevelProgressChanged -= OnLevelProgressChanged;
    }
    
    public void OnRestartButtonClick()
    {
        LevelManager.I.LoadLevel();
    }

    public void OnNextButtonClick()
    {
        LevelManager.I.LoadNextLevel();
    }
}
