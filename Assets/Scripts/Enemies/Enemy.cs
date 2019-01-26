using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private HealthController _health;
	public HealthController Health => _health;
	
	[SerializeField]
	private BulletHellManager _bulletHellManager;

	[SerializeField] private GameObject _destroyEffect;

    // Start is called before the first frame update
    void Start()
    {
		_health = GetComponent<HealthController>();
		_health.OnHealthZero = OnHealthZero;

		_bulletHellManager.enabled = true;
		_bulletHellManager.IsFriendlyFire = false;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnHealthZero()
	{
		if(_destroyEffect != null)
		{
			Destroy(Instantiate(_destroyEffect, transform.position, transform.rotation), 3f);
		}

		_bulletHellManager.enabled = false;
		
		Destroy(gameObject);
	}
}
