using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableWaggon : MonoBehaviour
{
	[SerializeField] private GameObject _waggonPrefab;

    // Start is called before the first frame update
    void Awake()
    {
		gameObject.tag = "CollectableWaggon";
		gameObject.GetComponent<BoxCollider>().isTrigger = false;
    }

	public void OnCollectedEvent()
	{
		// The Waggon was collected
		//TODO: Play Particle Effect
		Destroy(gameObject);
	}
}
