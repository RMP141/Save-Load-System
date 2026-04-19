using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Click Settings")]
    [SerializeField] private float basePointsPerSecond = 10f;

    [Header("Save System")]
    [SerializeField] private SaveSystemType saveSystemType = SaveSystemType.Json;
    private ISaveSystem saveSystem;


    private float currentPoints;
    private float currentGameTime;

    private bool isHolding = false;
    private float holdDuration = 0f;

    public System.Action<float, float> OnStatsUpdated;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeSaveSystem();
        LoadGame();
    }

    private void InitializeSaveSystem()
    {
        saveSystem = saveSystemType switch
        {
            SaveSystemType.Json => new JsonSaveSystem(),
            SaveSystemType.PlayerPrefs => new PlayerPrefsSaveSystem(),
            _ => new JsonSaveSystem()
        };
    }

    private void Update()
    {
        currentGameTime += Time.deltaTime;

        if (isHolding)
        {
            holdDuration += Time.deltaTime;
            float pointsToAdd = basePointsPerSecond * holdDuration * Time.deltaTime;
            currentPoints += pointsToAdd;
        }

        OnStatsUpdated?.Invoke(currentPoints, currentGameTime);
    }

    public void StartHolding()
    {
        isHolding = true;
        holdDuration = 0f;
    }

    public void StopHolding()
    {
        isHolding = false;
        holdDuration = 0f;
    }

    public void SaveGame()
    {
        GameData data = new GameData(currentPoints, currentGameTime);
        saveSystem.Save(data);
    }

    public void LoadGame()
    {
        GameData data = saveSystem.Load();
        currentPoints = data.points;
        currentGameTime = data.gameTime;
        OnStatsUpdated?.Invoke(currentPoints, currentGameTime);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) SaveGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SetSaveSystem(SaveSystemType type)
    {
        saveSystemType = type;
        InitializeSaveSystem();
    }
}

public enum SaveSystemType
{
    Json,
    PlayerPrefs
}