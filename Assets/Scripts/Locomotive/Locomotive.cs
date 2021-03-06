﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotive : Waggon
{
    public float RoadDistance = 10f;

    public float CarFollowSpeed = 1f;
    public float CarRotationMultiplier = 1f;

    [SerializeField] private float _maxFollowSpeed = 40f;
    [SerializeField] private float _followInSeconds = 1f;

    public float LocomotiveSpeedMS = 1f;


    public List<Waggon> Waggons
    {
        get
        {
            List<Waggon> waggons = new List<Waggon>();
            Waggon currentWaggon = this;
            while (currentWaggon != null)
            {
                waggons.Add(currentWaggon);
                currentWaggon = currentWaggon.NextWaggon;
            }

            return waggons;
        }
    }

    private float _targetCarX = 0f;
    private float _velocityDeltaX;

    private float _velocityX;

    public override bool IsConnected => true;

    // Start is called before the first frame update
    protected override void AfterAwake()
    {
    }

    // Update is called once per frame
    protected override void BeforeFixedUpdate()
    {
        // If the left mouse is pressed, update target pos
        if (Input.GetMouseButton(0))
        {
            float mouseX = GetRelativeMouseX();
            _targetCarX = mouseX;
        }
    }


    private float GetRelativeMouseX()
    {
        // Get Mouse Position
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = RoadDistance;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);

        return worldPoint.x;
    }

    protected override void UpdatePosition()
    {
        Vector3 targetPos = new Vector3(_targetCarX, transform.position.y, transform.position.z + LocomotiveSpeedMS * Time.deltaTime);

        float oldCarX = transform.position.x;

        float targetX = Mathf.SmoothDamp(transform.position.x, targetPos.x, ref _velocityX, _followInSeconds, _maxFollowSpeed);
		
		float previousZ = transform.position.z;

		transform.position = new Vector3(targetX, targetPos.y, targetPos.z);
		
		velocityZ = (transform.position.z - previousZ) * (1 / Time.deltaTime);

		float newCarX = transform.position.x;

        float oldVelocity = velocityX;
        velocityX = (newCarX - oldCarX) * (1 / Time.fixedDeltaTime);

        _velocityDeltaX = (oldVelocity - velocityX);
    }

    protected override void UpdateRotation()
    {
        float yEuler = velocityX * CarRotationMultiplier;

        if (yEuler > 80f)
        {
            yEuler = 80f;
        }
        else if (yEuler < -80f)
        {
            yEuler = -80f;
        }

        transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, yEuler, GetTiltAngle());
    }

    public override void OnHealthZero()
    {
        Debug.Log("GAME OVER!");
        LocomotiveSpeedMS = 0;
    }

    #region  WAGGON MANAGEMENT

    public void AddWaggon(Waggon waggon)
    {
        if (waggon == null)
        {
            Debug.LogError("Every Waggon Prefab needs to contain a Waggon Script! Waggon could not be added!");
            return;
        }


        List<Waggon> waggons = Waggons;

        if (waggon.NextLevelWaggonPrefab != null)
        {
            foreach (Waggon w in waggons)
            {
                if (w.NextLevelWaggonPrefab == waggon.NextLevelWaggonPrefab)
                {
                    // COMBINE THEM
                    Waggon prevWaggon = w.PreviousWaggon;

                    Waggon combinedWaggon = Instantiate(waggon.NextLevelWaggonPrefab.gameObject).GetComponent<Waggon>();
                    prevWaggon.NextWaggon = combinedWaggon;
                    combinedWaggon.PreviousWaggon = prevWaggon;
                    combinedWaggon.NextWaggon = w.NextWaggon;

                    if (w.NextWaggon != null)
                    {
                        w.NextWaggon.PreviousWaggon = combinedWaggon;
                    }

                    Destroy(w.gameObject);
                    Destroy(waggon.gameObject);

                    combinedWaggon.OnConnectEvent();
                    return;
                }
            }
        }

        Waggon lastWaggon = waggons[waggons.Count - 1];

        waggon.PreviousWaggon = lastWaggon;
        lastWaggon.NextWaggon = waggon;

        waggon.OnConnectEvent();
    }

    public void RemoveWaggon(int index)
    {
        List<Waggon> waggons = Waggons;

        if (waggons.Count <= index)
        {
            Debug.LogError("Invalid index (" + index + "). Train only has " + waggons.Count + " Waggons");
            return;
        }

        if (index < 1)
        {
            Debug.LogError("Invalid index. Can not remove locomotive");
            return;
        }

        waggons[index - 1].NextWaggon = null;


    }

    #endregion
}
