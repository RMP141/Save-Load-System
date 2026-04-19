using System.IO;
using UnityEngine;

public class JsonSaveSystem : ISaveSystem
{
    private string filePath;

    public JsonSaveSystem()
    {
        filePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    public void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
        Debug.Log($"[JsonSave] Saved to {filePath}");
    }

    public GameData Load()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        return new GameData(0f, 0f);
    }
}