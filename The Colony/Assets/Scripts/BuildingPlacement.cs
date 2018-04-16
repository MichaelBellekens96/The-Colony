using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour {

    public PlayerController playerController;

    private Camera playerCamera;
    private AudioListener playerAudioListener;
    private Camera buildCamera;
    private AudioListener buildAudioListener;

    private float h_Axis;
    private float v_Axis;
    private float height;
    private Vector3 newCameraPos;

	// Use this for initialization
	void Start () {
        buildCamera = GetComponentInChildren<Camera>();
        buildAudioListener = GetComponentInChildren<AudioListener>();
        playerCamera = playerController.GetComponentInChildren<Camera>();
        playerAudioListener = playerController.GetComponentInChildren<AudioListener>();

        buildCamera.enabled = false;
        buildAudioListener.enabled = false;
	}

    private void Update()
    {
        if (buildCamera.enabled) MoveCamera();
    }

    public void ToggleBuildMenu()
    {
        // When player camera is still active switch to overview camera
        if (playerCamera.enabled == true)
        {
            playerCamera.enabled = false;
            playerAudioListener.enabled = false;

            buildCamera.enabled = true;
            buildAudioListener.enabled = true;

            playerController.freezed = true;

            Debug.Log("Cursor: confined");
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        // When overview camera is still active switch to player camera
        else
        {
            playerCamera.enabled = true;
            playerAudioListener.enabled = true;

            buildCamera.enabled = false;
            buildAudioListener.enabled = false;

            playerController.freezed = false;

            Debug.Log("Cursor: locked");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void MoveCamera()
    {
        h_Axis = Input.GetAxis("Horizontal");
        v_Axis = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (buildCamera.orthographicSize < 12)
            {
                buildCamera.orthographicSize += 1;
            } 
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (buildCamera.orthographicSize > 3)
            {
                buildCamera.orthographicSize -= 1;
            }
        }

        newCameraPos.x = h_Axis;
        newCameraPos.z = v_Axis;

        buildCamera.transform.position += newCameraPos * 10 * Time.deltaTime;
    }
}
