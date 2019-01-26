using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotiveDetector : MonoBehaviour
{
	private Locomotive _locomotive;

    // Start is called before the first frame update
    void Start()
    {
		_locomotive = GetComponent<Locomotive>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("CollectableWaggon"))
		{
			// Collect the Waggon
			_locomotive.AddWaggon(other.GetComponent<Waggon>());

		}
	}
}
