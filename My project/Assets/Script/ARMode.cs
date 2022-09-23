using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
public class ARMode : MonoBehaviour
{
    public GameObject field;
    private ARRaycastManager _arRaycastManager;
    private ARPlaneManager _arPlaneManager;
    public GameObject arCamera,MainCamera;
    public bool AROn,clicked;
    Vector3 fieldPos;
    
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    // Start is called before the first frame update
    void Awake()
    {
        fieldPos= field.transform.position;
        _arRaycastManager=GetComponent<ARRaycastManager>();
        _arPlaneManager=GetComponent<ARPlaneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(AROn&&clicked)
        {
            _arPlaneManager.planesChanged+= PlaneUpdated;
            arCamera.SetActive(true);
            MainCamera.SetActive(false);
            clicked=false;
        }
        if(!AROn&&!clicked)
        {
            arCamera.SetActive(false);
            MainCamera.SetActive(true);
            field.transform.position=fieldPos;
        }
    }
    private void PlaneUpdated(ARPlanesChangedEventArgs args)
    {
        if(args.added != null)
        {
            ARPlane arPlane = args.added[0];
            field.transform.position= arPlane.transform.position;
        }
    }
}
