﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 10f;
    public bool isGrounded;

    [Header("Rotation")]
    public float x_Sensitivity = 5f;
    public float y_Sensitivity = 5f;

    private float speed;

    private const string H_AXIS = "Horizontal";
    private const string V_AXIS = "Vertical";
    private const string MOUSE_X = "Mouse X";
    private const string MOUSE_Y = "Mouse Y";
    private const string JUMP = "Jump";

    private float x_AxisMouse;
    private float y_AxisMouse;
    private float v_Axis;
    private float h_Axis;
    private bool l_Shift;
    private bool jump;

    private Rigidbody rb;
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
        x_AxisMouse = Input.GetAxis(MOUSE_X);
        y_AxisMouse = Input.GetAxis(MOUSE_Y);
        v_Axis = Input.GetAxis(V_AXIS);
        h_Axis = Input.GetAxis(H_AXIS);
        l_Shift = Input.GetKey(KeyCode.LeftShift);
        jump = Input.GetButton(JUMP);

        LookRotation(transform, playerCamera.transform);

        #region TestingCode
        if (Input.GetKeyDown(KeyCode.H))
        {
            playerStats.Heal(5f);
            Debug.Log("Healed player with 5");
        }
        #endregion

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        speed = l_Shift ? sprintSpeed : walkSpeed;

        rb.MovePosition(transform.position + (transform.forward * v_Axis + transform.right * h_Axis) * speed * Time.deltaTime);

        if (jump && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void LookRotation(Transform character, Transform camera)
    {
        float yRot = x_AxisMouse * x_Sensitivity;
        float xRot = y_AxisMouse * y_Sensitivity;
        character.localRotation *= Quaternion.Euler(0f, yRot, 0f);
        camera.localRotation *= Quaternion.Euler(-xRot, 0f, 0f);
        camera.localRotation = ClampRotationAroundXAxis(camera.localRotation);
    }

    private Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;
        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, -90f, 90f);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
        return q;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = collision.gameObject.layer == 9 ? true : false;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = collision.gameObject.layer == 9 ? false : true;
    }
}