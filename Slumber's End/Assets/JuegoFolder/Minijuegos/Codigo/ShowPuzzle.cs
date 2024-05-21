using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class Canvas : MonoBehaviour
{
    public GameObject WinnerCanvas;
    public GameObject LoserCanvas;
    public GameObject PuzzleCanvas;
    public UnityEvent OnCanvasDestroyed;
    public AudioSource completed;

    public Text UserInputText;
    public string SecretCode = "83638";

    public CharacterController PlayerController;

    public bool SecretCodeEntered = false;

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(UserInputText.text == SecretCode && !SecretCodeEntered)
            {
                Debug.Log("El codigo secreto es correcto");
                SecretCodeEntered = true;
                WinnerCanvas.SetActive(true);
                completed.Play();
                PlayerController.enabled = true;
                Destroy(PuzzleCanvas.gameObject);
                OnCanvasDestroyed.Invoke();
                SceneManager.LoadScene("DialogoFin");
            }
            else if(UserInputText.text != SecretCode && SecretCodeEntered){
                Debug.Log("El codigo secreto es incorrecto");
                LoserCanvas.SetActive(true);
                PuzzleCanvas.SetActive(false);
                PlayerController.enabled = true;
            }
        }
    }

    void OnTriggerStay(Collider EsaCosa)
    {
        if(EsaCosa.tag =="Player" && !SecretCodeEntered)
        {
            //check press E
            if(Input.GetKey(KeyCode.E))
            {
                //show canvas
                PuzzleCanvas.SetActive(true);
                //stop the player 
                //PlayerController.enabled = false;
                //Give cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            // else if(Input.GetKey(KeyCode.Escape))
            // {
            //     ExitButton();
            // }
        }
    }

    public void ExitButton()
    {
        WinnerCanvas.SetActive(false);
        PlayerController.enabled = true;
        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
