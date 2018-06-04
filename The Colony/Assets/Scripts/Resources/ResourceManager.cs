using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceTypes
{
    BioPlastic = 0,
    RawFood,
    Metal,
    MetalOre,
    Meal
}

public class ResourceManager : MonoBehaviour {

    public static ResourceManager Instance;

    [Header("All resource types")]
    public ResourceData[] resourceTypes;
    
    [Header("All active resource boxes")]
    public List<Transform> MetalBox = new List<Transform>();
    public List<Transform> BioPlasticBox = new List<Transform>();
    public List<Transform> RawFoodBox = new List<Transform>();
    public List<Transform> MetalOreBox = new List<Transform>();
    public int numMetalBoxes;
    public int numMetalOreBoxes;
    public int numBioplasticBoxes;
    public int numRawFoodBoxes;
    public int numMeals;

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
        MainUIManager.Instance.UpdateResourcePanel(numMetalBoxes, numMetalOreBoxes, numBioplasticBoxes, numRawFoodBoxes, numMeals, 0);
        
    }

    public void CreateResourceBox(ResourceTypes type, Vector3 pos, Quaternion rot)
    {
        GameObject resourceBox = Instantiate(standardBox, pos, rot) as GameObject;
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
            case ResourceTypes.RawFood:
                RawFoodBox.Remove(trans);
                numRawFoodBoxes--;
                break;
            case ResourceTypes.Metal:
                MetalBox.Remove(trans);
                numMetalBoxes--;
                break;
            case ResourceTypes.MetalOre:
                MetalOreBox.Remove(trans);
                numMetalOreBoxes--;
                break;
            default:
                break;
        }
        Destroy(trans.gameObject);
        MainUIManager.Instance.UpdateResourcePanel(numMetalBoxes, numMetalOreBoxes, numBioplasticBoxes, numRawFoodBoxes, numMeals, 0);
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
            case ResourceTypes.RawFood:
                RawFoodBox.Add(box);
                numRawFoodBoxes++;
                break;
            case ResourceTypes.Metal:
                MetalBox.Add(box);
                numMetalBoxes++;
                break;
            case ResourceTypes.MetalOre:
                MetalOreBox.Add(box);
                numMetalOreBoxes++;
                break;
            default:
                break;
        }
        MainUIManager.Instance.UpdateResourcePanel(numMetalBoxes, 0, numBioplasticBoxes, numRawFoodBoxes, 0, 0);
    }

    public void AddMealToList()
    {
        numMeals++;
        MainUIManager.Instance.UpdateResourcePanel(numMetalBoxes, numMetalOreBoxes, numBioplasticBoxes, numRawFoodBoxes, numMeals, 0);
    }
	
    public void RemoveMealFromList()
    {
        numMeals--;
        MainUIManager.Instance.UpdateResourcePanel(numMetalBoxes, numMetalOreBoxes, numBioplasticBoxes, numRawFoodBoxes, numMeals, 0);
    }
}
