using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectDetector : MonoBehaviour
{
	private Enemy _enemy;
    // Start is called before the first frame update
    void Start()
    {
		_enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("FriendlyBullet"))
		{
			// Deal damage
			_enemy.Health.ApplyDamage(other.GetComponent<Bullet>().damage);
		}
		else if (other.CompareTag("Waggon") || other.CompareTag("Locomotive"))
		{
			// Kill instantly
			_enemy.Health.ApplyDamage(1000);
		}
	}
}
