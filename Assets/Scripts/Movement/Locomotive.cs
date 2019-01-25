using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotive : Waggon
{
	public float RoadDistance = 10f;
	public float RoadDiameter = 2f;

	public float CarFollowSpeed = 1f;
	public float CarRotationMultiplier = 1f;

	public float CarForwardSpeed = 1f;


	public GameObject debug_car;

	private float _targetCarX = 0f;
	private float _carVelocityX;

	private Rigidbody _rigidbody;

	// Start is called before the first frame update
	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	protected override void BeforeFixedUpdate()
	{
		// If the left mouse is pressed, update target pos
		if (Input.GetMouseButton(0))
		{
			float mouseX = GetRelativeMouseX();
			_targetCarX = mouseX * (RoadDiameter / 2);
		}

	}


	private float GetRelativeMouseX()
	{
		// Get Mouse Position
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = RoadDistance;
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);

		float mouseX = worldPoint.x;

		float roadRadius = RoadDiameter / 2f;

		if (mouseX > roadRadius)
		{
			mouseX = roadRadius;
		}
		else if (mouseX < -roadRadius)
		{
			mouseX = -roadRadius;
		}

		float relativeMouseX = mouseX / roadRadius;

		return relativeMouseX;
	}
	
	protected override void UpdatePosition()
	{
		Vector3 targetPos = new Vector3(_targetCarX, debug_car.transform.position.y, debug_car.transform.position.z + CarForwardSpeed * Time.deltaTime);

		float oldCarX = debug_car.transform.position.x;

		debug_car.transform.position = Vector3.Lerp(debug_car.transform.position, targetPos, Time.fixedDeltaTime * CarFollowSpeed);

		float newCarX = debug_car.transform.position.x;
		
		_carVelocityX = (newCarX - oldCarX) * (1 / Time.fixedDeltaTime);
	}

	protected override void UpdateRotation()
	{
		debug_car.transform.eulerAngles = new Vector3(debug_car.transform.rotation.eulerAngles.x, _carVelocityX * CarRotationMultiplier, debug_car.transform.rotation.eulerAngles.z);
	}
}
