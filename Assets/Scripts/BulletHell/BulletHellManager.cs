using UnityEngine;

public class BulletHellManager : MonoBehaviour
{
	public Vector3 relativeVelocity;
	public Vector3 bulletSpawnOffset;
	public bool IsFriendlyFire;

    public Bullet bullet;
    public string rotationFrequency;
    public string spawnFrequency;
    public float speed;

    private float nextRotateAction = float.MaxValue;
    private float nextSpawnAction = float.MaxValue;
    private PatternParser rotatePattern;
    private PatternParser spawnPattern;

	[SerializeField] private AudioClip _soundFX;
	[SerializeField] private bool _onlyPlayOnFire;

	private const float AUDIO_SOURCE_VOLUME = 0.7f;

	private AudioSource _audioSource;

    private void UpdateSpawnPattern(string value)
    {
        spawnPattern = new PatternParser(spawnFrequency, true);
        if (spawnPattern.GetNextExecution() != null)
        {
            nextSpawnAction = Time.time + (float)spawnPattern.GetNextExecution() / 1000;
        }
    }

    private void UpdateRotatePattern(string value)
    {
        rotatePattern = new PatternParser(rotationFrequency, true);
        if (rotatePattern.GetNextExecution() != null)
        {
            nextRotateAction = Time.time + (float)rotatePattern.GetNextExecution() / 1000;
        }
    }

    void Awake()
    {
        UpdateSpawnPattern(spawnFrequency);
        UpdateRotatePattern(rotationFrequency);

		if(_soundFX != null)
		{
			_audioSource = gameObject.AddComponent<AudioSource>();
			_audioSource.volume = AUDIO_SOURCE_VOLUME;
			_audioSource.playOnAwake = false;
			_audioSource.clip = _soundFX;
			if(!_onlyPlayOnFire)
			{
				_audioSource.loop = true;
			}
		}

		if(!_onlyPlayOnFire && _audioSource != null)
		{
			_audioSource.Play();
		}
    }

	void OnDisable()
	{
		if(_audioSource != null)
		{
			_audioSource.Stop();
		}
	}

    void Update()
    {
        if (Time.time >= nextRotateAction)
        {
            bool skipRotate = false;
            string action = rotatePattern.GetNextAction();
            switch (action)
            {
                case "delay":
                    break;

                case "sync":
                    skipRotate = spawnPattern.GetCurrentStep() > 0;
                    break;

                default:
                    int degrees = int.Parse(action);
                    transform.Rotate(new Vector3(0, degrees, 0));
                    break;
            }

            if (!skipRotate)
            {
                if (rotatePattern.Next() != null)
                {
                    nextRotateAction = Time.time + (float)rotatePattern.GetNextExecution() / 1000;
                }
                else
                {
                    nextRotateAction = float.MaxValue;
                }
            }
        }

        if (Time.time >= nextSpawnAction)
        {
            string action = spawnPattern.GetNextAction();
            switch (action)
            {
                case "delay":
                    break;

                default:
					if(_onlyPlayOnFire && _audioSource != null)
					{
						_audioSource.pitch = Random.Range(0.9f, 1.1f);
						_audioSource.Play();
					}
                    Bullet obj = Instantiate<Bullet>(bullet, transform.position + transform.TransformVector(bulletSpawnOffset), transform.rotation);
					obj.IsFriendly = IsFriendlyFire;
					obj.Launch((transform.forward * speed) + relativeVelocity);
                    break;

            }

            if (spawnPattern.Next() != null)
            {
                nextSpawnAction = Time.time + (float)spawnPattern.GetNextExecution() / 1000;
            }
            else
            {
                nextSpawnAction = float.MaxValue;
            }
        }

    }
}
