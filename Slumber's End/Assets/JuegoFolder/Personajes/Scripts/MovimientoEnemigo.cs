using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class MovimientoEnemigo : MonoBehaviour
{
    private int rutina;
    private float cronometro;
    private Animator animator;
    private Quaternion angulo;
    private float grado;
    public int walkspeed = 1;
    public int runspeed = 2;
    private bool atacando;

    private GameObject target;
    public NavMeshAgent agente;

    public AudioSource walk_hall;
    public AudioSource run_hall;
    public AudioSource walk_grass;
    public AudioSource run_grass;

    private string currentScene;

    void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void Comportamiento()
    {
        if (!atacando)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > 5)
            {
                animator.SetBool("Persiguiendo", false);
                cronometro += 1 * Time.deltaTime;
                if (cronometro >= 4)
                {
                    rutina = Random.Range(0, 2);
                    cronometro = 0;
                }

                switch (rutina)
                {
                    case 0:
                        animator.SetBool("Andando", false);
                        StopMovementSounds();
                        break;
                    case 1:
                        grado = Random.Range(0, 360);
                        angulo = Quaternion.Euler(0, grado, 0);
                        rutina++;
                        break;
                    case 2:
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                        transform.Translate(Vector3.forward * walkspeed * Time.deltaTime);
                        animator.SetBool("Andando", true);
                        PlayMovementSounds(walkspeed);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                agente.enabled = true;
                agente.SetDestination(target.transform.position);

                if (Vector3.Distance(transform.position, target.transform.position) > 1.5f && !atacando)
                {
                    animator.SetBool("Andando", false);
                    animator.SetBool("Persiguiendo", true);
                    PlayMovementSounds(runspeed);
                    animator.SetBool("Atacando", false);
                }
                else
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                    animator.SetBool("Andando", false);
                    animator.SetBool("Persiguiendo", false);
                    animator.SetBool("Atacando", true);
                    atacando = true;
                    StopMovementSounds();
                }
            }
        }
        else
        {
            agente.enabled = false;
            Debug.Log("El enemigo está atacando...");
            if (Vector3.Distance(transform.position, target.transform.position) < 1.5f && atacando)
            {
                Invoke("CargarEscenaGameOver", 1f);
            }
            else
                atacando = false;
        }
    }

    void CargarEscenaGameOver()
    {
        if (SceneManager.GetActiveScene().name == "Bosque")
            SceneManager.LoadScene("GameOverBosque");
        if (SceneManager.GetActiveScene().name == "Asilo")
            SceneManager.LoadScene("GameOverAsilo");
        if (SceneManager.GetActiveScene().name == "Laboratorio")
            SceneManager.LoadScene("GameOverLab");
        if (SceneManager.GetActiveScene().name == "pruebas")
            SceneManager.LoadScene("GameOver");
    }

    void Update()
    {
        Comportamiento();
    }

    void PlayMovementSounds(float speed)
    {
        bool isRunning = (speed == runspeed);

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
