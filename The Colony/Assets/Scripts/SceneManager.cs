using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {

	public void StartGame()
    {
        AudioManager.Instance.Play("Btn_Press");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        GameData.Instance.newGame = true;
    }

    public void LoadPreviousGame()
    {
        AudioManager.Instance.Play("Btn_Press");
        GameData.Instance.loadPreviousGame = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        AudioManager.Instance.Play("Btn_Press");
        Application.Quit();
    }
}
