﻿using System.Collections;
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
		if(!_waggon.IsConnected)
		{
			return;
		}

		if (other.CompareTag("CollectableWaggon"))
		{
			// Collect the Waggon
			OnCollectableWaggonHit(other.GetComponent<Waggon>());
		}
		else if (other.CompareTag("SolidBarrier"))
		{
			OnSolidBarrierHit(other.GetComponent<SolidBarrier>());
		}
		else if (other.CompareTag("Enemy"))
		{
			OnEnemyHit(other.GetComponent<Enemy>());
		}
	}


	protected virtual void OnCollectableWaggonHit(Waggon waggon)
	{

	}
	
	protected virtual void OnSolidBarrierHit(SolidBarrier barrier)
	{
		Debug.Log(gameObject.name + " Hit a harmfull object with " + barrier.Damage + " damage");
		_waggon.Health.ApplyDamage(barrier.Damage);
		barrier.OnCollisionEvent();
	}

	protected virtual void OnEnemyHit(Enemy hitEnemy)
	{
		_waggon.Health.ApplyDamage(hitEnemy.CollisionDamage);

		// Kill the enemy
		hitEnemy.Health.ApplyDamage(1000);
		
	}



}
