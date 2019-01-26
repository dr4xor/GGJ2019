[System.Serializable]
public class LevelSequence
{
    public int initialDelay;
    public int minSpawnDelay;
    public int maxSpawnDelay;
    public string[] objects;

    // runtime properties
    public float nextSpawnTime;
}