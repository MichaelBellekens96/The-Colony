using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Tools
{
    Hands = 0,
    Drill,
    Welder,
    Shovel
}

public class PlayerController : MonoBehaviour {

    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float jumpForce;
    public bool isGrounded = true;
    public bool freezed;
    public bool allowJump = true;
    private float speed;

    [Header("Rotation")]
    public float x_Sensitivity = 5f;
    public float y_Sensitivity = 5f;
    private float yRot;
    private float xRot;
    private float angleX;

    [Header("Tools")]
    public int indexTool = 0;
    public Transform toolsHolder;
    public GameObject[] tools = new GameObject[4];
    public GameObject flashlight;

    [Header("Script references")]
    public BuildingPlacement build;
    public InputManager input;
    private PlayerStats playerStats;
    private PlayerTasks playerTasks;

    private static Rigidbody rb;
    private static Camera playerCamera;
    private RaycastHit hit;
    private RaycastHit collisionHit;
    private RaycastHit inBaseHit;
    private string tagHit;
    private GameObject hitGameObject;
    private Vector3 startPosition = new Vector3(0, 0.8f, 0);
    public bool insideBase;
    private bool machineInRange;

    public float testFloat;
    public Vector3 lastPosition = Vector3.zero;
    public float v_input;
    public float playerSpeed = 0;
    private bool isSleeping = false;
    public GameObject hittingObject;

    private bool DirtWalking = false;
    private bool DirtRunning = false;
    private bool MetalRunning = false;
    private bool MetalWalking = false;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        playerStats = GetComponent<PlayerStats>();
        playerTasks = GetComponent<PlayerTasks>();

        playerCamera.enabled = true;

        InstantiateTools();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        LookRotation(transform, playerCamera.transform);
        if (input.interact && playerTasks.isCarryingObject && !machineInRange)
        {
            Debug.Log("Dropped ResourceBox");
            playerTasks.DropObject();
        }
        if (input.mouse_0) CheckTool(indexTool);
        else
        {
            AudioManager.Instance.Stop("Drill");
            AudioManager.Instance.Stop("Welder");
            playerTasks.playingWelderSound = false;
        }
        if (input.flashlight) ToggleFlashlight();

        #region TestingCode
        if (Input.GetKeyDown(KeyCode.H))
        {
            playerStats.Heal(5f);
            Debug.Log("Healed player with 5");
        }
        if (Input.GetKeyDown(KeyCode.F9)) playerStats.Save();
        if (Input.GetKeyDown(KeyCode.F10)) playerStats.Load();
        #endregion
    }

    private void FixedUpdate()
    {
        Move();
        CheckCollision();
    }

    private void InstantiateTools()
    {
        for (int i = 0; i < tools.Length; i++)
        {
            if (tools[i] != null)
            {
                GameObject selectedTool = Instantiate(tools[i], toolsHolder) as GameObject;
                selectedTool.transform.localPosition = new Vector3(0, 0, 1);
                selectedTool.transform.rotation = Quaternion.identity;
                tools[i] = selectedTool;
                selectedTool.gameObject.SetActive(false);
            }
        }
        indexTool = 0;
        ChangeTool(indexTool);
    }

    private void Move()
    {
        speed = input.l_Shift ? sprintSpeed : walkSpeed;

        lastPosition = transform.position;

        v_input = CheckMovementCollision();
        rb.MovePosition(transform.position + (transform.forward * v_input + transform.right * input.h_Axis) * speed * Time.deltaTime);

        if (v_input != 0 && speed != sprintSpeed || input.h_Axis != 0 && speed != sprintSpeed)
        {
            playerCamera.transform.localPosition = startPosition + new Vector3(0.0f, 0.02f * Mathf.Sin(10 * Time.time), 0.0f);
        }
        else if (v_input != 0 && speed == sprintSpeed || input.h_Axis != 0 && speed == sprintSpeed)
        {
            playerCamera.transform.localPosition = startPosition + new Vector3(0.0f, 0.03f * Mathf.Sin(20 * Time.time), 0.0f);
        }

        if (input.v_Axis != 0 && speed == walkSpeed || input.h_Axis != 0 && speed == walkSpeed)
        {
            AudioManager.Instance.Stop("Running_Sand");
            DirtRunning = false;
            if (DirtWalking == false)
            {
                Debug.Log("Playing walking dirt sound");
                DirtWalking = true;
                AudioManager.Instance.Play("Walking_Sand");
            }
        }
        else if (input.v_Axis != 0 && speed == sprintSpeed || input.h_Axis != 0 && speed == sprintSpeed)
        {
            AudioManager.Instance.Stop("Walking_Sand");
            DirtWalking = false;
            if (DirtRunning == false)
            {
                Debug.Log("Playing running dirt sound");
                DirtRunning = true;
                AudioManager.Instance.Play("Running_Sand");
            }
        }
        else
        {
            AudioManager.Instance.Stop("Walking_Sand");
            AudioManager.Instance.Stop("Running_Sand");
            DirtWalking = false;
            DirtRunning = false;
        }

        if (Time.time <= 1)
        {
            playerSpeed = transform.position.z;
        }

        if (input.jump && allowJump)
        {
            Debug.DrawLine(transform.position, transform.position + new Vector3(0, -1f, 0), Color.yellow);
            if (Physics.Raycast(transform.position, -transform.up, 1f))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                StartCoroutine(JumpTimeOut());
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }

    private float CheckMovementCollision()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.yellow);
        if (Physics.Raycast(transform.position, transform.forward, out collisionHit,0.8f, 1 << LayerMask.NameToLayer("Base")))
        {
            //Debug.Log("Hitting base");

            if (input.v_Axis > 0 && !collisionHit.collider.isTrigger)
            {
                return 0;
            }
            else
            {
                return input.v_Axis;
            }
        }
        else
        {
            return input.v_Axis;
        }
    }

    private void CheckCollision()
    {
        int layermask = 1 << 13;
        layermask = ~layermask;
        GameObject lastConstructionSite = null;
        hittingObject = null;
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 5, Color.red);
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out collisionHit, 5f, layermask))
        {
            hittingObject = collisionHit.transform.gameObject;
            //MainUIManager.Instance.ToggleInteractionText(false);
            if (collisionHit.transform.gameObject.layer == 12)
            {
                if (!MainUIManager.Instance.constructionPanel.activeSelf || collisionHit.transform.gameObject != lastConstructionSite)
                {
                    if (collisionHit.transform.gameObject.GetComponent<ConstructionSite>())
                    {
                        ConstructionSite data = collisionHit.transform.gameObject.GetComponent<ConstructionSite>();
                        MainUIManager.Instance.UpdateConstructionPanel(data.numNeededMetal, data.numNeededBioPlastic, data.buildingName);
                        MainUIManager.Instance.UpdateBuildPercentage(data.buildPercentage);
                        MainUIManager.Instance.ToggleConstructionPanel(true);
                    }
                }
                lastConstructionSite = collisionHit.transform.gameObject;
            }
            else
            {
                MainUIManager.Instance.ToggleConstructionPanel(false);
            }

            if (collisionHit.transform.tag == "ResourceBox")
            {
                if (input.interact && !playerTasks.isCarryingObject && indexTool == 0)
                {
                    Debug.Log("ResourceBox picked up");
                    playerTasks.CarryObject(collisionHit.transform.gameObject);
                    StartCoroutine(DelayCarryObject());

                }
                if (!playerTasks.isCarryingObject && indexTool == 0)
                {
                    MainUIManager.Instance.SetInteractionText("Press 'F' to pick up the resource box");
                    MainUIManager.Instance.ToggleInteractionText(true);
                }
                else
                {
                    MainUIManager.Instance.ToggleInteractionText(false);
                }

                //Debug.Log("Hitting resource box");
            }
            else if (collisionHit.transform.tag == "Contruction Site")
            {
                if (!playerTasks.isCarryingObject && collisionHit.transform.GetComponent<ConstructionSite>().allResourcesPresent && indexTool == 2)
                {
                    MainUIManager.Instance.SetInteractionText("Press 'Left Mouse Button' to construct your building");
                    MainUIManager.Instance.ToggleInteractionText(true);
                }
                else if (!playerTasks.isCarryingObject && collisionHit.transform.GetComponent<ConstructionSite>().allResourcesPresent)
                {
                    MainUIManager.Instance.SetInteractionText("Use your 'Welder' and press 'Left Mouse Button' to construct your building");
                    MainUIManager.Instance.ToggleInteractionText(true);
                }
                else
                {
                    MainUIManager.Instance.ToggleInteractionText(false);
                }
            }
            else if (collisionHit.transform.tag == "AirlockPanel")
            {
                Debug.Log("Hitting an AirlockPanel");
                if (input.interact && !playerTasks.isCarryingObject)
                {
                    AudioManager.Instance.Play("Airlock");
                    playerTasks.UseAirlockPanel();
                    MainUIManager.Instance.ToggleInteractionText(false);
                }
                else
                {
                    Debug.Log("Looking at AirlockPanel");
                    MainUIManager.Instance.SetInteractionText("Press 'F' to use the 'Airlock'");
                    MainUIManager.Instance.ToggleInteractionText(true);
                }
            }
            else if (collisionHit.transform.tag == "Metal Ore")
            {
                if (!playerTasks.isCarryingObject && indexTool == 1)
                {
                    MainUIManager.Instance.SetInteractionText("Press 'Left Mouse Button' to drill for 'Metal Ore'");
                    MainUIManager.Instance.ToggleInteractionText(true);
                }
                else if (!playerTasks.isCarryingObject)
                {
                    MainUIManager.Instance.SetInteractionText("Use your 'Drill' and press 'Left Mouse Button' to drill for 'Metal Ore'");
                    MainUIManager.Instance.ToggleInteractionText(true);
                }
                else
                {
                    MainUIManager.Instance.ToggleInteractionText(false);
                }

            }
            else if (collisionHit.transform.tag == "Plants")
            {
                if (!playerTasks.isCarryingObject)
                {
                    PlantController plantController = collisionHit.transform.GetComponent<PlantController>();
                    if (!plantController.mayHarvest && !plantController.plantsFullyGrown && !plantController.plantsGrowing)
                    {
                        MainUIManager.Instance.SetInteractionText("Press 'F' to plant seeds");
                        MainUIManager.Instance.ToggleInteractionText(true);
                        if (input.interact)
                        {
                            AudioManager.Instance.Play("Throw_Seeds");
                            plantController.GrowPlants();
                        }
                    }
                    else if (plantController.mayHarvest && plantController.plantsFullyGrown)
                    {
                        MainUIManager.Instance.SetInteractionText("Press 'F' to harvest your crops");
                        MainUIManager.Instance.ToggleInteractionText(true);
                        if (input.interact)
                        {
                            plantController.HarvestPlants(transform.position, transform.rotation);
                        }
                    }
                    else
                    {
                        MainUIManager.Instance.ToggleInteractionText(false);
                    }
                }
            }
            else if (collisionHit.transform.tag == "HydraulicPress")
            {
                if (playerTasks.isCarryingObject && !collisionHit.transform.GetComponent<HydraulicPress>().processingMetalOre)
                {
                    if (playerTasks.grabbedObject.GetComponent<ResourceBox>().type == ResourceTypes.MetalOre)
                    {
                        MainUIManager.Instance.SetInteractionText("Press 'F' to convert your 'Metal Ore' to 'Metal'");
                        MainUIManager.Instance.ToggleInteractionText(true);
                        if (input.interact)
                        {
                            collisionHit.transform.GetComponent<HydraulicPress>().StartPress();
                            ResourceManager.Instance.RemoveResourceBox(playerTasks.grabbedObject.transform);
                            playerTasks.isCarryingObject = false;
                            MainUIManager.Instance.ToggleInteractionText(false);
                        }
                    }
                }
                else if (!playerTasks.isCarryingObject && collisionHit.transform.GetComponent<HydraulicPress>().metalReady)
                {
                    MainUIManager.Instance.SetInteractionText("Press 'F' to receive your 'Metal'");
                    MainUIManager.Instance.ToggleInteractionText(true);
                    if (input.interact)
                    {
                        collisionHit.transform.GetComponent<HydraulicPress>().GrabMetalBox();
                        ResourceManager.Instance.CreateResourceBox(ResourceTypes.Metal, transform.position + Vector3.forward * 0.5f, transform.rotation);
                        MainUIManager.Instance.ToggleInteractionText(false);
                    }
                }
                else
                {
                    MainUIManager.Instance.ToggleInteractionText(false);
                }
            }
            else if (collisionHit.transform.tag == "BioplasticOven")
            {
                if (playerTasks.isCarryingObject && !collisionHit.transform.GetComponent<BioPlasticOven>().ovenIsBaking)
                {
                    if (playerTasks.grabbedObject.GetComponent<ResourceBox>().type == ResourceTypes.RawFood)
                    {
                        MainUIManager.Instance.SetInteractionText("Press 'F' to convert your 'Raw Food' to 'Bioplastic'");
                        MainUIManager.Instance.ToggleInteractionText(true);
                        if (input.interact)
                        {
                            collisionHit.transform.GetComponent<BioPlasticOven>().StartOven();
                            ResourceManager.Instance.RemoveResourceBox(playerTasks.grabbedObject.transform);
                            playerTasks.isCarryingObject = false;
                            MainUIManager.Instance.ToggleInteractionText(false);
                        }
                    }
                }
                else if (!playerTasks.isCarryingObject && collisionHit.transform.GetComponent<BioPlasticOven>().BioPlasticReady)
                {
                    MainUIManager.Instance.SetInteractionText("Press 'F' to receive your 'Bioplastic'");
                    MainUIManager.Instance.ToggleInteractionText(true);
                    if (input.interact)
                    {
                        collisionHit.transform.GetComponent<BioPlasticOven>().GrabBioPlastic();
                        ResourceManager.Instance.CreateResourceBox(ResourceTypes.BioPlastic, transform.position + Vector3.forward * 0.5f, transform.rotation);
                        MainUIManager.Instance.ToggleInteractionText(false);
                    }
                }
                else
                {
                    MainUIManager.Instance.ToggleInteractionText(false);
                }
            }
            else if (collisionHit.transform.tag == "FoodProcessor")
            {
                if (playerTasks.isCarryingObject && playerTasks.grabbedObject.GetComponent<ResourceBox>())
                {
                    if (playerTasks.grabbedObject.GetComponent<ResourceBox>().type == ResourceTypes.RawFood)
                    {
                        machineInRange = true;
                        MainUIManager.Instance.SetInteractionText("Press 'F' to add 'Raw Food' to the 'Food Processor'");
                        MainUIManager.Instance.ToggleInteractionText(true);
                        if (input.interact)
                        {
                            Debug.Log("Added meal to foodprocessor");
                            collisionHit.transform.GetComponent<FoodProcessor>().AddMeal(playerTasks.grabbedObject);
                            playerTasks.isCarryingObject = false;
                            machineInRange = false;
                        }
                    }
                }
                else if (!playerTasks.isCarryingObject && ResourceManager.Instance.numMeals > 0)
                {
                    machineInRange = false;
                    MainUIManager.Instance.SetInteractionText("Press 'F' to get a 'Meal' from the 'Food Processor'");
                    MainUIManager.Instance.ToggleInteractionText(true);
                    if (input.interact)
                    {
                        collisionHit.transform.GetComponent<FoodProcessor>().PrepareMeal();
                        playerStats.Hunger += 50;
                        MainUIManager.Instance.UpdateStatsPanel();
                    }
                }
                else
                {
                    MainUIManager.Instance.ToggleInteractionText(false);
                }
            }
            else if (collisionHit.transform.tag == "StorageMetal")
            {
                if (playerTasks.isCarryingObject && playerTasks.grabbedObject.GetComponent<ResourceBox>())
                {
                    if (playerTasks.grabbedObject.GetComponent<ResourceBox>().type == ResourceTypes.Metal)
                    {
                        MainUIManager.Instance.SetInteractionText("Press 'F' to store your 'Metal'");
                        MainUIManager.Instance.ToggleInteractionText(true);
                        if (input.interact)
                        {
                            AudioManager.Instance.Play("AddToStorage");
                            ResourceManager.Instance.MetalBox.Remove(playerTasks.grabbedObject.transform);
                            Destroy(playerTasks.grabbedObject);
                            playerTasks.isCarryingObject = false;
                            MainUIManager.Instance.ToggleInteractionText(false);
                            ResourceManager.Instance.numStoredMetalBoxes++;
                        }
                    }
                    else
                    {
                        MainUIManager.Instance.ToggleInteractionText(false);
                    }
                }
                else if (!playerTasks.isCarryingObject && ResourceManager.Instance.numStoredMetalBoxes > 0)
                {
                    MainUIManager.Instance.SetInteractionText("Press 'F' to get 'Metal'");
                    MainUIManager.Instance.ToggleInteractionText(true);
                    if (input.interact)
                    {
                        ResourceManager.Instance.CreateExistingResourceBox(ResourceTypes.Metal, transform.position + transform.forward * 0.5f, transform.rotation);
                        MainUIManager.Instance.ToggleInteractionText(false);
                        ResourceManager.Instance.numStoredMetalBoxes--;
                    }
                }
                else
                {
                    MainUIManager.Instance.ToggleInteractionText(false);
                }
            }
            else if (collisionHit.transform.tag == "StorageMetalOre")
            {
                if (playerTasks.isCarryingObject && playerTasks.grabbedObject.GetComponent<ResourceBox>())
                {
                    if (playerTasks.grabbedObject.GetComponent<ResourceBox>().type == ResourceTypes.MetalOre)
                    {
                        MainUIManager.Instance.SetInteractionText("Press 'F' to store your 'Metal Ore'");
                        MainUIManager.Instance.ToggleInteractionText(true);
                        if (input.interact)
                        {
                            AudioManager.Instance.Play("AddToStorage");
                            ResourceManager.Instance.MetalOreBox.Remove(playerTasks.grabbedObject.transform);
                            Destroy(playerTasks.grabbedObject);
                            playerTasks.isCarryingObject = false;
                            MainUIManager.Instance.ToggleInteractionText(false);
                            ResourceManager.Instance.numStoredMetalBoxes++;
                        }
                    }
                    else
                    {
                        MainUIManager.Instance.ToggleInteractionText(false);
                    }
                }
                else if (!playerTasks.isCarryingObject && ResourceManager.Instance.numStoredMetalOreBoxes > 0)
                {
                    MainUIManager.Instance.SetInteractionText("Press 'F' to get 'Metal Ore'");
                    MainUIManager.Instance.ToggleInteractionText(true);
                    if (input.interact)
                    {
                        ResourceManager.Instance.CreateExistingResourceBox(ResourceTypes.MetalOre, transform.position + transform.forward * 0.5f, transform.rotation);
                        MainUIManager.Instance.ToggleInteractionText(false);
                        ResourceManager.Instance.numStoredMetalOreBoxes--;
                    }
                }
                else
                {
                    MainUIManager.Instance.ToggleInteractionText(false);
                }
            }
            else if (collisionHit.transform.tag == "StorageRawFood")
            {
                if (playerTasks.isCarryingObject && playerTasks.grabbedObject.GetComponent<ResourceBox>())
                {
                    if (playerTasks.grabbedObject.GetComponent<ResourceBox>().type == ResourceTypes.RawFood)
                    {
                        MainUIManager.Instance.SetInteractionText("Press 'F' to store your 'RawFood'");
                        MainUIManager.Instance.ToggleInteractionText(true);
                        if (input.interact)
                        {
                            AudioManager.Instance.Play("AddToStorage");
                            ResourceManager.Instance.RawFoodBox.Remove(playerTasks.grabbedObject.transform);
                            Destroy(playerTasks.grabbedObject);
                            playerTasks.isCarryingObject = false;
                            MainUIManager.Instance.ToggleInteractionText(false);
                            ResourceManager.Instance.numStoredRawFoodBoxes++;
                        }
                    }
                    else
                    {
                        MainUIManager.Instance.ToggleInteractionText(false);
                    }
                }
                else if (!playerTasks.isCarryingObject && ResourceManager.Instance.numStoredRawFoodBoxes > 0)
                {
                    MainUIManager.Instance.SetInteractionText("Press 'F' to get 'RawFood'");
                    MainUIManager.Instance.ToggleInteractionText(true);
                    if (input.interact)
                    {
                        ResourceManager.Instance.CreateExistingResourceBox(ResourceTypes.RawFood, transform.position + transform.forward * 0.5f, transform.rotation);
                        MainUIManager.Instance.ToggleInteractionText(false);
                        ResourceManager.Instance.numStoredRawFoodBoxes--;
                    }
                }
                else
                {
                    MainUIManager.Instance.ToggleInteractionText(false);
                }
            }
            else if (collisionHit.transform.tag == "StorageBioPlastic")
            {
                if (playerTasks.isCarryingObject && playerTasks.grabbedObject.GetComponent<ResourceBox>())
                {
                    if (playerTasks.grabbedObject.GetComponent<ResourceBox>().type == ResourceTypes.BioPlastic)
                    {
                        MainUIManager.Instance.SetInteractionText("Press 'F' to store your 'Bioplastic'");
                        MainUIManager.Instance.ToggleInteractionText(true);
                        if (input.interact)
                        {
                            AudioManager.Instance.Play("AddToStorage");
                            ResourceManager.Instance.BioPlasticBox.Remove(playerTasks.grabbedObject.transform);
                            Destroy(playerTasks.grabbedObject);
                            playerTasks.isCarryingObject = false;
                            MainUIManager.Instance.ToggleInteractionText(false);
                            ResourceManager.Instance.numStoredBioplasticBoxes++;
                        }
                    }
                    else
                    {
                        MainUIManager.Instance.ToggleInteractionText(false);
                    }
                }
                else if (!playerTasks.isCarryingObject && ResourceManager.Instance.numStoredBioplasticBoxes > 0)
                {
                    MainUIManager.Instance.SetInteractionText("Press 'F' to get 'Bioplastic'");
                    MainUIManager.Instance.ToggleInteractionText(true);
                    if (input.interact)
                    {
                        ResourceManager.Instance.CreateExistingResourceBox(ResourceTypes.BioPlastic, transform.position + transform.forward * 0.5f, transform.rotation);
                        MainUIManager.Instance.ToggleInteractionText(false);
                        ResourceManager.Instance.numStoredBioplasticBoxes--;
                    }
                }
                else
                {
                    MainUIManager.Instance.ToggleInteractionText(false);
                }
            }
            else if (collisionHit.transform.tag == "BunkBed")
            {
                if (!playerTasks.isCarryingObject)
                {
                    MainUIManager.Instance.SetInteractionText("Press 'F' to use 'Bunkbed'");
                    MainUIManager.Instance.ToggleInteractionText(true);

                    if (input.interact)
                    {
                        MainUIManager.Instance.GoToSleep();
                        MainUIManager.Instance.ToggleInteractionText(false);
                        playerStats.Sleep = 100;
                    }
                }
            }
            else if (collisionHit.transform.gameObject.layer == 11)
            {
                GameObject building = collisionHit.transform.gameObject;
                if (Input.GetKeyDown(KeyCode.B))
                {
                    BaseManager.Instance.DisableBuilding(building.GetComponentInParent<BuildingController>().gameObject);
                    Debug.Log("Disabling building...");
                }
                if (Input.GetKeyDown(KeyCode.V))
                {
                    BaseManager.Instance.EnableBuilding(building.GetComponentInParent<BuildingController>().gameObject);
                    Debug.Log("Enabling building...");
                }
            }
            
        }
        else
        {
            MainUIManager.Instance.ToggleInteractionText(false);
        }
    }

    public void LookRotation(Transform character, Transform camera)
    {
        yRot = input.x_AxisMouse * x_Sensitivity;
        xRot = input.y_AxisMouse * y_Sensitivity;
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
        angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, -90f, 90f);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
        return q;
    }

    private void CheckTool(int indexActiveTool)
    {
        switch (indexActiveTool)
        {
            case (int)Tools.Hands:
                // do nothing
                break;
            case (int)Tools.Drill:
                playerTasks.Drill();
                break;
            case (int)Tools.Welder:
                playerTasks.Weld();
                break;
            case (int)Tools.Shovel:
                playerTasks.Dig();
                break;
            default:
                break;
        }
    }

    public void ChangeTool(int index)
    {
        for (int i = 0; i < tools.Length; i++)
        {
            if (index == i)
            {
                tools[i].SetActive(true);
                indexTool = i;
                MainUIManager.Instance.UpdateToolPanel(index);
            }
            else
            {
                tools[i].SetActive(false);
            }
        }
    }

    private void ToggleFlashlight()
    {
        flashlight.SetActive(!flashlight.activeSelf);
        AudioManager.Instance.Play("Flashlight");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Base")
        {
            rb.drag = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Base")
        {
            rb.drag = 0.2f;
        }
    }

    public bool CheckStillInBase()
    {
        if (Physics.Raycast(transform.position, -transform.up, out inBaseHit, 3f, 1 << LayerMask.NameToLayer("Base")))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator DelayCarryObject()
    {
        yield return new WaitForSeconds(0.1f);
        playerTasks.isCarryingObject = true;
    }

    public IEnumerator JumpTimeOut()
    {
        allowJump = false;
        yield return new WaitForSeconds(0.7f);
        AudioManager.Instance.Play("Jump_Sand");
        yield return new WaitForSeconds(0.5f);
        allowJump = true;
    }
}