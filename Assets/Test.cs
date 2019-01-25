using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	[SerializeField]
	private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
		_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
		_rigidbody.AddForce(new Vector3(3,3,3), ForceMode.Impulse);
		

		
    }
}
