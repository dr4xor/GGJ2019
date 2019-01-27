using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public delegate void HealthEvent();

    public HealthEvent OnHealthZero;

    private Waggon _waggon;

    [SerializeField]
    private int _maximumHealth = 1;

	[SerializeField] private Slider _slider;

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
                if (_currentHealth > 0)
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

	private void Update()
	{
		if(_slider != null)
		{
			_slider.value = ((float)_currentHealth) / (float)_maximumHealth;
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
