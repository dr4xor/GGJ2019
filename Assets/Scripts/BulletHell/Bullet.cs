using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int lifeTime = 1;
    public int strength = 1;
    public GameObject hitAnimation;
	public int damage = 1;

	public bool IsFriendly = true;

    private Rigidbody rigidBody;
    private float timeToDie;

    void Awake()
    {
        timeToDie = Time.time + lifeTime;
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
        {
            rigidBody = gameObject.AddComponent<Rigidbody>();
        }
        rigidBody.useGravity = false;
    }

    void Update()
    {
        if (Time.time > timeToDie)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitAnimation != null)
        {
            Destroy(Instantiate(hitAnimation, transform.position, transform.rotation), 1f);
        }
        Destroy(gameObject);
    }

	private void OnTriggerEnter(Collider other)
	{
		if (IsFriendly)
		{
			if (other.CompareTag("Enemy"))
			{
				other.GetComponent<Enemy>().Health.ApplyDamage(damage);
				DestroyEvent();
			}
			else if(other.CompareTag("SolidBarrier"))
			{
				DestroyEvent();
			}
		}

	}

	private void DestroyEvent()
	{
		if (hitAnimation != null)
		{
			Destroy(Instantiate(hitAnimation, transform.position, transform.rotation), 1f);
		}
		Destroy(gameObject);
	}

    public void Launch(Vector3 force)
    {
        rigidBody.AddForce(force, ForceMode.VelocityChange);

    }
}
