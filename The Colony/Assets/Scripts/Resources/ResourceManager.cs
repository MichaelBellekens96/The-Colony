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

    public int numStoredMetalBoxes;
    public int numStoredMetalOreBoxes;
    public int numStoredBioplasticBoxes;
    public int numStoredRawFoodBoxes;

    public GameObject standardBox;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            RemoveResourceBox(MetalBox[0]);
        }

        if (Input.GetKeyDown(KeyCode.F9)) Save();
        if (Input.GetKeyDown(KeyCode.F10)) Load();
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

    public void Save()
    {
        Debug.Log("Saving resources...");
        SaveLoadManager.SaveResources(this);
    }

    public void Load()
    {
        Debug.Log("Loading resources...");
        ResourceBoxData data = SaveLoadManager.LoadResources();
        if (data != null)
        {
            Debug.Log("Resources not null");
            Transform resourceBox;
            for (int i = 0; i < MetalBox.Count; i++)
            {
                resourceBox = MetalBox[i];
                Destroy(resourceBox.gameObject);
            }
            for (int i = 0; i < MetalOreBox.Count; i++)
            {
                resourceBox = MetalOreBox[i];
                Destroy(resourceBox.gameObject);
            }
            for (int i = 0; i < BioPlasticBox.Count; i++)
            {
                resourceBox = BioPlasticBox[i];
                Destroy(resourceBox.gameObject);
            }
            for (int i = 0; i < RawFoodBox.Count; i++)
            {
                resourceBox = RawFoodBox[i];
                Destroy(resourceBox.gameObject);
            }

            MetalBox.Clear();
            MetalOreBox.Clear();
            BioPlasticBox.Clear();
            RawFoodBox.Clear();

            numMetalBoxes = data.numMetal;
            numMetalOreBoxes = data.numMetalOre;
            numBioplasticBoxes = data.numBioplastic;
            numRawFoodBoxes = data.numRawFood;
            numMeals = data.numMeals;

            numStoredMetalBoxes = data.numStoredMetal;
            numStoredMetalOreBoxes = data.numStoredMetalOre;
            numStoredBioplasticBoxes = data.numStoredBioplastic;
            numStoredRawFoodBoxes = data.numStoredRawFood;

            MainUIManager.Instance.UpdateResourcePanel(numMetalBoxes, numMetalOreBoxes, numBioplasticBoxes, numRawFoodBoxes, numMeals, 0);

            for (int i = 0; i < data.metalPosX.Length; i++)
            {
                CreateExistingResourceBox(ResourceTypes.Metal, new Vector3(data.metalPosX[i], data.metalPosY[i], data.metalPosZ[i]), Quaternion.identity);
            }
            for (int i = 0; i < data.metalOrePosX.Length; i++)
            {
                CreateExistingResourceBox(ResourceTypes.MetalOre, new Vector3(data.metalOrePosX[i], data.metalOrePosY[i], data.metalOrePosZ[i]), Quaternion.identity);
            }
            for (int i = 0; i < data.BioplasticPosX.Length; i++)
            {
                CreateExistingResourceBox(ResourceTypes.BioPlastic, new Vector3(data.BioplasticPosX[i], data.BioplasticPosY[i], data.BioplasticPosZ[i]), Quaternion.identity);
            }
            for (int i = 0; i < data.rawFoodPosX.Length; i++)
            {
                CreateExistingResourceBox(ResourceTypes.RawFood, new Vector3(data.rawFoodPosX[i], data.rawFoodPosY[i], data.rawFoodPosZ[i]), Quaternion.identity);
            }
        }
    }

    public void CreateExistingResourceBox(ResourceTypes type, Vector3 pos, Quaternion rot)
    {
        GameObject resourceBox = Instantiate(standardBox, pos, rot) as GameObject;
        resourceBox.GetComponent<ResourceBox>().LoadData(resourceTypes[GetResourceType(type)]);
        if (type == ResourceTypes.Metal)
        {
            MetalBox.Add(resourceBox.transform);
        }
        else if (type == ResourceTypes.MetalOre)
        {
            MetalOreBox.Add(resourceBox.transform);
        }
        else if (type == ResourceTypes.BioPlastic)
        {
            BioPlasticBox.Add(resourceBox.transform);
        }
        else if (type == ResourceTypes.RawFood)
        {
            RawFoodBox.Add(resourceBox.transform);
        }
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
        MainUIManager.Instance.UpdateResourcePanel(numMetalBoxes, numMetalOreBoxes, numBioplasticBoxes, numRawFoodBoxes, numMeals, 0);
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
