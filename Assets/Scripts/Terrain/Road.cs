using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{

    [SerializeField]
    private float bumpiness = 0.1f;

    [SerializeField]
    private int tilesWide = 1;

    [SerializeField]
    private int tilesLong = 1;

    public Vector3[] terrainPoints;

    private Mesh mesh;

    void Start()
    {

        float tileWidth = transform.localScale.x / tilesWide;
        float tileLength = transform.localScale.z / tilesLong;



        terrainPoints = new Vector3[(tilesWide + 1) * (tilesLong + 1)];
        Vector2[] uv = new Vector2[terrainPoints.Length];
        for (int y = 0; y <= tilesLong; y++)
        {
            for (int x = 0; x <= tilesWide; x++)
            {
                terrainPoints[x + y * (tilesWide + 1)] = new Vector3(tileWidth * x, Random.Range(0, bumpiness), tileLength * y);
                uv[x + y * (tilesWide + 1)] = new Vector2((float)x / tilesWide, (float)y / tilesLong);
            }
        }

        mesh = new Mesh();
        mesh.name = "bumpy road";
        mesh.vertices = terrainPoints;
        mesh.uv = uv;

        GetComponent<MeshFilter>().mesh = mesh;

        int[] triangles = new int[tilesLong * tilesWide * 6];
        for (int y = 0; y < tilesLong; y++)
        {
            for (int x = 0; x < tilesWide; x++)
            {
                triangles[(x + y * tilesWide) * 6 + 0] = (x + 0) + (y + 0) * (tilesWide + 1);
                triangles[(x + y * tilesWide) * 6 + 1] = (x + 0) + (y + 1) * (tilesWide + 1);
                triangles[(x + y * tilesWide) * 6 + 2] = (x + 1) + (y + 0) * (tilesWide + 1);
                triangles[(x + y * tilesWide) * 6 + 3] = (x + 0) + (y + 1) * (tilesWide + 1);
                triangles[(x + y * tilesWide) * 6 + 4] = (x + 1) + (y + 1) * (tilesWide + 1);
                triangles[(x + y * tilesWide) * 6 + 5] = (x + 1) + (y + 0) * (tilesWide + 1);
            }
        }

        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
