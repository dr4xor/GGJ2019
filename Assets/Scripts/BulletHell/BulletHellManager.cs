using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellManager : MonoBehaviour
{
	public Vector3 relativeVelocity;
	public bool IsFriendlyFire;

    public Bullet bullet;
    public string rotationFrequency;
    public string spawnFrequency;
    public float speed;

    private float nextRotateAction = float.MaxValue;
    private float nextSpawnAction = float.MaxValue;
    private PatternParser rotatePattern;
    private PatternParser spawnPattern;

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
        if (Time.time >= nextRotateAction)
        {
            bool skipRotate = false;
            string action = rotatePattern.GetNextAction();
            switch (action)
            {
                case "delay":
                    break;

                case "sync":
                    skipRotate = spawnPattern.GetCurrentStep() > 0;
                    break;

                default:
                    int degrees = int.Parse(action);
                    transform.Rotate(new Vector3(0, degrees, 0));
                    break;
            }

            if (!skipRotate)
            {
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

        if (Time.time >= nextSpawnAction)
        {
            string action = spawnPattern.GetNextAction();
            switch (action)
            {
                case "delay":
                    break;

                default:
                    Bullet obj = Instantiate<Bullet>(bullet, transform.position, transform.rotation);
					obj.IsFriendly = IsFriendlyFire;
					obj.Launch((transform.forward * speed) + relativeVelocity);
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

    }
}
