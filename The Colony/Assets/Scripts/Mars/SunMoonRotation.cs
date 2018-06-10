using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMoonRotation : MonoBehaviour {

    public float timeScale;

    public GameObject Moon;
    public GameObject Sun;
    public Light sunLight;

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.F9)) Save();
        if (Input.GetKeyDown(KeyCode.F10)) Load();

        Moon.transform.RotateAround(Vector3.zero, Vector3.right, timeScale * Time.deltaTime);
        Moon.transform.LookAt(Vector3.zero);

        Sun.transform.RotateAround(Vector3.zero, Vector3.right, timeScale * Time.deltaTime);
        Sun.transform.LookAt(Vector3.zero);
        if (Sun.transform.position.y < 0)
        {
            sunLight.intensity = 0;
        }
        else if (Sun.transform.position.y < 200)
        {
            sunLight.intensity = Mathf.Lerp(0, 1, Sun.transform.position.y / 200);
        }
        else
        {
            sunLight.intensity = 1;
        }
    }

    public void Save()
    {
        Debug.Log("Saving sky...");
        SaveLoadManager.SaveSunMoon(this);
    }

    public void Load()
    {
        Debug.Log("Loading sky...");
        SunMoonData data = SaveLoadManager.LoadSunMoon();
        if (data != null)
        {
            Sun.transform.position = new Vector3(0, data.sunPosY, data.sunPosZ);
            Moon.transform.position = new Vector3(0, data.moonPosY, data.moonPosZ);
        }
    }
}
