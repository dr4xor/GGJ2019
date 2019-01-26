using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaggonObjectDetector : MonoBehaviour
{
	protected Waggon _waggon;

    // Start is called before the first frame update
    void Start()
    {
		_waggon = GetComponent<Waggon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("CollectableWaggon"))
		{
			// Collect the Waggon
			OnCollectableWaggonHit(other.GetComponent<Waggon>());
		}
		else if (other.CompareTag("HarmfullObject"))
		{
			OnHarmfullObjectHit(other.GetComponent<HarmfullObject>());
		}
	}


	protected virtual void OnCollectableWaggonHit(Waggon waggon)
	{
	}
	
	protected virtual void OnHarmfullObjectHit(HarmfullObject harmfullObject)
	{
		Debug.Log(gameObject.name + " Hit a harmfull object with " + harmfullObject.Damage + " damage");
		_waggon.Health.ApplyDamage(harmfullObject.Damage);
		harmfullObject.OnCollisionEvent();
	}



}
