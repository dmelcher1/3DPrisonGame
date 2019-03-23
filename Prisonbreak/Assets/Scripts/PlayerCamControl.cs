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
    private Vector3 targetPos;

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
        //Sets cam post
        targetPos = follow.position + follow.up * offsetY - follow.forward * offsetX; 
        Debug.DrawRay(follow.position, Vector3.up * offsetY, Color.red);
        Debug.DrawRay(follow.position, -1.0f * follow.forward * offsetX, Color.blue);
        Debug.DrawLine(follow.position, targetPos, Color.green);

        //Updates cam pos to where we want it
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * camSmoothing); 

        transform.LookAt(follow);
    }
}
