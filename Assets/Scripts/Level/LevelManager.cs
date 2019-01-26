﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelManager : MonoBehaviour
{
	public static LevelManager s;

    public Transform startingPoint;
    public int tileSize = 25;
    public int roadTileCount = 30;
    public int sideTileCount = 5;
    public float roadSpawnDelay = 1f;

    private Level level;
    private GameObject roadObject;
    private GameObject sideObject;
    private GameObject root;
    private float startTime;
    private float nextRowTime;
    private int rowIdx;
    private float deleteTimeout = 20;
    private float horizonZ;

	void Awake()
	{
		s = this;
	}
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
            if (Time.time > nextRowTime)
            {
                CreateRow(rowIdx);
                rowIdx += 1;

                nextRowTime = Time.time + roadSpawnDelay;
            }

            foreach (LevelSequence seq in level.sequences)
            {
                if (Time.time > startTime + seq.initialDelay)
                {
                    if (Time.time > seq.nextSpawnTime)
                    {
                        Vector3 pos = new Vector3(startingPoint.position.x + Random.Range((float)-sideTileCount, (float)sideTileCount) * (float)tileSize, startingPoint.position.y, startingPoint.position.z + rowIdx * tileSize);

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

    private void CreateRow(int rowIdx)
    {
        Vector3 pos = new Vector3(startingPoint.position.x, startingPoint.position.y, startingPoint.position.z + rowIdx * tileSize);

        GameObject obj = Instantiate(roadObject, pos, roadObject.transform.rotation);
        obj.transform.parent = root.transform;
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
        deleteTimeout = 25;

        root = new GameObject("LevelRuntime");
        root.transform.position = startingPoint.position;
        root.transform.rotation = startingPoint.rotation;

        roadObject = Resources.Load("Environment/" + level.artSet + "/road1", typeof(GameObject)) as GameObject;
        sideObject = Resources.Load("Environment/" + level.artSet + "/side1", typeof(GameObject)) as GameObject;

        for (rowIdx = 0; rowIdx < roadTileCount + 1; rowIdx++)
        {
            CreateRow(rowIdx);
        }
    }
}
