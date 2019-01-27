using UnityEngine;

public class BulletHellDesigner : MonoBehaviour
{
    void Start()
    {
        // activate full bullet hell
        BulletHellManager[] bhm = FindObjectsOfType<BulletHellManager>();
        foreach (BulletHellManager bh in bhm)
        {
            bh.enabled = true;
        }
    }

    void Update()
    {

    }
}
