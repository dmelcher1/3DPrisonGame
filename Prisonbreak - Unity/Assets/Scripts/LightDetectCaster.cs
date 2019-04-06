using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetectCaster : MonoBehaviour {

    public Transform colDetector;
    private Vector3 targetPos;
    private Vector3 lightPos;

	// Use this for initialization
	void Start ()
    {
        lightPos = transform.position;
        //colDetector.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        DetectPlayer(lightPos, ref targetPos);
	}

    private void DetectPlayer(Vector3 thisLocation, ref Vector3 floorDetect)
    {
        Debug.DrawLine(thisLocation, floorDetect, Color.red);
        RaycastHit lightSurface = new RaycastHit();
        if(Physics.Linecast(thisLocation, transform.TransformDirection(transform.forward), out lightSurface))
        {
            //Debug.DrawRay(lightSurface.point, Vector3.forward, Color.red);
            floorDetect = new Vector3(lightSurface.point.x, lightSurface.point.y, lightSurface.point.z);
            colDetector.position = floorDetect;
        }
    }
}
