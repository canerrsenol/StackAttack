using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager I;
    private LevelData currentLevelData;
    public LevelData CurrentLevelData => currentLevelData;
    [SerializeField] private LevelData[] levelList;
    [SerializeField] private LevelData[] secondLevelList;
    private GameObject levelContent;
    public GameObject LevelContent => levelContent;
    public int LevelCount => levelList.Length;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        LoadLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.ChangeGameState(GameState.Win);
        }
    }

    public void LoadNextLevel()
    {
        SaveLoad.I.playerProgress.currentLevel++;
        SaveLoad.I.SaveToJson();

        Debug.Log("Loading next level: " + SaveLoad.I.playerProgress.currentLevel);
        
        LoadLevel();
    }

    public void LoadLevel()
    {
        StartCoroutine(LoadLevelRoutine());
    }

    private IEnumerator LoadLevelRoutine()
    {
        if (levelContent != null)
        {
            DestroyOldContent();
            yield return new WaitForSeconds(.05f);
        }

        CreateContent();
        yield return new WaitForSeconds(.05f);
        GameManager.Instance.ChangeGameState(GameState.Initialized);
    }

    private void DestroyOldContent()
    {
        if (levelContent != null)
        {
            Destroy(levelContent);
        }
    }

    private void CreateContent()
    {
        if (SaveLoad.I.playerProgress.currentLevel >= levelList.Length)
        {
            currentLevelData =
                secondLevelList[GetContentIndexFromSecondList(SaveLoad.I.playerProgress.secondLevelListIndex)];
        }
        else
        {
            currentLevelData = levelList[GetContentIndexFromLevelList(SaveLoad.I.playerProgress.currentLevel)];
        }

        levelContent = Instantiate(currentLevelData.gameObject);
    }

    public LevelData GetContentDataFromLevelList(int level)
    {
        if (level >= levelList.Length)
        {
            return levelList[level % levelList.Length];
        }

        return levelList[level];
    }

    public LevelData GetContentDataFromSecondList(int level)
    {
        if (level >= secondLevelList.Length)
        {
            return secondLevelList[level % secondLevelList.Length];
        }

        return secondLevelList[level];
    }

    public int GetContentIndexFromLevelList(int level)
    {
        if (level >= levelList.Length)
        {
            return level % levelList.Length;
        }

        return level;
    }

    public int GetContentIndexFromSecondList(int level)
    {
        if (level >= secondLevelList.Length)
        {
            return level % secondLevelList.Length;
        }

        return level;
    }
}