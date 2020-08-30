using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SpawnObject : MonoBehaviour
{
    public GameObject objectToSpawn;
    private ARRaycastManager rayManager;

    private bool objectSpawned = false; 

    // Start is called before the first frame update
    void Start()
    {
        rayManager = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinBounds);

        if (hits.Count > 0 && !objectSpawned)
        {
            GameObject obj = Instantiate(objectToSpawn, hits[0].pose.position, hits[0].pose.rotation);
            objectSpawned = true;
        }

    }
}
