using UnityEngine;

public class UILevelEnd : MonoBehaviour
{
    [SerializeField] private GlobalEventsSO globalEventsSO;
    public void OnRestartButtonClick()
    {
        LevelManager.I.LoadLevel();
    }

    public void OnNextButtonClick()
    {
        LevelManager.I.LoadNextLevel();
    }
}
