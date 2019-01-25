using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
	public float RoadDistance = 10f;
	public float RoadDiameter = 2f;

	public float CarFollowSpeed = 1f;
	public float CarRotationMultiplier = 1f;


	public GameObject debug_car;

	private float _targetCarX = 0f;
	private float _carVelocityX;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		// If the left mouse is pressed, update target pos
		if (Input.GetMouseButton(0))
		{
			float mouseX = GetRelativeMouseX();
			_targetCarX = mouseX * (RoadDiameter / 2);
		}



		// Update Position
		UpdateCarPosition();

		// Update Rotation
		UpdateCarRotation();
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

	/// <summary>
	/// Sets the position of the car
	/// </summary>
	/// <param name="carX">Value between -1 - 1</param>
	private void UpdateCarPosition()
	{
		Vector3 targetPos = new Vector3(_targetCarX, debug_car.transform.position.y, debug_car.transform.position.z);

		float oldCarX = debug_car.transform.position.x;

		debug_car.transform.position = Vector3.Lerp(debug_car.transform.position, targetPos, Time.deltaTime * CarFollowSpeed);

		float newCarX = debug_car.transform.position.x;


		_carVelocityX = (newCarX - oldCarX) * (1 / Time.deltaTime);
	}

	private void UpdateCarRotation()
	{

		debug_car.transform.eulerAngles = new Vector3(debug_car.transform.rotation.eulerAngles.x, _carVelocityX * CarRotationMultiplier, debug_car.transform.rotation.eulerAngles.z);
	}
}
