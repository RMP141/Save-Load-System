using UnityEngine;

public class PlayerPrefsSaveSystem : ISaveSystem
{
    private const string POINTS_KEY = "Points";
    private const string TIME_KEY = "GameTime";

    public void Save(GameData data)
    {
        PlayerPrefs.SetFloat(POINTS_KEY, data.points);
        PlayerPrefs.SetFloat(TIME_KEY, data.gameTime);
        PlayerPrefs.Save();
        Debug.Log("[PlayerPrefs] Saved");
    }

    public GameData Load()
    {
        float points = PlayerPrefs.GetFloat(POINTS_KEY, 0f);
        float time = PlayerPrefs.GetFloat(TIME_KEY, 0f);
        return new GameData(points, time);
    }
}