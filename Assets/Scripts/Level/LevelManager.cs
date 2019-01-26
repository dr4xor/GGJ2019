using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelManager : MonoBehaviour
{
    public Transform startingPoint;
    public int tileSize = 25;
    public int roadTileCount = 30;
    public int sideTileCount = 5;
    public float speed = -0.2f;

    private Level level;
    private GameObject roadObject;
    private GameObject sideObject;
    private GameObject root;
    private Transform lastRoad;
    private float startTime;
    private float deleteTimeout = 20;
    private float horizonZ;

    void Start()
    {
        horizonZ = startingPoint.position.z + roadTileCount * tileSize;

        TextAsset text = Resources.Load("Level-1") as TextAsset;
        level = JsonUtility.FromJson<Level>(text.text);

        CreateInitialLevel();
    }

    void Update()
    {
        if (startTime > 0)
        {
            root.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (lastRoad.transform.position.z + tileSize < horizonZ)
            {
                CreateRow(new Vector3(0, 0, lastRoad.transform.position.z + tileSize), 0);
            }

            foreach (LevelSequence seq in level.sequences)
            {
                if (Time.time > startTime + seq.initialDelay)
                {
                    if (Time.time > seq.nextSpawnTime)
                    {
                        Vector3 pos = new Vector3(startingPoint.position.x + Random.Range((float)-sideTileCount, (float)sideTileCount) * (float)tileSize, startingPoint.position.y, startingPoint.position.z + roadTileCount * tileSize);

                        int idx = Random.Range(0, seq.objects.Length);
                        GameObject obj = Resources.Load("Environment/" + level.artSet + "/" + seq.objects[idx], typeof(GameObject)) as GameObject;
                        obj = Instantiate(obj, pos, obj.transform.rotation);
                        obj.transform.parent = root.transform;
                        Destroy(obj, deleteTimeout);

                        seq.nextSpawnTime = Time.time + Random.Range(seq.minSpawnDelay, seq.maxSpawnDelay) / 1000f;
                    }
                }
            }
        }
    }

    private void CreateRow(Vector3 pos, int rowIdx)
    {
        GameObject obj = Instantiate(roadObject, pos, roadObject.transform.rotation);
        obj.transform.parent = root.transform;
        lastRoad = obj.transform;
        Destroy(obj, deleteTimeout);

        for (int i2 = -sideTileCount; i2 <= sideTileCount; i2++)
        {
            if (i2 != 0)
            {
                Vector3 newPos = new Vector3(pos.x + i2 * tileSize, pos.y, pos.z);

                obj = Instantiate(sideObject, newPos, sideObject.transform.rotation);
                obj.transform.parent = root.transform;
                Destroy(obj, deleteTimeout);
            }
        }
    }

    private void CreateInitialLevel()
    {
        startTime = Time.time + 1;
        deleteTimeout = 20; // TODO: calc this better, something along maybe Mathf.Abs(speed) * tileSize;

        root = new GameObject("LevelRuntime");
        root.transform.position = startingPoint.position;
        root.transform.rotation = startingPoint.rotation;

        roadObject = Resources.Load("Environment/" + level.artSet + "/road1", typeof(GameObject)) as GameObject;
        sideObject = Resources.Load("Environment/" + level.artSet + "/side1", typeof(GameObject)) as GameObject;

        for (int i = 0; i < roadTileCount + 1; i++)
        {
            Vector3 newPos = new Vector3(startingPoint.position.x, startingPoint.position.y, startingPoint.position.z + i * tileSize);
            CreateRow(newPos, i);
        }
    }
}
