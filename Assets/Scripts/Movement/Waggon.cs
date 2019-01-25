using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waggon : MonoBehaviour
{
	public Waggon PreviousWaggon;
	public Waggon NextWaggon;

	[SerializeField] private float _waggonLength;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		BeforeFixedUpdate();


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

		transform.position = new Vector3(Mathf.Lerp(transform.position.x, targetX, Time.fixedDeltaTime * _followSpeed), prevWaggonPos.y, prevWaggonPos.z - (PreviousWaggon._waggonLength / 2f) - (_waggonLength / 2f));
		
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

		transform.eulerAngles = new Vector3(transform.eulerAngles.x, yAngle, transform.eulerAngles.z);

	}
}
