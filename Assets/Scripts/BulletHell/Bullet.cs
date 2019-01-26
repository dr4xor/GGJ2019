using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int lifeTime = 1;

    private Rigidbody rigidBody;
    private float timeToDie;

    void Awake()
    {
        timeToDie = Time.time + lifeTime;
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
        {
            rigidBody = gameObject.AddComponent<Rigidbody>();
        }
        rigidBody.useGravity = false;
    }

    void Update()
    {
        if (Time.time > timeToDie)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(float speed)
    {
        rigidBody.AddRelativeForce(Vector3.forward * speed);
    }
}
