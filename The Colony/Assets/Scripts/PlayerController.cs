using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Rigidbody rb;

    public float walkSpeed = 10f;
    public float sprintSpeed = 15f;
    public float rotationSpeed = 5f;
    public float jumpForce = 20f;
    public bool isGrounded;
    public float cameraRotation;

    private float raycastLength = 0.55f;

    private const string H_AXIS = "Horizontal";
    private const string V_AXIS = "Vertical";
    private const string MOUSE_X = "Mouse X";
    private const string MOUSE_Y = "Mouse Y";
    private const string JUMP = "Jump";

    private Camera playerCamera;
    private PlayerStats playerStats;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        playerStats = GetComponent<PlayerStats>();
	}
	
	// Update is called once per frame
	void Update () {
        isGrounded = Physics.Raycast(transform.position, -transform.up, raycastLength, 1 << 9);
        Debug.DrawLine(transform.position, transform.position - new Vector3(0, raycastLength, 0), Color.red);

        Move();
        MouseLook();

        if (Input.GetKeyDown(KeyCode.H))
        {
            playerStats.Heal(5f);
            Debug.Log("Healed player with 5");
        }
        
    }

    private void Move()
    {
        if (Input.GetButton(V_AXIS))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb.position += transform.forward * (Input.GetAxis(V_AXIS) * sprintSpeed) * Time.deltaTime;
            }
            else
            {
                rb.position += transform.forward * (Input.GetAxis(V_AXIS) * walkSpeed) * Time.deltaTime;
            }

        }
        if (Input.GetButton(H_AXIS))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb.position += transform.right * (Input.GetAxis(H_AXIS) * sprintSpeed) * Time.deltaTime;
            }
            else
            {
                rb.position += transform.right * (Input.GetAxis(H_AXIS) * walkSpeed) * Time.deltaTime;
            }
        }
        if (Input.GetButton(JUMP) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void MouseLook()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, Input.GetAxis(MOUSE_X) * rotationSpeed, 0)));

        cameraRotation = playerCamera.transform.localEulerAngles.x + -Input.GetAxis(MOUSE_Y) * rotationSpeed;

        playerCamera.transform.localEulerAngles = new Vector3(cameraRotation, 0, 0);
    }
}
