using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityInteraction : MonoBehaviour
{
    public GameObject interactionCanvas;  // Canvas que se activa al presionar 'E'
    public float activationDistance = 3f;  // Distancia a la que se puede activar el Canvas
    private Transform player;
    public GameObject activarScript;

    void Start()
    {
    
        if (interactionCanvas != null)
        {
            interactionCanvas.SetActive(false);  // Asegúrate de que el interactionCanvas esté desactivado al inicio
            if(activarScript != null) {
                activarScript.SetActive(false);
            }
        }

        // Buscar al jugador por su etiqueta "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player no encontrado. Asegúrate de que el jugador tenga la etiqueta 'Player'.");
        }
    }

    void Update()
    {
        if (player != null && interactionCanvas != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= activationDistance)
            {

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactionCanvas.SetActive(true);  // Activa el interactionCanvas
                    if(activarScript != null) {
                        activarScript.SetActive(true);
                    }
                }
            }
        }
    }

    bool PuedeInteractuar() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, activationDistance))
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }
}