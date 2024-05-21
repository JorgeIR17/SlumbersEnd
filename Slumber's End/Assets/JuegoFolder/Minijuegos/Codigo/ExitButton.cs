using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{

    public GameObject OutCanvas;

    public CharacterController PlayerController;

    public void ExitButtonCanvas()
    {
        OutCanvas.SetActive(false);
        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.enabled = true;

    }
}

