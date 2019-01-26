using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	[SerializeField] private Locomotive _locomotive;
	

	[SerializeField]
	private float _defaultDistanceY = 6f;

	[SerializeField]
	private float _defaultDistanceZ = 6f;

	private float cameraOffsetZ = 0f;
	// Start is called before the first frame update
	void Start()
    {

	}

    // Update is called once per frame
    void LateUpdate()
    {
		Vector3 playerPos = _locomotive.transform.position;

		float waggonDistanceExtra = 0f;
		
		foreach(Waggon waggon in _locomotive.Waggons)
		{
			waggonDistanceExtra += waggon.WaggonLength;
		}

		float cameraHeight = Mathf.Lerp(transform.position.y, _defaultDistanceY + waggonDistanceExtra, Time.deltaTime);
		cameraOffsetZ = Mathf.Lerp(cameraOffsetZ, _defaultDistanceZ + waggonDistanceExtra, Time.deltaTime);

		transform.position = new Vector3(transform.position.x, cameraHeight, playerPos.z - cameraOffsetZ);
		
		
    }
}
