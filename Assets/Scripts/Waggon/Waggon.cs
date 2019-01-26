﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Waggon : MonoBehaviour
{
	[SerializeField]
	private Renderer _renderer;
	public Renderer Renderer;

	public Waggon PreviousWaggon;
	public Waggon NextWaggon;

	private WaggonHealth _waggonHealth;
	public WaggonHealth Health => _waggonHealth;

	[SerializeField] private float _waggonLength;
	public float WaggonLength => _waggonLength;

	[SerializeField] private float _waggonWidth;
	public float WaggonWidth => _waggonWidth;

	[SerializeField] protected float _waggonTiltFactor;
	protected const float MAX_Z_TILT = 30f;

	protected float velocityX = 0f;

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

	protected virtual bool IsConnected => PreviousWaggon != null; 

	void Awake()
	{
		_waggonHealth = GetComponent<WaggonHealth>();
		Assert.IsNotNull(_waggonHealth, "Sure you want a waggon without health?");

		AfterAwake();
	}

	protected virtual void AfterAwake()
	{
	}

	// Start is called before the first frame update
	void Start()
    {
        if (PreviousWaggon == null)
		{
			// The Object is not connected to the train => It's collectable
			gameObject.tag = "CollectableWaggon";
		}
    }

	public void OnCollectedEvent()
	{
		gameObject.tag = "Waggon";
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
	}

	protected virtual void BeforeFixedUpdate()
	{
	}


	protected virtual void UpdatePosition()
	{
		Vector3 prevWaggonPos = PreviousWaggon.transform.position;

		float targetX = prevWaggonPos.x;

		float previousX = transform.position.x;
		velocityX = (transform.position.x - previousX) * (1 / Time.fixedDeltaTime);
		
		// Calculate offset from the ground
		float sin = Mathf.Sin(GetTiltAngle() * Mathf.Deg2Rad);
		float tiltHeight = sin * (WaggonWidth / 2f);


		transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetX, Time.fixedDeltaTime * _followSpeed), tiltHeight, prevWaggonPos.z - (PreviousWaggon._waggonLength / 2f) - (_waggonLength / 2f));
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
	}

}