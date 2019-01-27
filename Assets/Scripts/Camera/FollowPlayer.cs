using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Locomotive _locomotive;

    [SerializeField] private Vector3 _pivotOffset;
    [SerializeField] private float _defaultDistance;

    [SerializeField]
    private float _defaultDistanceY = 6f;

    [SerializeField]
    private float _defaultDistanceZ = 6f;

    private float cameraOffsetZ = 0f;

    [SerializeField]
    private float zoomOutDistancePerWaggonMeter = 1f;
    [SerializeField]
    private float zoomOutSpeed = 1f;

    private Vector3 _targetCamPivotOffset;
    private Vector3 _camPivotOffset;

    public Vector3 CameraPivot
    {
        get
        {
            Vector3 locoWithOffset = (_locomotive.transform.position + _pivotOffset);

            return new Vector3(0f, locoWithOffset.y, locoWithOffset.z);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 playerPos = _locomotive.transform.position;

        float totalWaggonDistance = 0f;

        foreach (Waggon waggon in _locomotive.Waggons)
        {
            totalWaggonDistance += waggon.WaggonLength;
        }

        float zoomOutDistance = _defaultDistance + totalWaggonDistance * zoomOutDistancePerWaggonMeter;

        _targetCamPivotOffset = (-transform.forward) * zoomOutDistance;

        _camPivotOffset = Vector3.Lerp(_camPivotOffset, _targetCamPivotOffset, Time.deltaTime * zoomOutSpeed);

        transform.position = new Vector3(0f, (CameraPivot + _camPivotOffset).y, (CameraPivot + _camPivotOffset).z);


        _locomotive.RoadDistance = zoomOutDistance + _pivotOffset.y;

    }
}
