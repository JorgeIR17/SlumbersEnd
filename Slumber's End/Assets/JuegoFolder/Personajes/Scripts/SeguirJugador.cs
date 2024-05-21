using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirJugador : MonoBehaviour
{
    private Vector2 angle = new Vector2(90 * Mathf.Deg2Rad, 0);

    public Transform follow;
    public float distance = 5f; // Distancia deseada de la cámara al jugador
    public Vector2 sensitivity;
    public LayerMask collisionMask; // Capa de colisión para las paredes y otros obstáculos

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float hor = Input.GetAxis("Mouse X");

        if (hor != 0)
        {
            angle.x += hor * Mathf.Deg2Rad * sensitivity.x;
        }

        float ver = Input.GetAxis("Mouse Y");

        if (ver != 0)
        {
            angle.y += ver * Mathf.Deg2Rad * sensitivity.y;
            angle.y = Mathf.Clamp(angle.y, -80 * Mathf.Deg2Rad, 80 * Mathf.Deg2Rad);
        }
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = CalculateCameraPosition();
        Vector3 correctedPosition = CorrectCameraPosition(desiredPosition);

        transform.position = correctedPosition;
        transform.rotation = Quaternion.LookRotation(follow.position - transform.position);
    }

    Vector3 CalculateCameraPosition()
    {
        return follow.position + new Vector3(
            Mathf.Cos(angle.x) * Mathf.Cos(angle.y),
            -Mathf.Sin(angle.y),
            -Mathf.Sin(angle.x) * Mathf.Cos(angle.y)
        ) * distance;
    }

    Vector3 CorrectCameraPosition(Vector3 desiredPosition)
    {
        RaycastHit hit;
        Vector3 direction = desiredPosition - follow.position;
        float maxDistance = direction.magnitude;

        if (Physics.Raycast(follow.position, direction, out hit, maxDistance, collisionMask))
        {
            // Si el raycast golpea una pared, coloca la cámara en el punto de impacto
            return hit.point;
        }

        // Si no hay colisión, usa la posición deseada
        return desiredPosition;
    }
}