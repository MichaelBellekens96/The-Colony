using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTasks : MonoBehaviour {
    
    public GameObject grabbedObject;
    public bool isCarryingObject = false;
    private RaycastHit hit;
    private Camera playerCamera;
    private PlayerController playerController;
    private PlayerStats playerStats;
    public float tempOxygenRate;
    public float timer = 0;

    public bool playingDrillSound = false;
    public bool playingWelderSound = false;

    private void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        playerController = GetComponent<PlayerController>();
        playerStats = GetComponent<PlayerStats>();
    }

    public void Drill()
    {
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 1.8f, Color.yellow);
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 1.8f, 1 << 15))
        {
            if (hit.transform.CompareTag("Metal Ore"))
            {
                if (playingDrillSound == false)
                {
                    AudioManager.Instance.Play("Drill");
                    playingDrillSound = true;
                }

                timer += Time.fixedDeltaTime;
                if (timer >= 3)
                {
                    timer = 0;
                    //Destroy(hit.transform.gameObject);
                    Debug.Log("Instantiate Metal Resource Box...");
                    ResourceManager.Instance.CreateResourceBox(ResourceTypes.MetalOre, transform.position + transform.right, transform.rotation);
                }
            }
            else
            {
                timer = 0;
                AudioManager.Instance.Stop("Drill");
                playingDrillSound = false;
            }
        }
        else
        {
            timer = 0;
            AudioManager.Instance.Stop("Drill");
            playingDrillSound = false;
        }
    }

    public void Weld()
    {
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 1.8f, Color.yellow);
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 1.8f, 1 << 12 ))
        {
            if (hit.transform.tag == "Contruction Site")
            {
                hit.transform.GetComponent<ConstructionSite>().ConstructBuilding(Time.deltaTime * 5);
                MainUIManager.Instance.UpdateBuildPercentage(hit.transform.GetComponent<ConstructionSite>().buildPercentage);
            }
            else
            {
                hit.transform.GetComponent<ConstructionSite>().StopWelderSound();
            }
        }
        else
        {
            AudioManager.Instance.Stop("Welder");
        }
    }

    public void Dig()
    {
        Debug.Log("Digging in the ground");
    }

    public void CarryObject(GameObject box)
    {
        grabbedObject = box;
        Destroy(box.GetComponent<Rigidbody>());
        box.gameObject.transform.parent = gameObject.transform;
        box.transform.localPosition = new Vector3(0, 0, 1);
        box.transform.localRotation = Quaternion.identity;
    }

    public void DropObject()
    {
        grabbedObject.transform.parent = null;
        grabbedObject.AddComponent<Rigidbody>();
        StartCoroutine(DelayDroppedObject());
    }

    private IEnumerator DelayDroppedObject()
    {
        yield return new WaitForSeconds(0.1f);
        isCarryingObject = false;
    }

    public void UseAirlockPanel()
    {
        if (!playerController.insideBase)
        {
            tempOxygenRate = playerStats.oxygenRate;
            playerStats.oxygenRate = 0;
            playerStats.Oxygen = 100;
            playerController.insideBase = true;
            MainUIManager.Instance.UpdateStatsPanel(playerStats.Health, playerStats.Oxygen, playerStats.Hunger, playerStats.Thirst, playerStats.Sleep);
        }
        else
        {
            playerController.insideBase = false;
            playerStats.oxygenRate = tempOxygenRate;
        }
    }
}
