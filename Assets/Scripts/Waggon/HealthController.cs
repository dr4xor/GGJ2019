using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
	public delegate void HealthEvent();

	public HealthEvent OnHealthZero;

	private Waggon _waggon;

	[SerializeField]
	private int _maximumHealth = 1;

	[SerializeField]
	private int _currentHealth;
	public int CurrentHealth
	{
		get
		{
			return _currentHealth;
		}
		private set
		{
			if (value <= 0)
			{
				if(_currentHealth > 0)
				{
					_currentHealth = 0;
					OnHealthZero.Invoke();
				}
				return;
			}

			if (value > _maximumHealth)
			{
				_currentHealth = _maximumHealth;
				return;
			}

			_currentHealth = value;
		}
	}
	
	private void Awake()
	{
		_waggon = GetComponent<Waggon>();
		_currentHealth = _maximumHealth;
	}
	

	public void ApplyDamage(int damage)
	{
		//Debug.Log(gameObject.name + " damage: " + damage);
		CurrentHealth -= damage;


	}
	

}
