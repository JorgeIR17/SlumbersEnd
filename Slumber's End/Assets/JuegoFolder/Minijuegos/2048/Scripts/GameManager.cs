using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject Canvas;
    [SerializeField] private TileBoard board;
    [SerializeField] private CanvasGroup winner;
    [SerializeField] private CanvasGroup gameOver;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI hiscoreText;

    private int score;
    public int Score => score;
    public UnityEvent OnCanvasDestroyed;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (OnCanvasDestroyed == null)
            OnCanvasDestroyed = new UnityEvent();
    }

    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        // Reset score
        SetScore(0);
        hiscoreText.text = LoadHiscore().ToString();

        // Hide game over and winner screens
        gameOver.alpha = 0f;
        gameOver.interactable = false;

        winner.alpha = 0f;
        winner.interactable = false;

        // Update board state
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;

        // Ensure game is not paused
        ResumeGame();
    }

    public void Winner()
    {
        board.enabled = false;
        winner.interactable = true;
        StartCoroutine(HandleWinner());
    }

    private IEnumerator HandleWinner()
    {
        yield return StartCoroutine(Fade(winner, 1f, 1f));
        PauseGame();
        yield return new WaitForSecondsRealtime(2); // Use unscaled time
        DestroyCanvas();
        ResumeGame();
    }

    public void GameOver()
    {
        board.enabled = false;
        gameOver.interactable = true;
        StartCoroutine(HandleGameOver());
    }

    private IEnumerator HandleGameOver()
    {
        yield return StartCoroutine(Fade(gameOver, 1f, 1f));
        PauseGame();
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay = 0f)
    {
        yield return new WaitForSecondsRealtime(delay); // Use unscaled time

        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.unscaledDeltaTime; // Use unscaled time
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    public void IncreaseScore(int points)
    {
        SetScore(score + points);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();

        SaveHiscore();
    }

    private void SaveHiscore()
    {
        int hiscore = LoadHiscore();

        if (score > hiscore)
        {
            PlayerPrefs.SetInt("hiscore", score);
        }
    }

    private int LoadHiscore()
    {
        return PlayerPrefs.GetInt("hiscore", 0);
    }

    public bool ReachScore()
    {
        return score >= 1000;
    }

    public void DestroyCanvas()
    {
        Destroy(Canvas.gameObject);
        OnCanvasDestroyed.Invoke(); // Launch the event
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }
}