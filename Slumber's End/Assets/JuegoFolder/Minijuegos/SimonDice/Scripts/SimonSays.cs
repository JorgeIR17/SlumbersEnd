using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SimonSays : MonoBehaviour
{
    public Image PreviaImagen;
    public GameObject NoImagen;
    public List<Color> Colores;
    public int NumNivel;
    public int MostrarColores;
    public int Nose;
    public Text NoseText;
    public List<int> Orden;
    public Text LevelText;
    public GameObject EndScene;
    public GameObject Winner;
    public GameObject canva;
    public int highscore;
    public UnityEvent OnCanvasDestroyed;
    private bool gameWon = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if (OnCanvasDestroyed == null)
            OnCanvasDestroyed = new UnityEvent();
        Orden = new List<int>();
        StartCoroutine(startin());
    }

    public void Generator()
    {
        if (gameWon) return;  // Si el juego ha sido ganado, no continuar

        NumNivel++;
        LevelText.text = "Nivel: " + NumNivel;
        Orden.Add(Random.Range(0, 4));
        MuestraPrevia();

        if (NumNivel == 4)
        {
            Ganador();
        }
    }

    public void MuestraPrevia()
    {
        if (gameWon) return;  // Si el juego ha sido ganado, no continuar

        if (Orden.Count <= MostrarColores)
        {
            PreviaImagen.color = Color.white;
            MostrarColores = 0;
            Nose = Orden.Count;
            NoseText.text = Nose.ToString();
            NoImagen.SetActive(false);
        }
        else
        {
            int colorIndex = Orden[MostrarColores];

            if (colorIndex == 0)
                PreviaImagen.color = Color.blue;
            if (colorIndex == 1)
                PreviaImagen.color = Color.red;
            if (colorIndex == 2)
                PreviaImagen.color = Color.green;
            if (colorIndex == 3)
                PreviaImagen.color = Color.yellow;

            StartCoroutine(MostrarProximo());
        }
    }

    public void BotonColor(int ID)
    {
        if (gameWon) return;  // Si el juego ha sido ganado, no continuar

        if (ID == Orden[MostrarColores])
        {
            MostrarColores++;
            Nose--;
            NoseText.text = Nose.ToString();

            if (MostrarColores == Orden.Count)
            {
                NoImagen.SetActive(true);
                NoseText.text = "";
                Nose = 0;
                MostrarColores = 0;
                StartCoroutine(startin());
            }
        }
        else
        {
            EndScene.SetActive(true);
            NoImagen.SetActive(true);
            NoseText.text = "";
            Nose = 0;
            MostrarColores = 0;
        }
    }

    public void Again()
    {
        gameWon = false;  // Reiniciar la variable de control
        Orden = new List<int>();
        NumNivel = 0;
        LevelText.text = "Nivel: " + NumNivel;
        EndScene.SetActive(false);
        StartCoroutine(startin());
    }

    public void Ganador()
    {
        gameWon = true;  // Establecer que el juego ha sido ganado
        NoImagen.SetActive(true);
        Winner.SetActive(true);
        NoseText.text = "";
        Nose = 0;
        MostrarColores = 0;
    }

    public void Destruir()
    {
        OnCanvasDestroyed.Invoke();
        Cursor.lockState = CursorLockMode.Locked;
        Destroy(canva);
    }

    IEnumerator startin()
    {
        if (gameWon) yield break;  // Si el juego ha sido ganado, no continuar

        yield return new WaitForSeconds(0.5f);
        Generator();
    }

    IEnumerator MostrarProximo()
    {
        if (gameWon) yield break;  // Si el juego ha sido ganado, no continuar

        yield return new WaitForSeconds(0.3f);
        PreviaImagen.color = Color.white;
        yield return new WaitForSeconds(0.7f);
        MostrarColores++;
        MuestraPrevia();
    }
}