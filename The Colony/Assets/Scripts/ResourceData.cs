using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Resource")]
public class ResourceData : ScriptableObject {

    public string resourceName;
    public string description;

    public float value;
    public Color color;
    public Sprite sprite;

    public void PrintData()
    {
        string message = string.Format("Name: {0}, Description: {1}, value: {2}, Color: {3}", resourceName, description, value.ToString(), color.ToString());
        Debug.Log(message);
    }
}
