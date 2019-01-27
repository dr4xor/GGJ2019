using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyObjectDetector : MonoBehaviour
{
	private Enemy _enemy;
    // Start is called before the first frame update
    void Start()
    {
		_enemy = GetComponent<Enemy>();

		Assert.IsNotNull(_enemy, "No enemy component found");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
	}
}
