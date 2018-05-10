using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum BuildingTypes
{
    Square2x2 = 0,
    Square6x6,
    Rectangle2x6
}

public class BuildingPlacement : MonoBehaviour {

    public PlayerController playerController;
    public InputManager input;
    public Transform marsBase;
    private Camera buildCamera;
    public GameObject constructionSitePrefab;
    public GameObject longConstructionSitePrefab;
    public BuildingData currentBuildingData;
    private BuildUIManager buildUIManager;

    private Vector3 newCameraPos;
    private bool placingObject = false;
    public GameObject selectedBuilding;
    private CollisionDetector collisionDetector;
    private bool isInitialized = false;
    private Vector3 constructionSitePosition = new Vector3(0, -0.5f, 0);

    // Silhouette
    public Material silhouette;
    public Material[] allOriginalMaterials;
    private Color redSilhouette = new Color(1, 0, 0, 0.3137255f);
    private Color greenSilhouette = new Color(0, 1, 0, 0.3137255f);

    // Use this for initialization
    void Initialize() {
        isInitialized = true;
        buildCamera = GetComponentInChildren<Camera>();
        GetComponent<BuildUIManager>().CreateBuildingMenu();
        
        //Debug.Log("Cursor: confined");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        MoveCamera();
        if (placingObject) PlaceOnTerrain();
        if (placingObject && input.turnLeft) TurnBuilding(-90f);
        else if (placingObject && input.turnRight) TurnBuilding(90f);
    }

    private void OnDisable()
    {
        //Debug.Log("Cursor: locked");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        if (!isInitialized)
        {
            Debug.Log("Initializing buildingManager...");
            Initialize();
        }
        else
        {
            newCameraPos = playerController.transform.position;
            newCameraPos.y = 50f;
            buildCamera.transform.position = newCameraPos;

            //Debug.Log("Cursor: confined");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void TurnBuilding(float turnAngle)
    {
        Quaternion finalAngle = Quaternion.Euler(0, turnAngle, 0);
        Rigidbody rb = selectedBuilding.GetComponent<Rigidbody>();
        rb.MoveRotation(rb.rotation * finalAngle);
        Debug.Log("Turning building " + turnAngle + " degrees");
    }

    private void MoveCamera()
    {
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

        newCameraPos.x = input.h_Axis;
        newCameraPos.y = 0;
        newCameraPos.z = input.v_Axis;

        buildCamera.transform.position += newCameraPos * 10 * Time.deltaTime;
    }

    public void CreateBuilding(BuildingData data)
    {
        currentBuildingData = data;
        selectedBuilding = Instantiate(data.buildingPrefab, transform);
        selectedBuilding.transform.rotation = Quaternion.identity;
        selectedBuilding.name = data.buildingName;
        selectedBuilding.AddComponent<CollisionDetector>();
        collisionDetector = selectedBuilding.GetComponent<CollisionDetector>();

        allOriginalMaterials = selectedBuilding.GetComponent<Renderer>().materials;
        selectedBuilding.GetComponent<Renderer>().material = silhouette;
        //silhouette = selectedBuilding.GetComponent<Renderer>().material;
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

        if (collisionDetector.CheckCollision())
        {
            silhouette.color = redSilhouette;
        }
        else
        {
            silhouette.color = greenSilhouette;
            if (input.mouse_0_down)
            {
                PlaceBuilding();
            }
        }
        if (input.cancel) CancelBuild();
    }

    private void PlaceBuilding()
    {
        Vector3 saveLocation = SnapOnGrid(selectedBuilding.transform.position);
        //Debug.Log(saveLocation.ToString());
        selectedBuilding.transform.parent = marsBase;
        selectedBuilding.transform.position = saveLocation;
        //Debug.Log(selectedBuilding.transform.position.ToString());

        Destroy(selectedBuilding.GetComponent<CollisionDetector>());
        selectedBuilding.GetComponent<Renderer>().materials = allOriginalMaterials;
        selectedBuilding.GetComponent<Collider>().isTrigger = false;
        selectedBuilding.SetActive(false);

        GameObject constructionSite = null;
        BuildingTypes buildingType = currentBuildingData.type;
        switch (buildingType)
        {
            case BuildingTypes.Square2x2:
                constructionSite = Instantiate(constructionSitePrefab, selectedBuilding.transform.position + constructionSitePosition, selectedBuilding.transform.rotation) as GameObject;
                break;
            case BuildingTypes.Square6x6:
                constructionSite = Instantiate(constructionSitePrefab, selectedBuilding.transform.position + constructionSitePosition, selectedBuilding.transform.rotation) as GameObject;
                break;
            case BuildingTypes.Rectangle2x6:
                constructionSite = Instantiate(longConstructionSitePrefab, selectedBuilding.transform.position + constructionSitePosition, selectedBuilding.transform.rotation) as GameObject;
                break;
        }

        constructionSite.GetComponent<ConstructionSite>().InitializeConstructionSite(currentBuildingData);
        constructionSite.GetComponent<ConstructionSite>().linkedBuilding = selectedBuilding;

        currentBuildingData = null;
        selectedBuilding = null;
        placingObject = false;
    }

    private void CancelBuild()
    {
        Destroy(selectedBuilding);
        selectedBuilding = null;
        placingObject = false;
    }
}