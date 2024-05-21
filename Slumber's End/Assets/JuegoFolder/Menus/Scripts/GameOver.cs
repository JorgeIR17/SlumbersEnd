using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartButton()
    {
        if(SceneManager.GetActiveScene().name == "GameOverBosque")
            SceneManager.LoadScene("Bosque");
        if(SceneManager.GetActiveScene().name == "GameOverAsilo")
            SceneManager.LoadScene("Asilo");
        if(SceneManager.GetActiveScene().name == "GameOverLab")
            SceneManager.LoadScene("Laboratorio");
    }


    public void ExitGameButton()
    {
        SceneManager.LoadScene("MenuInicial");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
