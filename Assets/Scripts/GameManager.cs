using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public PlayerData PlayerData { get; private set; }

    public List<string> GameNameList = new List<string>();


    public bool IsGamePaused = false;

    public event EventHandler OnGamePauseChanged;


   



    public void Pause()
    {
        IsGamePaused= true;
        Time.timeScale= 0;
        OnGamePauseChanged?.Invoke(this, EventArgs.Empty);
    }
    public void Unpause()
    {
        IsGamePaused = false;
        Time.timeScale = 1f;
        OnGamePauseChanged?.Invoke(this, EventArgs.Empty);
    }



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);


        SceneManager.sceneLoaded += OnSceneLoaded;

        if (PlayerPrefs.HasKey("GameNameList"))
        {
            string gameListData = PlayerPrefs.GetString("GameNameList");

            GameNameList = gameListData.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList<string>();

        }



    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name != SceneLoader.Scene.MainMenu.ToString())
        {
            SaveGame();
        }
    }

    private void InitPlayerData()
    {
        PlayerData = new PlayerData();
        PlayerData.Coin = 0;
        PlayerData.Health = PlayerData.MAX_HEALTH;
        PlayerData.Name = DateTime.Now.ToString("G");
        PlayerData.SceneLevel = SceneLoader.Scene.Level1;
    }



    public void NewGame()
    {
        InitPlayerData();

        GameNameList.Add(PlayerData.Name);
        SceneLoader.LoadScene(SceneLoader.Scene.Level1);
    }

    public void SaveGame()
    {
        if (PlayerData == null)
        {
            Debug.LogWarning("PlayerData is null. Cannot save game.");

            InitPlayerData();
            return;
        }

        if (!GameNameList.Contains(PlayerData.Name))
        {
            GameNameList.Add(PlayerData.Name);
        }
        SaveGameList();
        SavePlayerData();


        PlayerPrefs.Save();
    }
    private void SaveGameList()
    {
        string gameListData = string.Join(',', GameNameList);
        PlayerPrefs.SetString("GameNameList", gameListData);
    }
    private void SavePlayerData()
    {
        string data = JsonUtility.ToJson(PlayerData);
        PlayerPrefs.SetString(PlayerData.Name, data);
    }

    public void LoadGame(string gameName)
    {
        if (PlayerPrefs.HasKey(gameName))
        {
            string data = PlayerPrefs.GetString(gameName);
            PlayerData = JsonUtility.FromJson<PlayerData>(data);
        }
        else
        {
            InitPlayerData();
        }

        SceneLoader.LoadScene(PlayerData.SceneLevel);
    }

    public void DeleteSavedGames(string gameName)
    {

        GameNameList.Remove(gameName);
        SaveGameList();

        PlayerPrefs.DeleteKey(gameName);

        PlayerPrefs.Save();
    }
}
