using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellManager : MonoBehaviour
{
    public Bullet bullet;
    public string frequencyPattern;

    private float nextAction = 0;
    private PatternParser pattern;

    void Start()
    {
        pattern = new PatternParser(frequencyPattern);
        nextAction = Time.time + (float)pattern.GetNextExecution() / 1000;
    }

    void Update()
    {
        if (Time.time >= nextAction)
        {
            Instantiate<Bullet>(bullet);

            if (pattern.Next() != null)
            {
                nextAction = Time.time + (float)pattern.GetNextExecution() / 1000;
            } else
            {
                nextAction = float.MaxValue;
            }
        }
    }
}
