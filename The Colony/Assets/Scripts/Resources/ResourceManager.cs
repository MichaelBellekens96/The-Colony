using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceTypes
{
    BioPlastic = 0,
    Corn,
    Metal
}

public class ResourceManager : MonoBehaviour {

    public static ResourceManager Instance;

    [Header("All resource types")]
    public ResourceData[] resourceTypes;
    
    [Header("All active resource boxes")]
    public List<Transform> MetalBox = new List<Transform>();
    public List<Transform> BioPlasticBox = new List<Transform>();
    public List<Transform> CornBox = new List<Transform>();
    public int numMetalBoxes;
    public int numBioplasticBoxes;
    public int numCornBoxes;

    public GameObject standardBox;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            RemoveResourceBox(MetalBox[0]);
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        MainUIManager.Instance.UpdateResourcePanel(numMetalBoxes, 0, numBioplasticBoxes, numCornBoxes, 0, 0);
        
    }

    public void CreateResourceBox(ResourceTypes type, Transform trans)
    {
        GameObject resourceBox = Instantiate(standardBox, trans.position, trans.rotation) as GameObject;
        resourceBox.GetComponent<ResourceBox>().LoadData(resourceTypes[GetResourceType(type)]);
        AddToResourceList(type, resourceBox.transform);
    }

    public void RemoveResourceBox(Transform trans)
    {
        ResourceTypes resourceType = trans.GetComponent<ResourceBox>().type;
        switch (resourceType)
        {
            case ResourceTypes.BioPlastic:
                BioPlasticBox.Remove(trans);
                numBioplasticBoxes--;
                break;
            case ResourceTypes.Corn:
                CornBox.Remove(trans);
                numCornBoxes--;
                break;
            case ResourceTypes.Metal:
                MetalBox.Remove(trans);
                numMetalBoxes--;
                break;
            default:
                break;
        }
        Destroy(trans.gameObject);
        MainUIManager.Instance.UpdateResourcePanel(numMetalBoxes, 0, numBioplasticBoxes, numCornBoxes, 0, 0);
    }

    private int GetResourceType(ResourceTypes type)
    {
        string resourceType = type.ToString();
        int index = 0;

        for (int i = 0; i < resourceTypes.Length; i++)
        {
            if (resourceType == resourceTypes[i].name)
            {
                index = i;
            }
        }
        return index;
    }


    private void AddToResourceList(ResourceTypes type, Transform box)
    {
        switch (type)
        {
            case ResourceTypes.BioPlastic:
                BioPlasticBox.Add(box);
                numBioplasticBoxes++;
                break;
            case ResourceTypes.Corn:
                CornBox.Add(box);
                numCornBoxes++;
                break;
            case ResourceTypes.Metal:
                MetalBox.Add(box);
                numMetalBoxes++;
                break;
            default:
                break;
        }
        MainUIManager.Instance.UpdateResourcePanel(numMetalBoxes, 0, numBioplasticBoxes, numCornBoxes, 0, 0);
    }
	
}
