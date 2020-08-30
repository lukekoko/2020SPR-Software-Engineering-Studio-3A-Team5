using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObject : MonoBehaviour
{
    public GameObject objectToSpawn;
    private Indicator indicator;
    private ARRaycastManager rayManager;

    // Start is called before the first frame update
    void Start()
    {
        indicator = FindObjectOfType<Indicator>();
        rayManager = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        // {
        //     GameObject obj = Instantiate(objectToSpawn, indicator.transform.position, indicator.transform.rotation);
        // }
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (rayManager.Raycast(touch.position, hits, TrackableType.PlaneWithinBounds))
                {
                    GameObject obj = Instantiate(objectToSpawn, hits[0].pose.position, hits[0].pose.rotation);
                }
            }
        }

    }
}
