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
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
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

    [Header("Script references")]
    public BuildingPlacement build;
    public InputManager input;
    private PlayerStats playerStats;
    private PlayerTasks playerTasks;

    private static Rigidbody rb;
    private static Camera playerCamera;
    private RaycastHit hit;
    private string tagHit;
    private GameObject hitGameObject;

    public float testFloat;

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
        if (input.cancel && playerTasks.isCarryingObject) playerTasks.DropObject();
        if (input.mouse_0) CheckTool(indexTool);

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

    private void InstantiateTools()
    {
        for (int i = 0; i < tools.Length; i++)
        {
            if (tools[i] != null)
            {
                GameObject selectedTool = Instantiate(tools[i], toolsHolder) as GameObject;
                selectedTool.transform.localPosition = new Vector3(0, 0, 1);
                selectedTool.transform.localRotation = Quaternion.identity;
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

        rb.MovePosition(transform.position + (transform.forward * input.v_Axis + transform.right * input.h_Axis) * speed * Time.deltaTime);

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Base")
        {
            rb.drag = 1;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 5f, Color.yellow);
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 5f))
        {
            tagHit = hit.transform.gameObject.tag;
            hitGameObject = hit.transform.gameObject;

            switch (tagHit)
            {
                case "ResourceBox":
                    if (input.interact && !playerTasks.isCarryingObject) playerTasks.CarryObject(hitGameObject);
                    Debug.Log("Hitting resource box");
                    // Show UI hint
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Base")
        {
            rb.drag = 0.2f;
        }
    }

    public IEnumerator JumpTimeOut()
    {
        allowJump = false;
        yield return new WaitForSeconds(1.2f);
        allowJump = true;
    }
}