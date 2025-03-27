using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using static UnityEditor.FilePathAttribute;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    public GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;

    public static DataPersistenceManager Instance {  get; private set; }
    private FileDataHandler dataHandler;

    private void Awake()
    {
        if (Instance != null)
        {
            // Debug.LogError("More than one found");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }

    public static DataPersistenceManager GetInstance()
    {
        return Instance;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }


    public void NewGame()
    {
        if(this.gameData==null)
        {
            Debug.Log("game data is empty, creating new game");
            this.gameData = new GameData();
        }
        else{
            this.gameData.location = Vector3.zero;
            this.gameData.roundNumber = 1;
            this.gameData.money = 0;
            this.gameData.ammo = 7;
            this.gameData.ammoMax = 35;
            this.gameData.ammoTwo = 0;
            this.gameData.ammoMaxTwo = 0;
            this.gameData.hasJug = false;
            this.gameData.hasSpeedCola = false;
            this.gameData.hasDoubleTap = false;
            this.gameData.hasQuickRevive = false;
            this.gameData.hasDeadshot = false;
            this.gameData.firstGun = null;
            this.gameData.secondGun = null;
            this.gameData.lethalCount = 0;
            this.gameData.tacticalCount = 0;
        }
        dataHandler.Save(gameData);
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        //TODO
        if (this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        if (this.gameData == null)
        {
            return;
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        Debug.Log("saving?");
        if(this.gameData == null)
        {
            return;
        }

        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        dataHandler.Save(gameData);
        Debug.Log("Game saved");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }
}
