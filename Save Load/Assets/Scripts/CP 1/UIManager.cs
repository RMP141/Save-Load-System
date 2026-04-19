using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI timeText;

    private void Start()
    {
        GameManager.Instance.OnStatsUpdated += UpdateUI;
        UpdateUI(0f, 0f);
    }

    private void UpdateUI(float points, float gameTime)
    {
        pointsText.text = $"Очки: {Mathf.FloorToInt(points)}";
        timeText.text = $"Время: {FormatTime(gameTime)}";
    }

    private string FormatTime(float seconds)
    {
        int totalSeconds = Mathf.FloorToInt(seconds);
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int secs = totalSeconds % 60;
        return $"{hours:D2}:{minutes:D2}:{secs:D2}";
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStatsUpdated -= UpdateUI;
    }
}