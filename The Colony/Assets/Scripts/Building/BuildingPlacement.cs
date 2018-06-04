using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum BuildingTypes
{
    Square4x4 = 0,
    Square5x5,
    Square8x8,
    Rectangle8x3,
    Square10x10
}

public class BuildingPlacement : MonoBehaviour {

    public PlayerController playerController;
    public InputManager input;
    public Transform marsBase;
    private Camera buildCamera;
    public GameObject[] ConstructionSites;
    public BuildingData currentBuildingData;
    public BuildUIManager buildUIManager;
    public float gridSnap = 8f;

    private Vector3 newCameraPos;
    private bool placingObject = false;
    public GameObject selectedBuilding;
    private CollisionDetector collisionDetector;
    private BuildingController buildingController;
    private bool isInitialized = false;
    private Vector3 constructionSitePosition = new Vector3(0, 0.3f, 0);

    // Silhouette
    public Material silhouette;
    public Renderer[] allOriginalMaterials;
    private Color redSilhouette = new Color(1, 0, 0, 0.3137255f);
    private Color greenSilhouette = new Color(0, 1, 0, 0.3137255f);
    private BoxCollider[] allColliders;

    // Use this for initialization
    void Initialize() {
        isInitialized = true;
        buildCamera = GetComponentInChildren<Camera>();
        GetComponent<BuildUIManager>().CreateBuildingMenu();

        newCameraPos = playerController.transform.position;
        newCameraPos.y = 50f;
        buildCamera.transform.position = newCameraPos;
    }

    void Update()
    {
        MoveCamera();
        if (placingObject)
        {
            PlaceOnTerrain();
            buildUIManager.buildMenu.gameObject.SetActive(false);
        }
        else
        {
            buildUIManager.buildMenu.gameObject.SetActive(true);
        }
        if (placingObject && input.turnLeft) TurnBuilding(-90f);
        else if (placingObject && input.turnRight) TurnBuilding(90f);
    }

    private void OnDisable()
    {
        if (selectedBuilding != null)
        {
            Destroy(selectedBuilding);
        }
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
        }
    }

    private void TurnBuilding(float turnAngle)
    {
        Quaternion finalAngle = Quaternion.Euler(0, turnAngle, 0);
        Rigidbody rb = selectedBuilding.GetComponent<Rigidbody>();
        rb.MoveRotation(rb.rotation * finalAngle);
        //Debug.Log("Turning building " + turnAngle + " degrees");
    }

    private void MoveCamera()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (buildCamera.orthographicSize < 15)
            {
                buildCamera.orthographicSize += 1;
            } 
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (buildCamera.orthographicSize > 3)
            {
                buildCamera.orthographicSize -= 1;
            }
        }

        buildCamera.orthographicSize = Mathf.Clamp(buildCamera.orthographicSize + -Input.GetAxis("Mouse ScrollWheel") * 20, 3f, 15f);

        newCameraPos.x = input.h_Axis;
        newCameraPos.y = 0;
        newCameraPos.z = input.v_Axis;

        buildCamera.transform.position += newCameraPos * 12 * Time.deltaTime;
    }

    public void CreateBuilding(BuildingData data)
    {
        currentBuildingData = data;

        selectedBuilding = Instantiate(data.buildingPrefab, marsBase);
        selectedBuilding.transform.rotation = Quaternion.identity;
        selectedBuilding.name = data.buildingName;
        selectedBuilding.AddComponent<CollisionDetector>();
        selectedBuilding.AddComponent<Rigidbody>().isKinematic = true;
        buildingController = selectedBuilding.GetComponent<BuildingController>();
        if (buildingController != null)
        {
            buildingController.buildingData = data;
        }
        collisionDetector = selectedBuilding.GetComponent<CollisionDetector>();
        allColliders = null;

        allColliders = selectedBuilding.GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < allColliders.Length; i++)
        {
            if (allColliders[i] != selectedBuilding.GetComponent<BoxCollider>())
            {
                allColliders[i].enabled = false;
            }
        }

        allOriginalMaterials = selectedBuilding.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < allOriginalMaterials.Length; i++)
        {
            if (allOriginalMaterials[i].materials.Length == 2)
            {
                allOriginalMaterials[i].materials = new Material[2] { silhouette, silhouette };
            }
            else
            {
                allOriginalMaterials[i].material = silhouette;
            }
        }

        //silhouette = selectedBuilding.GetComponent<Renderer>().material;
        placingObject = true;
    }

    private Vector3 CurrentCursorLocation()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = buildCamera.transform.position.y;
        return buildCamera.ScreenToWorldPoint(mousePosition);
    }

    private Vector3 SnapOnGrid(Vector3 cursorPosition, float snapFactor)
    {
        Vector3 gridPosition = Vector3.zero;
        gridPosition.x = Mathf.RoundToInt(cursorPosition.x / snapFactor) * snapFactor;
        gridPosition.z = Mathf.RoundToInt(cursorPosition.z / snapFactor) * snapFactor;

        return gridPosition;
    }

    private void PlaceOnTerrain()
    {
        Vector3 buildingPosition = CurrentCursorLocation();
        selectedBuilding.transform.position = SnapOnGrid(buildingPosition, gridSnap);

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
        Vector3 saveLocation = SnapOnGrid(selectedBuilding.transform.position, gridSnap);
        //Debug.Log(saveLocation.ToString());
        Destroy(selectedBuilding);
        selectedBuilding = Instantiate(currentBuildingData.buildingPrefab, selectedBuilding.transform.position, selectedBuilding.transform.rotation, marsBase);
        selectedBuilding.transform.parent = marsBase;
        selectedBuilding.transform.position = saveLocation + currentBuildingData.buildingPrefab.transform.position;
        //Debug.Log(selectedBuilding.transform.position.ToString());

        buildingController = selectedBuilding.GetComponent<BuildingController>();
        if (buildingController != null)
        {
            buildingController.buildingData = currentBuildingData;
        }

        for (int i = 0; i < allColliders.Length; i++)
        {
            allColliders[i].enabled = true;
        }

        //Destroy(selectedBuilding.GetComponent<Collider>());
        Destroy(selectedBuilding.GetComponent<Rigidbody>());
        //selectedBuilding.GetComponent<Renderer>().materials = allOriginalMaterials;
        //selectedBuilding.GetComponent<Collider>().isTrigger = false;
        selectedBuilding.SetActive(false);

        GameObject constructionSite = null;
        BuildingTypes buildingType = currentBuildingData.type;
        switch (buildingType)
        {
            case BuildingTypes.Square4x4:
                constructionSite = Instantiate(ConstructionSites[(int)buildingType], selectedBuilding.transform.position + constructionSitePosition, selectedBuilding.transform.rotation) as GameObject;
                break;
            case BuildingTypes.Square5x5:
                constructionSite = Instantiate(ConstructionSites[(int)buildingType], selectedBuilding.transform.position + constructionSitePosition, selectedBuilding.transform.rotation) as GameObject;
                break;
            case BuildingTypes.Square8x8:
                constructionSite = Instantiate(ConstructionSites[(int)buildingType], selectedBuilding.transform.position + constructionSitePosition, selectedBuilding.transform.rotation) as GameObject;
                break;
            case BuildingTypes.Rectangle8x3:
                constructionSite = Instantiate(ConstructionSites[(int)buildingType], selectedBuilding.transform.position + constructionSitePosition, selectedBuilding.transform.rotation) as GameObject;
                break;
            case BuildingTypes.Square10x10:
                constructionSite = Instantiate(ConstructionSites[(int)buildingType], saveLocation + constructionSitePosition, selectedBuilding.transform.rotation) as GameObject;
                break;
        }

        constructionSite.GetComponentInChildren<ConstructionSite>().InitializeConstructionSite(currentBuildingData);
        constructionSite.GetComponentInChildren<ConstructionSite>().linkedBuilding = selectedBuilding;

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