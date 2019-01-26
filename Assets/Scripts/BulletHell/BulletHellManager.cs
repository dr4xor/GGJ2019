using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellManager : MonoBehaviour
{
    public Bullet bullet;
    public string spawnFrequency;
    public string rotationFrequency;
    public float speed;

    private float nextSpawnAction = float.MaxValue;
    private float nextRotateAction = float.MaxValue;
    private PatternParser spawnPattern;
    private PatternParser rotatePattern;

    private void UpdateSpawnPattern(string value)
    {
        spawnPattern = new PatternParser(spawnFrequency, true);
        if (spawnPattern.GetNextExecution() != null)
        {
            nextSpawnAction = Time.time + (float)spawnPattern.GetNextExecution() / 1000;
        }
    }

    private void UpdateRotatePattern(string value)
    {
        rotatePattern = new PatternParser(rotationFrequency, true);
        if (rotatePattern.GetNextExecution() != null)
        {
            nextRotateAction = Time.time + (float)rotatePattern.GetNextExecution() / 1000;
        }
    }

    void Awake()
    {
        UpdateSpawnPattern(spawnFrequency);
        UpdateRotatePattern(rotationFrequency);
    }

    void Update()
    {
        if (Time.time >= nextSpawnAction)
        {
            string action = spawnPattern.GetNextAction();
            switch (action)
            {
                case "delay":
                    break;

                default:
                    Bullet obj = Instantiate<Bullet>(bullet, transform.position, transform.rotation);
                    obj.Launch(speed);
                    break;

            }

            if (spawnPattern.Next() != null)
            {
                nextSpawnAction = Time.time + (float)spawnPattern.GetNextExecution() / 1000;
            }
            else
            {
                nextSpawnAction = float.MaxValue;
            }
        }

        if (Time.time >= nextRotateAction)
        {
            string action = rotatePattern.GetNextAction();
            switch (action)
            {
                case "delay":
                    break;

                default:
                    int degrees = int.Parse(action);
                    transform.Rotate(new Vector3(0, degrees, 0));
                    break;
            }

            if (rotatePattern.Next() != null)
            {
                nextRotateAction = Time.time + (float)rotatePattern.GetNextExecution() / 1000;
            }
            else
            {
                nextRotateAction = float.MaxValue;
            }
        }
    }
}
