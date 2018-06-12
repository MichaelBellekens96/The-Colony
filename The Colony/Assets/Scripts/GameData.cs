using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour {

    public static GameData Instance;

    public bool loadPreviousGame;
    public bool loadedSaveFile;
    public bool newGame = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!loadedSaveFile)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1 && loadPreviousGame)
            {
                loadedSaveFile = true;
                MainUIManager.Instance.LoadPreviousSave();
                //StartCoroutine(LoadScene());
            }
        }

        if (newGame)
        {
            newGame = false;
            StartCoroutine(LoadResources());
        }
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(3f);
        MainUIManager.Instance.LoadPreviousSave();
    }

    private IEnumerator LoadResources()
    {
        yield return new WaitForSeconds(1f);
        //Vector3 pos = RandomCircle(transform.position, 25.0f);
        Quaternion rot = Quaternion.identity;
        for (int i = 0; i < 15; i++)
        {
            ResourceManager.Instance.CreateResourceBox(ResourceTypes.Metal, RandomCircle(transform.position, 5f), rot);
            ResourceManager.Instance.CreateResourceBox(ResourceTypes.BioPlastic, RandomCircle(transform.position, 5f), rot);
        }
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = 1f;
        pos.z = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }
}
