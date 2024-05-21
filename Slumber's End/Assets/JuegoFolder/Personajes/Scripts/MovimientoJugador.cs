using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovimientoJugador : MonoBehaviour
{
    public new Transform camera;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float climbSpeed = 5f;
    public float jumpForce = 10f;
    public float gravity = -30f;
    public float crouchSpeedMultiplier = 0.5f;
    public float climbRaycastDistance = 1f;

    private CharacterController controller;
    private bool isCrouching;
    private bool isClimbing;
    private Vector3 velocity;
    private Vector3 direction = Vector3.zero;
    private Animator animator;

    public AudioSource walk_hall;
    public AudioSource run_hall;
    public AudioSource walk_grass;
    public AudioSource run_grass;

    private string currentScene;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentScene = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        // Controles
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        if (horizontalInput != 0 || verticalInput != 0)
        {
            Vector3 forward = camera.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = camera.right;
            right.y = 0;
            right.Normalize();

            direction = forward * verticalInput + right * horizontalInput;
            direction.Normalize();

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.2f);
        }
        else
        {
            direction = Vector3.zero;
        }

        float speed = walkSpeed;
        // Andar y correr
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            speed = runSpeed;
        }
        else if (isCrouching)
        {
            speed *= crouchSpeedMultiplier;
        }

        Vector3 movement = direction * (speed * Mathf.Clamp01(direction.magnitude));

        if (movement == Vector3.zero)
        {
            animator.SetFloat("Velocidad", 0f);
            StopMovementSounds();
        }
        else
        {
            animator.SetFloat("Velocidad", speed);
            PlayMovementSounds(speed);
        }

        // Saltar
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space) && !isClimbing)
        {
            animator.SetBool("Saltando", true);
            velocity.y = jumpForce;
        }
        else
        {
            animator.SetBool("Saltando", false);
        }

        // Trepar
        if (CanClimb() && !isCrouching)
        {
            animator.SetBool("Escalando", true);
            if (Input.GetKey(KeyCode.G))
            {
                isClimbing = true;
                velocity.y = climbSpeed;
                animator.SetBool("Subiendo", true);
                animator.SetBool("Bajando", false);
            }
            else if (Input.GetKey(KeyCode.B))
            {
                isClimbing = true;
                velocity.y = -climbSpeed;
                animator.SetBool("Subiendo", false);
                animator.SetBool("Bajando", true);
            }
            else
            {
                velocity.y = 0f; // Si no presiona ninguna se mantiene en el sitio
                animator.SetBool("Subiendo", false);
                animator.SetBool("Bajando", false);
            }
        }
        else
        {
            animator.SetBool("Escalando", false);
        }

        // Gravedad
        if (!isClimbing)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // Movimiento
        controller.Move((movement + velocity) * Time.deltaTime);

        // Si toca el suelo deja de escalar
        if (controller.isGrounded)
        {
            isClimbing = false;
        }

        // Dejar de escalar si suelta la pared o toca con un techo
        if (isClimbing && (!CanClimb() || IsTouchingCeiling()))
        {
            isClimbing = false;
            velocity.y = 0f;
        }

        // Agacharse
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            animator.SetBool("Agachado", isCrouching);
        }
    }

    bool CanClimb()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, climbRaycastDistance))
        {
            return hit.collider.CompareTag("Escalable");
        }
        return false;
    }

    bool IsTouchingCeiling()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, Vector3.up, out hit, controller.height / 2f + 0.1f);
    }

    void PlayMovementSounds(float speed)
    {
        bool isRunning = (speed == runSpeed);

        if (currentScene == "Bosque")
        {
            if (isRunning)
            {
                if (!run_grass.isPlaying)
                {
                    walk_grass.Stop();
                    run_grass.Play();
                }
            }
            else
            {
                if (!walk_grass.isPlaying)
                {
                    run_grass.Stop();
                    walk_grass.Play();
                }
            }
        }
        else
        {
            if (isRunning)
            {
                if (!run_hall.isPlaying)
                {
                    walk_hall.Stop();
                    run_hall.Play();
                }
            }
            else
            {
                if (!walk_hall.isPlaying)
                {
                    run_hall.Stop();
                    walk_hall.Play();
                }
            }
        }
    }

    void StopMovementSounds()
    {
        if (currentScene == "Bosque")
        {
            if (walk_grass.isPlaying) walk_grass.Stop();
            if (run_grass.isPlaying) run_grass.Stop();
        }
        else
        {
            if (walk_hall.isPlaying) walk_hall.Stop();
            if (run_hall.isPlaying) run_hall.Stop();
        }
    }
}
