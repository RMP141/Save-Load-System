[System.Serializable]
public struct GameData
{
    public float points;
    public float gameTime;

    public GameData(float points, float gameTime)
    {
        this.points = points;
        this.gameTime = gameTime;
    }
}