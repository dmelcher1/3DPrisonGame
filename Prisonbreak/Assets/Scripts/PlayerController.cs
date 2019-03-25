using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float dampDirectionTime = .25f;
    //[SerializeField]
    public Camera playerCam;
    [SerializeField]
    private float directionSpeed = 3.0f;

    private float speed = 0.0f;
    private float direction = 0.0f;
    private float horizontal = 0.0f;
    private float vertical = 0.0f;

    private float playerRot = 80.0f;
    private float playerRun = 5.0f;

    private float controlDeadZone = 0.5f; //NEEDS ADJUSTING TO ALLOW PIVOTING WITHOUT MOVING

    public Vector2 controlInput;

    //Hash ID
    private int n_LocomotionID = 0;


	// Use this for initialization
	void Start ()
    {
        
        animator = GetComponent<Animator>();

        //if (animator.layerCount >= 2) ;
        //{
        //    animator.SetLayerWeight(1, 1);
        //}
	}
	
	// Update is called once per frame
	void Update ()
    {
        controlInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (controlInput.magnitude < controlDeadZone)
            controlInput = Vector2.zero;
        else
            controlInput = controlInput.normalized * ((controlInput.magnitude - controlDeadZone) / (1 - controlDeadZone));

        horizontal = controlInput.x * playerRot * Time.deltaTime;
        vertical = controlInput.y * playerRun * Time.deltaTime;

        transform.Translate(0, 0, vertical);
        transform.Rotate(0, horizontal, 0);

        ControlToWorld(this.transform, playerCam.transform, ref direction, ref speed);

        //speed = new Vector2(horizontal, vertical).magnitude; //for animations (.sqrMagnitude is faster)

        //animator.SetFloat("Speed", speed);
        //animator.SetFloat("Direction", direction, dampDirectionTime, Time.deltaTime);

        
	}

    //private void FixedUpdate()
    //{
    //    if(IsInLocomotion() && ((direction >= 0 && horizontal >= 0) || (direction < 0 && horizontal < 0)))
    //    {
    //        Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, rotationDegreePerSecond * (horizontal < 0.0f ? -1f : 1f), 0.0f), Mathf.Abs)
    //        Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
    //        this.transform.rotation = this.transform.rotation * deltatRotation;

    //    }
    //}

    public void ControlToWorld(Transform playerTrans, Transform camera, ref float directionOut, ref float speedOut)
    {
        Vector3 playerTransDirection = playerTrans.forward;

        Vector3 stickDirection = new Vector3(horizontal, 0, vertical);

        speedOut = stickDirection.sqrMagnitude;

        Vector3 CameraDirection = camera.forward;
        CameraDirection.y = 0.0f;
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);

        Vector3 moveDirection = referentialShift * stickDirection;
        Vector3 axisSign = Vector3.Cross(moveDirection, playerTransDirection); 
        //Determines whether or not the vector between moveDirection and playerTransform's Direction is positive or negative
        //so the camera knows where to turn

        Debug.DrawRay(new Vector3(playerTrans.position.x, playerTrans.position.y + 2.0f, playerTrans.position.z), moveDirection, Color.green);
        //Cross-prodcut: If yellow is up (positive) turn left, if down (negative) turn right
        Debug.DrawRay(new Vector3(playerTrans.position.x, playerTrans.position.y + 2.0f, playerTrans.position.z), axisSign, Color.yellow);
        Debug.DrawRay(new Vector3(playerTrans.position.x, playerTrans.position.y + 2.0f, playerTrans.position.z), playerTransDirection, Color.red);
        Debug.DrawRay(new Vector3(playerTrans.position.x, playerTrans.position.y + 2.0f, playerTrans.position.z), stickDirection, Color.blue);

        float angleplayerTransToMove = Vector3.Angle(playerTransDirection, moveDirection) * (axisSign.y >= 0 ? -1.0f : 1.0f);

        angleplayerTransToMove /= 100.0f;

        directionOut = angleplayerTransToMove * directionSpeed;
    }

    //public bool IsInLocomotion()
    //{
    //    return stateInfo.nameHash == n_LocomotionID;
    //}
}
