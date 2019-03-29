using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamControl : MonoBehaviour {

    
    public float offsetX;
    public float offsetY;
    [SerializeField]
    private float camSmoothing;
    [SerializeField]
    private Transform follow;
    [SerializeField]
    private Vector3 addOffset = new Vector3(0.0f, 1.5f, 0.0f);
    //[SerializeField]
    //private float widescreen = 0.2f;
    //[SerializeField]
    //private float targetingTime = 0.5f;

    public GameObject playerPrisoner;
    //public bool insideDoorArea = false;

    private Vector3 camLook;
    private Vector3 targetPos;
    private CameraPhase camPhase = CameraPhase.Behind;

    //smoothing and damping
    private Vector3 camSpeedSmoothing = Vector3.zero;
    [SerializeField]
    private float dampTime = 0.1f;

    public enum CameraPhase
    {
        Behind,
        Target
    }

	// Use this for initialization
	void Start ()
    {
        follow = GameObject.FindWithTag("Player").transform;	
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag == "Doorway")
    //    {
    //        Physics.IgnoreCollision(collision.collider, this.collider);
    //    }
    //}

    void LateUpdate()
    {
        Vector3 playerOffset = follow.position + addOffset;

        //Behind or Target Camera state
        if (Input.GetAxis("ResetCam") > 0.01f)
        {
            camPhase = CameraPhase.Target;
        }
        else
        {
            camPhase = CameraPhase.Behind;
        }


        switch (camPhase)
        {
            case CameraPhase.Behind:
                camLook = playerOffset - this.transform.position;
                camLook.y = 0;
                camLook.Normalize(); //Provides direction for our camera
                Debug.DrawRay(this.transform.position, camLook, Color.green);

                //New set cam position
                targetPos = playerOffset + follow.up * offsetY - camLook * offsetX;
                Debug.DrawRay(follow.position, Vector3.up * offsetY, Color.red);
                Debug.DrawRay(follow.position, -1.0f * follow.forward * offsetX, Color.blue);
                Debug.DrawLine(follow.position, targetPos, Color.green);
                break;

            case CameraPhase.Target:
                camLook = follow.forward;
                break;
        }
        targetPos = playerOffset + follow.up * offsetY - camLook * offsetX;

        //Detects camera collision w/wall and compensates instead of clipping
        DodgeObjects(playerOffset, ref targetPos);
        //Updates cam pos to where we want it
        smoothPosition(this.transform.position, targetPos);
        //transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * camSmoothing);
        //targetPos = playerOffset + follow.up * offsetY - camLook * offsetX;

        transform.LookAt(follow.position + new Vector3(0, 0.5f, 0));
    }

    private void smoothPosition(Vector3 start, Vector3 destination)
    {
        this.transform.position = Vector3.SmoothDamp(start, destination, ref camSpeedSmoothing, dampTime);
    }

    private void DodgeObjects(Vector3 fromCollision, ref Vector3 toPlayer)
    {
        Debug.DrawLine(fromCollision, toPlayer, Color.cyan);
        //Camera hits object compensation
        RaycastHit objDetect = new RaycastHit();
        if(Physics.Linecast(fromCollision, toPlayer, out objDetect))
        {
            Debug.DrawRay(objDetect.point, Vector3.left, Color.red);
            toPlayer = new Vector3(objDetect.point.x, toPlayer.y, objDetect.point.z);
        }
    }
}
