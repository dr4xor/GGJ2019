using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotive : Waggon
{
	public float RoadDistance = 10f;
	public float RoadDiameter = 2f;

	public float CarFollowSpeed = 1f;
	public float CarRotationMultiplier = 1f;

	public float LocomotiveSpeed = 1f;
	
	[SerializeField]
	private List<Waggon> _waggonList = new List<Waggon>();

	public List<Waggon> Waggons => _waggonList;
	
	private float _targetCarX = 0f;
	private float _velocityDeltaX;

	private Rigidbody _rigidbody;

	protected override bool IsConnected => true;

	// Start is called before the first frame update
	void Awake()
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
		Vector3 targetPos = new Vector3(_targetCarX, transform.position.y, transform.position.z + LocomotiveSpeed * Time.fixedDeltaTime);

		float oldCarX = transform.position.x;
		

		transform.position = Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime * CarFollowSpeed);

		float newCarX = transform.position.x;

		float oldVelocity = velocityX;
		velocityX = (newCarX - oldCarX) * (1 / Time.fixedDeltaTime);

		_velocityDeltaX = (oldVelocity - velocityX);
	}

	protected override void UpdateRotation()
	{
		float yEuler = velocityX * CarRotationMultiplier;

		transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, yEuler, GetTiltAngle());
	}

	public override void OnHealthZero()
	{
		Debug.Log("GAME OVER!");
		LocomotiveSpeed = 0;
	}

	#region  WAGGON MANAGEMENT

	public void AddWaggon(Waggon waggon)
	{
		if (waggon == null)
		{
			Debug.LogError("Every Waggon Prefab needs to contain a Waggon Script! Waggon could not be added!");
			return;
		}


		Waggon lastWaggon = _waggonList[_waggonList.Count - 1];

		waggon.PreviousWaggon = lastWaggon;
		lastWaggon.NextWaggon = waggon;

		_waggonList.Add(waggon);
		
		waggon.OnCollectedEvent();
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
