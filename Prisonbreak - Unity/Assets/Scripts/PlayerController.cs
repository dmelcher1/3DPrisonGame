using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private Animator animator;
    [SerializeField]
    //private float dampDirectionTime = .25f;
    //[SerializeField]
    public Camera playerCam;
    [SerializeField]
    private float directionSpeed = 3.0f;
    [SerializeField]
    private float rotDeg = 120f;
    [SerializeField]
    private float electricTimer = 0.0f;
    public GameObject spotLight;

    public Slider healthSlider;
    public Text keyText;

    public float jumpTimer;
    public bool zapped = false;
    public int health;
    public Transform playerStart;
    private float speed = 0.0f;
    private float direction = 0.0f;
    public float horizontal = 0.0f;
    public float vertical = 0.0f;
    public float spottedTimer = 10.0f;
    public float coolDownTimer;
    public bool spotted = false;
    public bool caught = false;
    public bool insideDoorArea = false;
    public bool airborne = false;

    public float fadeDelay = 10.0f;
    public bool dead = false;
    private float playerRot = 80.0f;
    private float playerRun = 4.0f;
    public float playerJump;
    private float controlDeadZone = 0.5f; //NEEDS ADJUSTING TO ALLOW PIVOTING WITHOUT MOVING
    private AnimatorStateInfo stateInfo;
    public GameObject mainCamera;
    PlayerCamControl playerCamControl;
    Rigidbody rb;
    public int keysHeld = 0;

    public Vector2 controlInput;

    //Hash ID
    private int Run_and_SprintID = 0;


    // Use this for initialization
    void Start()
    {
        this.transform.position = playerStart.position;
        this.transform.rotation = playerStart.rotation;

        health = 3;

        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        playerCamControl = mainCamera.GetComponent<PlayerCamControl>();

        //if (animator.layerCount >= 2)
        //{
        //    animator.SetLayerWeight(1, 1);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(rb.velocity);
        if (insideDoorArea == true)
        {
            playerCamControl.offsetX = 2.0f;
            playerCamControl.offsetY = -0.5f;

            //x = 3, y = 0
        }
        else
        {
            playerCamControl.offsetX = 5.0f;
            playerCamControl.offsetY = 2.0f;
        }

        if (animator && zapped == false && caught == false)
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            controlInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (controlInput.magnitude < controlDeadZone)
                controlInput = Vector2.zero;
            else
                controlInput = controlInput.normalized * ((controlInput.magnitude - controlDeadZone) / (1 - controlDeadZone));

            horizontal = controlInput.x * playerRot * Time.deltaTime;
            vertical = controlInput.y * playerRun * Time.deltaTime;

            if (airborne == false && Input.GetButton("Jump") && jumpTimer <= 0)
            {
                //Debug.Log("Jump");
                jumpTimer = 3.0f;
                rb.AddForce(Vector3.up * playerJump);
                airborne = true;
            }
            
            transform.Translate(0, 0, vertical);
            if(horizontal == 0)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX 
                    | RigidbodyConstraints.FreezeRotationZ;
            }
            else
            {
                transform.Rotate(0, horizontal, 0);
                rb.constraints = RigidbodyConstraints.FreezeRotationX
                    | RigidbodyConstraints.FreezeRotationZ;
            }
           

            ControlToWorld(this.transform, playerCam.transform, ref direction, ref speed);

            speed = new Vector2(horizontal, vertical).magnitude; //for animations (.sqrMagnitude is faster)

            //Debug.Log(speed);

            animator.SetFloat("Speed", speed);
            //animator.SetFloat("Direction", direction, dampDirectionTime, Time.deltaTime);
        }

        if(electricTimer <= 10.0f)
        {
            zapped = false;
            animator.SetBool("Zapped", false);
        }

        if (health <= 0 || caught == true)
        {
            dead = true;
            fadeDelay -= 0.1f;
        }
        //   this.transform.position = playerStart.position;
        //   this.transform.rotation = playerStart.rotation;
        //   health = 3;

        
        electricTimer -= 0.1f;
        jumpTimer -= 0.1f;
        if(electricTimer > 0 && dead == true)
        {
            zapped = true;
            animator.SetBool("Zapped", true);
        }
        if (spotted == true && coolDownTimer > 0)
        {
            coolDownTimer -= 0.1f;
        }
        else if(spotted == false && coolDownTimer < 10.0f)
        {
            coolDownTimer += 0.1f;
        }
        if (coolDownTimer >= 10 && spottedTimer < 10.0f)
        {
            spottedTimer += 0.1f;
        }
        if (spotted == true)
        {
            spottedTimer -= 0.1f;
            if (spottedTimer <= 0)
            {
                caught = true;
                animator.SetBool("Caught", true);
            }
        }
        if(caught == true)
        {
            spotLight.SetActive(true);
        }
        else
        {
            spotLight.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (IsInLocomotion() && ((direction >= 0 && horizontal >= 0) || (direction < 0 && horizontal < 0)))
        {
            //lerps between 0 and 0, 1, 0 if stick is held right, or 0, -1, 0 is stick is held left
            Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, rotDeg * (horizontal < 0.0f ? -1f : 1f), 0.0f), Mathf.Abs(horizontal));
            Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
            this.transform.rotation = this.transform.rotation * deltaRotation;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            airborne = false;
        }
        if (collision.gameObject.tag == "ElectricFence" && electricTimer <= 0.0f)
        {
            zapped = true;
            //StartCoroutine("BounceForce");
            animator.SetBool("Zapped", true);
            health -= 1;
            healthSlider.value = health;
            if (dead == false)
            {
                electricTimer = 20.0f;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Doorway"))
        {
            insideDoorArea = true;
        }
        if(other.gameObject.CompareTag("Key"))
        {
            keysHeld += 1;
            keyText.text = "Keys: " + keysHeld.ToString();
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("PlayerDetector"))
        {
            spotted = true;
        }
    }
   


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Doorway"))
        {
            insideDoorArea = false;
        }
        if (other.gameObject.CompareTag("PlayerDetector"))
        {
            spotted = false;
        }
    }

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

    //private IEnumerator BounceForce()
    //{
    //    rb.velocity = -rb.velocity;
    //    yield return new WaitForSeconds(3);
    //    rb.velocity = new Vector3(0, 0, 0);
    //}

    public bool IsInLocomotion()
    {
        return stateInfo.fullPathHash == Run_and_SprintID;
    }
}
