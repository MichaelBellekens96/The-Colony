using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTasks : MonoBehaviour {
    
    private GameObject grabbedObject;
    public bool isCarryingObject = false;
    private RaycastHit hit;
    private Camera playerCamera;
    public float timer = 0;

    private void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    public void Drill()
    {
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 1.8f, Color.yellow);
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 1.8f, 1 << 10))
        {
            if (hit.transform.CompareTag("Metal Ore"))
            {
                timer += Time.fixedDeltaTime;
                if (timer >= 3)
                {
                    timer = 0;
                    Destroy(hit.transform.gameObject);
                    Debug.Log("Instantiate Metal Resource Box...");
                    ResourceManager.Instance.CreateResourceBox(ResourceTypes.Metal, hit.transform);
                }
            }
            else
            {
                timer = 0;
            }
        }
        else
        {
            timer = 0;
        }
    }

    public void Weld()
    {
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 1.8f, Color.yellow);
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 1.8f, 1 << 11 ))
        {
            if (hit.transform.tag == "Contruction Site")
            {
                hit.transform.GetComponent<ConstructionSite>().ConstructBuilding(Time.deltaTime);
            }
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
        isCarryingObject = true;
    }

    public void DropObject()
    {
        grabbedObject.transform.parent = null;
        grabbedObject.AddComponent<Rigidbody>();
        isCarryingObject = false;
    }
}
