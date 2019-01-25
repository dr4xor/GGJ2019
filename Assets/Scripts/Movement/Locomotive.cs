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
	
	private List<Waggon> _waggonList = new List<Waggon>();
	
	private float _targetCarX = 0f;
	private float _carVelocityX;

	private Rigidbody _rigidbody;

	// Start is called before the first frame update
	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_waggonList.Add(this);
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
		Vector3 targetPos = new Vector3(_targetCarX, transform.position.y, transform.position.z + CarForwardSpeed * Time.deltaTime);

		float oldCarX = transform.position.x;

		transform.position = Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime * CarFollowSpeed);

		float newCarX = transform.position.x;
		
		_carVelocityX = (newCarX - oldCarX) * (1 / Time.fixedDeltaTime);
	}

	protected override void UpdateRotation()
	{
		transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, _carVelocityX * CarRotationMultiplier, transform.rotation.eulerAngles.z);
	}


	#region  WAGGON MANAGEMENT

	public void AddWaggon(GameObject waggonPrefab)
	{
		Waggon waggon = waggonPrefab.GetComponent<Waggon>();

		if (waggon == null)
		{
			Debug.LogError("Every Waggon Prefab needs to contain a Waggon Script! Waggon could not be added!");
			return;
		}


		Waggon lastWaggon = _waggonList[_waggonList.Count - 1];

		waggon.PreviousWaggon = lastWaggon;
		lastWaggon.NextWaggon = waggon;

		_waggonList.Add(waggon);
	}

	public void RemoveWaggon(int index)
	{
		if(_waggonList.Count <= index)
		{
			Debug.LogError("Invalid index (" + index + "). Train only has " + _waggonList.Count + " Waggons");
			return;
		}

		if(index < 1)
		{
			Debug.LogError("Invalid index. Can not remove locomotive");
			return;
		}


		_waggonList.RemoveRange(index, (_waggonList.Count - index));

		_waggonList[index - 1].NextWaggon = null;


	}

	#endregion
}
