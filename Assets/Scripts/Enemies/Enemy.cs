using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private HealthController _health;
	public HealthController Health => _health;
	
	protected BulletHellManager[] _bulletHellManagers;

	[SerializeField] private GameObject _destroyEffect;

    // Start is called before the first frame update
    void Start()
    {
		_health = GetComponent<HealthController>();
		_health.OnHealthZero = OnHealthZero;

		_bulletHellManagers = GetComponentsInChildren<BulletHellManager>(true);

		foreach(BulletHellManager bhm in _bulletHellManagers)
		{
			bhm.enabled = true;
			bhm.IsFriendlyFire = false;
		}
		AfterStart();
	}

	protected virtual void AfterStart()
	{
	}
	

	private void OnHealthZero()
	{
		if(_destroyEffect != null)
		{
			Destroy(Instantiate(_destroyEffect, transform.position, transform.rotation), 3f);
		}

		foreach (BulletHellManager bhm in _bulletHellManagers)
		{
			bhm.enabled = false;
		}
		
		Destroy(gameObject);
	}
}
