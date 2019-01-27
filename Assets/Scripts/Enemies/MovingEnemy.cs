using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy
{
	[SerializeField] private bool relativeToLocomotive = true;

	[SerializeField] private Vector3 relativeMovingDirection;

	private Locomotive _locomotive;

    // Start is called before the first frame update
    protected override void AfterStart()
    {
		_locomotive = GameObject.FindGameObjectWithTag("Locomotive").GetComponent<Locomotive>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		Vector3 prevPosition = transform.position;

		transform.position = new Vector3(transform.position.x + (relativeMovingDirection.x * Time.deltaTime), transform.position.y + (relativeMovingDirection.y * Time.deltaTime), transform.position.z + (_locomotive.Velocity.z + relativeMovingDirection.z) * Time.deltaTime);
		
		foreach(BulletHellManager bhm in _bulletHellManagers)
		{
			bhm.relativeVelocity = (transform.position - prevPosition) * (1 / Time.deltaTime);
		}
	}
}
