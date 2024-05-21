using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MemoryGameManagerUI : MonoBehaviour
{
    public static MemoryGameManagerUI Instance { get; private set; }

    [SerializeField] private CardGroup cardGroup;
    [SerializeField] private List<CardSingleUI> cardSingleUIList = new List<CardSingleUI>();
    [SerializeField] private GameObject gameArea;

    [SerializeField] private GameObject winnerMessage; // Añade esta línea
    [SerializeField] private Canvas canva;

    public UnityEvent OnCanvasDestroyed;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        cardGroup.OnCardMatch += CardGroup_OnCardMatch;
    }

    public void Subscribe(CardSingleUI cardSingleUI)
    {
        if (cardSingleUIList == null)
        {
            cardSingleUIList = new List<CardSingleUI>();
        }

        if (!cardSingleUIList.Contains(cardSingleUI))
        {
            cardSingleUIList.Add(cardSingleUI);
        }
    }

    private void CardGroup_OnCardMatch(object sender, EventArgs e)
    {
        if (cardSingleUIList.All(x => x.GetObjectMatch() == true))
        {
            StartCoroutine(OnCompleteGame());
        }
    }

    public void destruir()
    {
        Destroy(canva.gameObject);
        OnCanvasDestroyed.Invoke();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator OnCompleteGame()
    {
        yield return new WaitForSeconds(0.75f);

        // Hacer cualquier cosa cuando ganes
        Debug.Log("Has ganado");

        // Mostrar el mensaje de ganador
        winnerMessage.SetActive(true);
    }

    public void Restart()
    {
        cardSingleUIList.Clear();
        winnerMessage.SetActive(false); // Reiniciar el mensaje de ganador si se reinicia el juego
    }

    private void Toggle(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    private void ToggleGameArea(bool toggle)
    {
        gameArea.SetActive(toggle);
    }
}
