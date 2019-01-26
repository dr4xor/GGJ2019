﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_SceneScroller : MonoBehaviour
{
	public float ScrollSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (Time.deltaTime * ScrollSpeed));
    }
}
