using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Waggon))]
public class WaggonHealth : MonoBehaviour
{
	private Waggon _waggon;

	[SerializeField]
	private int _maximumHealth = 1;

	private int _currentHealth;
	public int CurrentHealth
	{
		get
		{
			return _currentHealth;
		}
		private set
		{
			if (_currentHealth + value < _maximumHealth)
			{
				if(_currentHealth > 0)
				{
					_currentHealth = 0;
					_waggon.OnHealthZero();
				}
				return;
			}

			if (_currentHealth + value > _maximumHealth)
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
		CurrentHealth -= damage;
	}
}
