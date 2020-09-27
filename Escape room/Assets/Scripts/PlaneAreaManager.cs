using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
public class PlaneAreaManager : MonoBehaviour
{
    ARPlaneManager m_ARPlaneManager;
    public GameObject objectToSpawn;

    private float CalculatePlaneArea(ARPlane plane)
    {
        return plane.size.x * plane.size.y;
    }

    public void CheckPlanes()
    {
        // loop over each plane that was detected
        foreach (var plane in m_ARPlaneManager.trackables)
        {
            // calclate area of each plane
            float area = CalculatePlaneArea(plane);
            // if (area > 0.5)
            // {
            // place object if area is greater than 0.5m^2
            Instantiate(objectToSpawn, plane.center, objectToSpawn.transform.rotation);
            // }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
        // Instantiate(objectToSpawn, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                if (Input.touchCount == 1)
                {
                    Ray raycast = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(raycast, out RaycastHit raycastHit))
                    {
                        var planeAreaBehaviour = raycastHit.collider.gameObject.GetComponent<PlaneAreaBehaviour>();
                        if (planeAreaBehaviour != null)
                        {
                            planeAreaBehaviour.ToggleAreaView();
                        }
                    }
                }
            }
        }
        CheckPlanes();
    }
}
