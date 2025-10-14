using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public static SaveLoad I;

    public PlayerProgress playerProgress;

    private string playerSaveName = "PlayerSave";

    private void Awake()
    {
        I = this;

        if (!PlayerPrefs.HasKey(playerSaveName))
        {
            playerProgress = new PlayerProgress();
            playerProgress.currentLevel = 0;
            playerProgress.secondLevelListIndex = 0;
            
            SaveToJson();
        }
        else
        {
            LoadFromJson();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveToJson();
        }
    }


    #region SAVE/LOAD
    public void SaveToJson()
    {
        string content = JsonUtility.ToJson(playerProgress, false);

        PlayerPrefs.SetString(playerSaveName, content);
        PlayerPrefs.Save();
    }

    private void LoadFromJson()
    {
        if (PlayerPrefs.HasKey(playerSaveName))
        {
            string content = PlayerPrefs.GetString(playerSaveName);
            playerProgress = JsonUtility.FromJson<PlayerProgress>(content);
        }
        else
        {


            SaveToJson();
        }
    }
    #endregion SAVE/LOAD
}
