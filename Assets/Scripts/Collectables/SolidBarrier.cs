using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidBarrier : MonoBehaviour
{
	[SerializeField] private int _damage;
	public int Damage => _damage;

	[SerializeField] private GameObject _destroyEffect;
	

	// called by the waggon this object is colliding with
	public void OnCollisionEvent()
	{
		if(_destroyEffect != null)
		{
			GameObject effect = Instantiate(_destroyEffect);
			effect.transform.position = transform.position;
			effect.transform.rotation = transform.rotation;
			effect.transform.localScale = transform.localScale;

			Destroy(effect, 3f);
		}

		Destroy(gameObject);
	}
}
