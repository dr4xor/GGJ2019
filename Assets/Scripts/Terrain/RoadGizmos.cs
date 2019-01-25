using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RoadGizmos : MonoBehaviour
{
    Road road;

    void Start()
    {
        road = GetComponent<Road>();
    }

    private void OnDrawGizmos()
    {
        if (road.terrainPoints == null)
            return;
        Gizmos.color = Color.black;
        for (int i = 0; i < road.terrainPoints.Length; i++)
        {
            Gizmos.DrawSphere(road.terrainPoints[i], 0.05f);
        }

        Vector3 br = transform.position;
        Vector3 tl = transform.position + transform.localScale;

        Debug.DrawLine(new Vector3(br.x, br.y, br.z), new Vector3(br.x, br.y, tl.z));
        Debug.DrawLine(new Vector3(br.x, br.y, br.z), new Vector3(tl.x, br.y, br.z));
        Debug.DrawLine(new Vector3(tl.x, br.y, tl.z), new Vector3(br.x, br.y, tl.z));
        Debug.DrawLine(new Vector3(tl.x, br.y, tl.z), new Vector3(tl.x, br.y, br.z));
    }
}
