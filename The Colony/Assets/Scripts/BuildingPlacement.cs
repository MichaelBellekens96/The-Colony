using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : InputManager {

    public PlayerController playerController;

    public Camera playerCamera;
    private AudioListener playerAudioListener;
    public Camera buildCamera;
    private AudioListener buildAudioListener;

    private Vector3 newCameraPos;
    private bool placingObject = false;
    public GameObject selectedBuilding;
    private CollisionDetector collisionDetector;
    public Material silhouette;
    private Color redSilhouette = new Color(1, 0, 0, 0.3137255f);
    private Color greenSilhouette = new Color(0, 1, 0, 0.3137255f);
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
        if (placingObject) PlaceOnTerrain();
    }

    public void ToggleBuildMenu()
    {
        // When player camera is still active switch to overview camera
        if (playerCamera.enabled == true)
        {
            playerCamera.enabled = false;
            playerAudioListener.enabled = false;

            newCameraPos = playerController.transform.position;
            newCameraPos.y = 50f;
            buildCamera.transform.position = newCameraPos;

            buildCamera.enabled = true;
            buildAudioListener.enabled = true;

            playerController.freezed = true;

            Debug.Log("Cursor: confined");
            Cursor.lockState = CursorLockMode.None;
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
        //h_Axis = Input.GetAxis("Horizontal");
        //v_Axis = Input.GetAxis("Vertical");

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
        newCameraPos.y = 0;
        newCameraPos.z = v_Axis;

        buildCamera.transform.position += newCameraPos * 10 * Time.deltaTime;
    }

    public void CreateBuilding(GameObject gameObject)
    {
        selectedBuilding = Instantiate(gameObject, transform);
        selectedBuilding.AddComponent<CollisionDetector>();
        collisionDetector = selectedBuilding.GetComponent<CollisionDetector>();
        collisionDetector.buildCamera = buildCamera;
        silhouette = selectedBuilding.GetComponent<Renderer>().material;
        placingObject = true;
    }

    private Vector3 CurrentCursorLocation()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = buildCamera.transform.position.y;
        return buildCamera.ScreenToWorldPoint(mousePosition);
    }

    private Vector3 SnapOnGrid(Vector3 cursorPosition)
    {
        Vector3 gridPosition = Vector3.zero;
        gridPosition.x = Mathf.RoundToInt(cursorPosition.x / 1) * 1;
        gridPosition.z = Mathf.RoundToInt(cursorPosition.z / 1) * 1;

        return gridPosition;
    }

    private void PlaceOnTerrain()
    {
        Vector3 buildingPosition = CurrentCursorLocation();
        selectedBuilding.transform.position = SnapOnGrid(buildingPosition);
        selectedBuilding.transform.rotation = Quaternion.identity;

        if (collisionDetector.CheckCollision())
        {
            silhouette.color = redSilhouette;
        }
        else
        {
            silhouette.color = greenSilhouette;
        }

        // check mouse click
        // add to base object
        // cancel building placement

    }

    /*private bool CheckCollision()
    {
        
    }*/
}
