using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class PlaneAreaBehaviour : MonoBehaviour
{
    public TextMeshPro areaText;
    public ARPlane ARPlane;
    public bool hasObjOn = false;

    private void OnEnable()
    {
        ARPlane.boundaryChanged += ArPlaneBoundaryChanged;
    }

    private void OnDisable()
    {
        ARPlane.boundaryChanged -= ArPlaneBoundaryChanged;
    }

    private void ArPlaneBoundaryChanged(ARPlaneBoundaryChangedEventArgs obj)
    {
        areaText.text = CalculatePlaneArea(ARPlane).ToString();
    }
    private float CalculatePlaneArea(ARPlane plane)
    {
        return plane.size.x * plane.size.y;
    }

    public void ToggleAreaView()
    {
        if (areaText.enabled)
            areaText.enabled = false;
        else
            areaText.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        hasObjOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        areaText.transform.rotation = Quaternion.LookRotation(areaText.transform.position - Camera.main.transform.position);
    }
}
