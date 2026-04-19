using UnityEngine;

public class SaveSystemSelector : MonoBehaviour
{
    public void SetJsonSaveSystem()
    {
        GameManager.Instance.SetSaveSystem(SaveSystemType.Json);
    }

    public void SetPlayerPrefsSaveSystem()
    {
        GameManager.Instance.SetSaveSystem(SaveSystemType.PlayerPrefs);
    }

    public void Save() => GameManager.Instance.SaveGame();
    public void Load() => GameManager.Instance.LoadGame();
}