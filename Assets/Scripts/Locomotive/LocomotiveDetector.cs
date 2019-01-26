using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotiveDetector : WaggonObjectDetector
{
	private Locomotive _locomotive
	{
		get
		{
			return (Locomotive)_waggon;
		}
		set
		{
			_waggon = value;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
		_locomotive = GetComponent<Locomotive>();
    }

	protected override void OnCollectableWaggonHit(Waggon waggon)
	{
		_locomotive.AddWaggon(waggon);
	}

}
