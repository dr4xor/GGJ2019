using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager s;

    public Transform startingPoint;
    public int tileSize = 25;
    public int roadTileCount = 30;
    public int sideTileCount = 5;
    public float roadSpawnDelay = 1f;

    private Level level;
    private AudioClip music;
    private GameObject[] roadObjects;
    private GameObject[] sideObjects;
    private GameObject root;
    private float startTime;
    private float nextRowTime;
    private int rowIdx;
    private float deleteTimeout = 20;
    private Dictionary<string, GameObject> cache = new Dictionary<string, GameObject>();

    void Awake()
    {
        s = this;
    }

    public Level LoadLevel(int idx)
    {
        TextAsset text = Resources.Load("Level-" + idx) as TextAsset;
        level = JsonUtility.FromJson<Level>(text.text);

        CreateInitialLevel();
        music = Resources.Load("Environment/" + level.artSet + "/" + level.music) as AudioClip;

        startTime = Time.time + 0.01f;

        return level;
    }

    public AudioClip getMusic()
    {
        return music;
    }

    void Update()
    {
        if (startTime > 0)
        {
            if (Time.time > nextRowTime)
            {
                CreateRow(rowIdx);
                rowIdx += 1;

                nextRowTime = Time.time + roadSpawnDelay;
            }

            foreach (LevelSequence seq in level.sequences)
            {
                if (Time.time > startTime + seq.initialDelay / 1000 && (seq.duration == 0 || Time.time < startTime + seq.initialDelay / 1000 + seq.duration / 1000))
                {
                    if (Time.time > seq.nextSpawnTime)
                    {
                        // find valid random position
                        bool validPosition = false;
                        Vector3 pos = Vector3.zero;
                        while (!validPosition)
                        {
                            pos = new Vector3(startingPoint.position.x + Random.Range((float)-sideTileCount, (float)sideTileCount) * (float)tileSize, startingPoint.position.y, startingPoint.position.z + rowIdx * tileSize);

                            if (seq.onRoads && !seq.onSides)
                            {
                                validPosition = pos.x >= 0 && pos.x <= tileSize;
                            }
                            if (seq.onSides && !seq.onRoads)
                            {
                                validPosition = pos.x < -5f || pos.x > tileSize;
                            }
                        }

                        string objPath;
                        if (seq.objects != null)
                        {
                            // load prop object
                            int idx = Random.Range(0, seq.objects.Length);
                            objPath = "Environment/" + level.artSet + "/" + seq.objects[idx];
                        }
                        else
                        {
                            // load global object
                            int idx = Random.Range(0, seq.globalObjects.Length);
                            objPath = seq.globalObjects[idx];
                        }

                        GameObject obj;
                        if (cache.ContainsKey(objPath))
                        {
                            obj = cache[objPath];
                        }
                        else
                        {
                            obj = Resources.Load(objPath, typeof(GameObject)) as GameObject;
                            cache[objPath] = obj;
                        }
                        if (obj == null)
                        {
                            Debug.Log("Object not found: " + objPath);
                        }
                        obj = Instantiate(obj, pos, obj.transform.rotation);
                        obj.transform.parent = root.transform;

                        // rotate correctly
                        obj.transform.localEulerAngles = new Vector3(obj.transform.localEulerAngles.x, obj.transform.localEulerAngles.y + Random.Range(seq.minRotation, seq.maxRotation), obj.transform.localEulerAngles.z);

                        if (seq.objects != null && !objPath.StartsWith("Waggons/"))
                        {
                            Destroy(obj, deleteTimeout);
                        }

                        seq.nextSpawnTime = Time.time + Random.Range(seq.minSpawnDelay, seq.maxSpawnDelay) / 1000f;
                    }
                }
            }
        }
    }

    private void CreateRow(int rowIdx)
    {
        Vector3 pos = new Vector3(startingPoint.position.x, startingPoint.position.y, startingPoint.position.z + rowIdx * tileSize);

        int roadIdx = Random.Range(0, roadObjects.Length);
        GameObject obj = Instantiate(roadObjects[roadIdx], pos, roadObjects[roadIdx].transform.rotation);
        obj.transform.parent = root.transform;
        Destroy(obj, deleteTimeout);

        for (int i2 = -sideTileCount; i2 <= sideTileCount; i2++)
        {
            if (i2 != 0)
            {
                Vector3 newPos = new Vector3(pos.x + i2 * tileSize, pos.y, pos.z);

                int sideIdx = Random.Range(0, sideObjects.Length);
                obj = Instantiate(sideObjects[sideIdx], newPos, sideObjects[sideIdx].transform.rotation);
                obj.transform.parent = root.transform;
                Destroy(obj, deleteTimeout);
            }
        }
    }

    private void CreateInitialLevel()
    {
        root = new GameObject("LevelRuntime");
        root.transform.position = startingPoint.position;
        root.transform.rotation = startingPoint.rotation;

        // cache prefabs
        roadObjects = new GameObject[level.roads.Length];
        sideObjects = new GameObject[level.sides.Length];
        for (int i = 0; i < level.roads.Length; i++)
        {
            roadObjects[i] = Resources.Load("Environment/" + level.artSet + "/" + level.roads[i], typeof(GameObject)) as GameObject;
        }
        for (int i = 0; i < level.sides.Length; i++)
        {
            sideObjects[i] = Resources.Load("Environment/" + level.artSet + "/" + level.sides[i], typeof(GameObject)) as GameObject;
        }

        // create initial roads
        for (rowIdx = 0; rowIdx < roadTileCount + 1; rowIdx++)
        {
            CreateRow(rowIdx);
        }
    }
}
