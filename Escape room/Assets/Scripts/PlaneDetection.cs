using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class PlaneDetection : MonoBehaviour
{
    ARPlaneManager m_ARPlaneManager;

    public GameObject objectToSpawn1, objectToSpawn2, objectToSpawn3;
    public GameObject startupText, scanDoneText, balloonGameUI;

    private bool objectPlaced1, objectPlaced2, objectPlaced3;
    private Vector3 object1Loc, object2Loc, object3Loc;
    public bool startupDone;

    // Start is called before the first frame update
    void Start()
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
        objectPlaced1 = false;
        objectPlaced2 = false;
        objectPlaced3 = false;
        startupDone = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startupDone)
            StartCoroutine(placeGameObjects());
    }

    private IEnumerator placeGameObjects()
    {
        foreach (var plane in m_ARPlaneManager.trackables)
        {
            // calclate area of each plane
            float area = CalculatePlaneArea(plane);
            if (area > 1)  // place object if area is greater than 1m^2
            {
                var planeAreaBehaviour = plane.GetComponent<PlaneAreaBehaviour>();
                if (!objectPlaced1 && !planeAreaBehaviour.hasObjOn)
                {
                    Instantiate(objectToSpawn1, plane.center, objectToSpawn1.transform.rotation);
                    object1Loc = plane.center;
                    objectPlaced1 = true;
                    planeAreaBehaviour.hasObjOn = true;
                    yield return new WaitForSeconds(1);
                }
                if (objectPlaced1 && !objectPlaced2 && !planeAreaBehaviour.hasObjOn)
                {
                    if (Vector3.Distance(object1Loc, plane.center) >= 2)
                    {
                        Instantiate(objectToSpawn2, plane.center, objectToSpawn2.transform.rotation);
                        object2Loc = plane.center;
                        objectPlaced2 = true;
                        planeAreaBehaviour.hasObjOn = true;
                        yield return new WaitForSeconds(1);
                    }
                }
                // if (objectPlaced1 && objectPlaced2 && !objectPlaced3 && !planeAreaBehaviour.hasObjOn)
                // {
                //     if (Vector3.Distance(object1Loc, plane.center) >= 1 && Vector3.Distance(object2Loc, plane.center) >= 1)
                //     {
                //         Instantiate(objectToSpawn3, plane.center, objectToSpawn3.transform.rotation);
                //         object3Loc = plane.center;
                //         objectPlaced3 = true;
                //         planeAreaBehaviour.hasObjOn = true;
                //         yield return new WaitForSeconds(1);
                //     }
                // }
            }
        }
        if (objectPlaced1 && objectPlaced2 && !startupDone)
        {
            TurnOffPlaneDetection();
            startupDone = true;
            balloonGameUI.SetActive(true);  
            startupText.SetActive(false);
            scanDoneText.SetActive(true);
            yield return new WaitForSeconds(5);
            scanDoneText.SetActive(false);
        }
    }

    private float CalculatePlaneArea(ARPlane plane)
    {
        return plane.size.x * plane.size.y;
    }

    public void TogglePlaneDetection()
    {
        m_ARPlaneManager.enabled = !m_ARPlaneManager.enabled;
        if (m_ARPlaneManager.enabled)
        {
            SetAllPlanesActive(true);
        }
        else
        {
            SetAllPlanesActive(false);
        }
    }

    private void TurnOffPlaneDetection()
    {
        SetAllPlanesActive(false);
        m_ARPlaneManager.enabled = false;
    }

    void SetAllPlanesActive(bool value)
    {
        foreach (var plane in m_ARPlaneManager.trackables)
            plane.gameObject.SetActive(value);
    }
}
