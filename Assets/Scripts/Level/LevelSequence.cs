[System.Serializable]
public class LevelSequence
{
    public int initialDelay;
    public int duration;
    public bool onRoads = true;
    public bool onSides = true;
    public int minSpawnDelay;
    public int maxSpawnDelay;
    public int minRotation;
    public int maxRotation;
    public string[] objects;

    // runtime properties
    public float firstSpawnTime;
    public float nextSpawnTime;
}