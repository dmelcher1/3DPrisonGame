using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamControl : MonoBehaviour {

    [SerializeField]
    private float offsetX;
    [SerializeField]
    private float offsetY;
    [SerializeField]
    private float camSmoothing;
    [SerializeField]
    private Transform follow;
    [SerializeField]
    private Vector3 addOffset = new Vector3(0.0f, 1.5f, 0.0f);

    private Vector3 camLook;
    private Vector3 targetPos;

    //smoothing and damping
    private Vector3 camSpeedSmoothing = Vector3.zero;
    [SerializeField]
    private float dampTime = 0.1f;

	// Use this for initialization
	void Start ()
    {
        follow = GameObject.FindWithTag("Player").transform;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LateUpdate()
    {
        Vector3 playerOffset = follow.position + addOffset;

        camLook = playerOffset - this.transform.position;
        camLook.y = 0;
        camLook.Normalize(); //Provides direction for our camera
        Debug.DrawRay(this.transform.position, camLook, Color.green);

        //New set cam position
        targetPos = playerOffset + follow.up * offsetY - camLook * offsetX;
        Debug.DrawRay(follow.position, Vector3.up * offsetY, Color.red);
        Debug.DrawRay(follow.position, -1.0f * follow.forward * offsetX, Color.blue);
        Debug.DrawLine(follow.position, targetPos, Color.green);

        //Updates cam pos to where we want it
        smoothPosition(this.transform.position, targetPos);
        //transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * camSmoothing);
        //targetPos = playerOffset + follow.up * offsetY - camLook * offsetX;

        transform.LookAt(follow);
    }

    private void smoothPosition(Vector3 start, Vector3 destination)
    {
        this.transform.position = Vector3.SmoothDamp(start, destination, ref camSpeedSmoothing, dampTime);
    }
}
