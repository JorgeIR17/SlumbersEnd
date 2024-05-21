using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject loading;

    public AudioSource buttonSound;

    void Start()
    {
        mainMenu.SetActive(true);
        loading.SetActive(false); 
    }

    public void StartButton()
    {
        loading.SetActive(true);
        mainMenu.SetActive(false);
        //buttonSound.Play();
        SceneManager.LoadScene("DialogoInicio");
        Debug.Log("App");
    }

    public void ExitGameButton()
    {
        //buttonSound.Play();
        Application.Quit();
        Debug.Log("App Has Exited");
    }

}