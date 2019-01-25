using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
	[SerializeField] private GameObject _player;

	[SerializeField] private float _playerOffsetZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
		Vector3 playerPos = _player.transform.position;
		transform.position = new Vector3(transform.position.x, transform.position.y, playerPos.z + _playerOffsetZ);
    }
}
