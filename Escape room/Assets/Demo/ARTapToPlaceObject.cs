using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject placementIndicator;
    public GameObject stakeObject;
    public GameObject hoopObject;

    private ARSessionOrigin arOrigin;
    private ARRaycastManager arManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool isStakePlaced = false;
    private int hoopObjectPlacedCap = 3;
    private List<GameObject> hoopObjectsPlaced = new List<GameObject>();
    
    private Vector2 startPos;
    private Vector2 endPos;
    private float startTime;
    private float diffTime;
    private float distance;
    private float speed;

    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        arManager = FindObjectOfType<ARRaycastManager>();

    }

    void Update()
    {
        /*
         * Every frame, check the world around us with the camera to scan where the floor is to decide where to place the object
         */

        UpdatePlacementPose();
        UpdatePlacementIndicator();
        if (Input.touchCount > 0 && Input.GetTouch(0).phase > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
                startTime = Time.time;
                if (!isStakePlaced && placementPoseIsValid) {
                    PlaceStake();
                }
            } else if (touch.phase == TouchPhase.Ended) {
                endPos = touch.position;
                diffTime = Time.time - startTime;
                startTime = 0;
                distance = Vector2.Distance(startPos,endPos);
                if(diffTime != 0)
                {
                    speed = distance / diffTime;
                }
                if (speed != 0 && isStakePlaced && hoopObjectsPlaced.Count < hoopObjectPlacedCap)
                {
                    ThrowHoop(speed);
                }
                // logic to detect swipe movement and instantiate hoopObject in front of camera with velocity of throwing related to speed of swipe of the finger
                // increment the amount of objects placed
                // Once cap is reached, go to next if statement
            }
        }
    }

    private void PlaceStake()
    {
        Instantiate(stakeObject, placementPose.position, placementPose.rotation);
        isStakePlaced = true;
    }
    
    private void ThrowHoop(float speed)
    {
        GameObject hoop = Instantiate(hoopObject, Camera.current.transform.position, new Quaternion(0, 0, 0, 1));
        hoop.GetComponent<Rigidbody>().AddForce(new Vector3(0, speed/20*(distance/diffTime), 2));
        speed = 0;
        hoopObjectsPlaced.Add(hoop);
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        } else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
