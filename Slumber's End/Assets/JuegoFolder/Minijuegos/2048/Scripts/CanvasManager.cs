using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI puzzlesText; // Canvas Text for puzzles

    private int destroyedCanvasCount = 0;
    private bool canvas1Destroyed = false;
    private bool canvas2Destroyed = false;

    [SerializeField] private GameObject DestroyCanvas1;
    [SerializeField] private GameObject DestroyCanvas2;

    public AudioSource completed;

    private void Start()
    {
        UpdatePuzzlesText();
    }

    private void Update()
    {
        if (!canvas1Destroyed && DestroyCanvas1 == null)
        {
            completed.Play();
            destroyedCanvasCount++;
            canvas1Destroyed = true;
            UpdatePuzzlesText();
        }
        if (!canvas2Destroyed && DestroyCanvas2 == null)
        {
            completed.Play();
            destroyedCanvasCount++;
            canvas2Destroyed = true;
            UpdatePuzzlesText();
        }
        if (destroyedCanvasCount == 2)
        {
            SceneManager.LoadScene("Laboratorio");
        }
    }

    private void UpdatePuzzlesText()
    {
        puzzlesText.text = $"Puzzles: {destroyedCanvasCount}/2";
    }
}