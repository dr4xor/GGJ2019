using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Waggon : MonoBehaviour
{
	[SerializeField]
	private Waggon _nextLevelWaggonPrefab;
	public Waggon NextLevelWaggonPrefab => _nextLevelWaggonPrefab;
	
	public Waggon PreviousWaggon;
	public Waggon NextWaggon;
	
	private BulletHellManager[] _bulletHellManagers;

	[SerializeField]
	private GameObject _onDestroyEffect;

	private HealthController _waggonHealth;
	public HealthController Health => _waggonHealth;

	[SerializeField] private float _waggonLength;
	public float WaggonLength => _waggonLength;

	[SerializeField] private float _waggonWidth;
	public float WaggonWidth => _waggonWidth;

	[SerializeField] protected float _waggonTiltFactor;
	protected const float MAX_Z_TILT = 20f;

	protected float velocityX = 0f;
	protected float velocityZ = 0f;
	public Vector3 Velocity => new Vector3(velocityX, 0f, velocityZ);
	
	public Vector3 BackConnectionPoint
	{
		get
		{
			return transform.position + (transform.forward * (_waggonLength / 2f));
		}
	}

	public Vector3 FrontConnectionPoint
	{
		get
		{
			return transform.position + (transform.forward * -(_waggonLength / 2f));
		}
	}
	

	[SerializeField] private float _followSpeed;

	public virtual bool IsConnected => PreviousWaggon != null; 

	void Awake()
	{
		_waggonHealth = GetComponent<HealthController>();
		_waggonHealth.OnHealthZero = OnHealthZero;

		Assert.IsNotNull(_waggonHealth, "Sure you want a waggon without health?");


		_bulletHellManagers = GetComponentsInChildren<BulletHellManager>();
		foreach (BulletHellManager bhm in _bulletHellManagers)
		{
			bhm.IsFriendlyFire = true;
		}

		AfterAwake();
	}

	protected virtual void AfterAwake()
	{
	}

	// Start is called before the first frame update
	void Start()
    {
		if (!IsConnected)
		{
			gameObject.tag = "CollectableWaggon";
			OnDisconnectEvent();
		}
    }

	private LineRenderer _connectionLine;

	public void OnConnectEvent()
	{
		gameObject.tag = "Waggon";
		transform.parent = null;
		
		foreach(BulletHellManager bhm in _bulletHellManagers)
		{
			bhm.enabled = true;
		}

		// Spawn Connection Line
		_connectionLine = (Instantiate(Resources.Load("WaggonConnectionLine", typeof(GameObject)) as GameObject)).GetComponent<LineRenderer>();

		GetComponent<Animator>().speed = 1f;
	}
	public void OnDisconnectEvent(bool explode = false)
	{
		// The Object is not connected to the train => It's collectable
		if(explode)
		{
			Destroy(Instantiate(_onDestroyEffect, transform.position, transform.rotation), 5f);
		}

		foreach (BulletHellManager bhm in _bulletHellManagers)
		{
			bhm.enabled = false;
		}

		if(_connectionLine != null)
		{
			Destroy(_connectionLine.gameObject);
			_connectionLine = null;
		}
		
		GetComponent<Animator>().speed = 0f;
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		BeforeFixedUpdate();
		
		if(!IsConnected)
		{
			return;
		}

		UpdatePosition();

		UpdateRotation();

		UpdateConnectionLine();
	}

	protected virtual void BeforeFixedUpdate()
	{
	}


	protected virtual void UpdatePosition()
	{
		Vector3 prevWaggonPos = PreviousWaggon.transform.position;

		float targetX = prevWaggonPos.x;

		float previousX = transform.position.x;
		float previousZ = transform.position.z;

		// Calculate offset from the ground
		float sin = Mathf.Sin(GetTiltAngle() * Mathf.Deg2Rad);
		float tiltHeight = Mathf.Abs(sin * (WaggonWidth / 2f));


		transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetX, Time.fixedDeltaTime * _followSpeed), tiltHeight, prevWaggonPos.z - (PreviousWaggon._waggonLength / 2f) - (_waggonLength / 2f));
		
		velocityX = (transform.position.x - previousX) * (1 / Time.fixedDeltaTime);
		velocityZ = (transform.position.z - previousZ) * (1 / Time.fixedDeltaTime);

		foreach (BulletHellManager bhm in _bulletHellManagers)
		{
			bhm.relativeVelocity = Velocity;
		}
	}

	protected virtual void UpdateRotation()
	{
		Vector3 prevWaggonConnectionPoint = PreviousWaggon.BackConnectionPoint;

		Vector3 nextWaggonConnectionPoint = transform.position;

		if(NextWaggon != null)
		{
			nextWaggonConnectionPoint = NextWaggon.FrontConnectionPoint;
		}


		Vector3 connectionVector = (prevWaggonConnectionPoint - nextWaggonConnectionPoint).normalized;

		

		float yAngle = Vector3.Angle(Vector3.forward, connectionVector);

		if(connectionVector.x < 0)
		{
			yAngle = -yAngle;
		}

		transform.eulerAngles = new Vector3(transform.eulerAngles.x, yAngle, GetTiltAngle());
	}

	protected virtual void UpdateConnectionLine()
	{
		if(_connectionLine == null)
		{
			return;
		}

		_connectionLine.transform.position = FrontConnectionPoint;
		
		_connectionLine.SetPositions(new Vector3[]{
			BackConnectionPoint + transform.up * 0.9f,
			PreviousWaggon.BackConnectionPoint + PreviousWaggon.transform.up * 0.9f,
		});
	}

	protected float GetTiltAngle()
	{
		float zEuler = -velocityX * _waggonTiltFactor;

		if (Mathf.Abs(zEuler) > MAX_Z_TILT)
		{
			if (zEuler > 0)
			{
				zEuler = MAX_Z_TILT;
			}
			else
			{
				zEuler = -MAX_Z_TILT;
			}
		}

		return zEuler;
	}


	public virtual void OnHealthZero()
	{
		PreviousWaggon.NextWaggon = null;
		PreviousWaggon = null;

		Waggon currentWaggon = this;
		while(currentWaggon != null)
		{
			currentWaggon.OnDisconnectEvent(true);
			currentWaggon = currentWaggon.NextWaggon;
		}
	}

}
