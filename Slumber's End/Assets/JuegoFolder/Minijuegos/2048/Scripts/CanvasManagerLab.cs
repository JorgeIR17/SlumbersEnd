using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManagerLab : MonoBehaviour
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
            destroyedCanvasCount++;
            canvas1Destroyed = true;
            completed.Play();
            UpdatePuzzlesText();
        }
        if (!canvas2Destroyed && DestroyCanvas2 == null)
        {
            destroyedCanvasCount++;
            canvas2Destroyed = true;
            completed.Play();
            UpdatePuzzlesText();
        }
        if (destroyedCanvasCount == 2)
        {
            SceneManager.LoadScene("Bosque");
        }
    }

    private void UpdatePuzzlesText()
    {
        puzzlesText.text = $"Puzzles: {destroyedCanvasCount}/2";
    }
}