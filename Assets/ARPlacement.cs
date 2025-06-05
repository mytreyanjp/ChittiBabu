using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARPlacement : MonoBehaviour 
{
    [SerializeField] GameObject rob; // Your robot model
    private ARRaycastManager arRaycastManager;
    private ARPlaneManager arPlaneManager;

    void Start() 
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();

        // Hide robot until placement
        rob.SetActive(false); 
    }

    void Update() 
    {
        if (Input.touchCount > 0 && !rob.activeSelf) 
        {
            Touch touch = Input.GetTouch(0);
            List<ARRaycastHit> hits = new List<ARRaycastHit>();

            if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon)) 
            {
                rob.transform.position = hits[0].pose.position;
                rob.SetActive(true);

                // ✅ Disable plane detection
                arPlaneManager.enabled = false;

                // ✅ Hide all existing planes
                foreach (ARPlane plane in arPlaneManager.trackables)
                {
                    plane.gameObject.SetActive(false);
                }
            }
        }
    }
}
